using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace ACgifts;

public enum LvExMainColumns:int
{
	None = 0,
	[Description("Forum Name")]
	ForumName = 1,
	[Description("Game Name")]
	GameName = 2,
	[Description("Button")]
	Button = 3,
	[Description("Last")]
	Last = 4,
	[Description("Last Days")]
	LastDays = 5,
	[Description("Last Hours")]
	LastHours = 6,
	[Description("Count")]
	Count = 7,
	[Description("Rate")]
	Rate = 8,
	[Description("Added")]
	Added = 9,
}


public class LvExMain:ListView
{
	private readonly Brush doneButColor = Brushes.LightGreen;
	private readonly Brush todoButColor = Brushes.NavajoWhite;
	private readonly Brush disabledButColor = Brushes.Gray;


	public static readonly string[] COLUMNS = { "", "Name", "Game Name", "Button", "Last", "Last Days", "Last Hours", "Cnt", "Rate", "Added" };
	public const int BUT_DISABLE_MILLIS = 600;



	private readonly int cntColumns;
	private readonly int[] colAutoWidth;
	private bool mb_Measured = false;
	private int ms32_RowHeight = 20;

	#region Windows API

	[StructLayout(LayoutKind.Sequential)]
	struct DRAWITEMSTRUCT
	{
		public int ctlType;
		public int ctlID;
		public int itemID;
		public int itemAction;
		public int itemState;
		public IntPtr hWndItem;
		public IntPtr hDC;
		public int rcLeft;
		public int rcTop;
		public int rcRight;
		public int rcBottom;
		public IntPtr itemData;
	}

	// LVS_OWNERDRAWFIXED: The owner window can paint ListView items in report view. 
	// The ListView control sends a WM_DRAWITEM message to paint each item. It does not send separate messages for each subitem. 
	const int LVS_OWNERDRAWFIXED = 0x0400;
	const int WM_SHOWWINDOW = 0x0018;
	const int WM_DRAWITEM = 0x002B;
	const int WM_MEASUREITEM = 0x002C;
	const int WM_REFLECT = 0x2000;
	const int WM_LBUTTONDOWN = 0x0201;
	const int WM_RBUTTONDOWN = 0x0204;
	const int WM_CONTEXTMENU = 0x7b;
	const int LVM_FIRST = 0x1000;
	const int LVM_DELETEITEM = LVM_FIRST + 8;
	const int LVM_DELETEALLITEMS = LVM_FIRST + 9;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams k_Params = base.CreateParams;
			k_Params.Style |= LVS_OWNERDRAWFIXED;
			return k_Params;
		}
	}


	#endregion


	#region Properties

	[Category("Appearance")]
	[Description("Sets the height of the ListView rows in pixels.")]
	public int RowHeight
	{
		get { return ms32_RowHeight; }
		set
		{
			if(!DesignMode)
				Debug.Assert(mb_Measured == false, "RowHeight must be set before ListViewEx is created.");
			ms32_RowHeight = value;
		}
	}

	[Category("Context Menu")]
	[Description("Shows a context menu for column headers.")]
	public ContextMenuStrip? HeaderContextMenu { get; set; } = null;

	[Category("Appearance")]
	[Description("Selects either Send or Recv data for display. Used for most columns.")]
	public bool IsSend = false;


	#endregion




	public LvExMain()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		cntColumns = Enum.GetNames(typeof(LvExMainColumns)).Length;
		colAutoWidth = new int[cntColumns];
		for(int i= 0; i<cntColumns; i++) colAutoWidth[i] = 0;
	}


	/// <summary>
	/// The messages WM_MEASUREITEM and WM_DRAWITEM are sent to the parent control rather than to the ListView itself.
	/// They come here as WM_REFLECT + WM_MEASUREITEM and WM_REFLECT + WM_DRAWITEM
	/// They are sent from Control.WmOwnerDraw() --> Control.ReflectMessageInternal()
	/// </summary>
	protected override void WndProc(ref Message k_Msg)
	{
		// Fix for ArgOutOfRangeEx when a click is performed to the right of any subitem (empty space)
		if(k_Msg.Msg == WM_LBUTTONDOWN || k_Msg.Msg == WM_RBUTTONDOWN)
		{
			// Get the x position of the end of the last subitem
			int width = -1;
			foreach(ColumnHeader col in Columns)
				width += col.Width;

			// Where did the click occur?
			int x_click = SignedLOWord(k_Msg.LParam);
			int y_click = SignedHIWord(k_Msg.LParam);
			// If the click is to the right of the last subitem, set the x-coordinate to the last subitem.
			if(x_click > width)
				k_Msg.LParam = MakeLparam(width, y_click);
		}

		base.WndProc(ref k_Msg); 

		switch(k_Msg.Msg)
		{
			case WM_CONTEXTMENU:		// Show header context if set
			{
				if(k_Msg.WParam != this.Handle) HeaderContextMenu?.Show(Control.MousePosition);
				break;
			}
			case WM_SHOWWINDOW:			// called when the ListView becomes visible
			{ 
				View = View.Details;
				OwnerDraw = false;
				break;
			}
			case WM_REFLECT + WM_MEASUREITEM:       // called once when the ListView is created, but only in Details view
			{
				mb_Measured = true;
				//Overwrite itemHeight, which is the fifth integer in MEASUREITEMSTRUCT
				Marshal.WriteInt32(k_Msg.LParam + 4 * sizeof(int), ms32_RowHeight);
				k_Msg.Result = (IntPtr)1;
				break;
			}
			case LVM_DELETEITEM:
			case LVM_DELETEALLITEMS:
			{
				// Clear existing column width for autosize.
				for(int i = 0; i < cntColumns; i++) colAutoWidth[i] = 0;
				break;
			}
			case WM_REFLECT + WM_DRAWITEM:          // called for each ListViewItem to be drawn
			{
				#region Prep and preCalcs

				object? lParam = k_Msg.GetLParam(typeof(DRAWITEMSTRUCT)) ?? throw new Exception("lParam shouldn't be null");
				DRAWITEMSTRUCT k_Draw = (DRAWITEMSTRUCT)lParam;

				using Font boldFont = new(Font, FontStyle.Bold);
				using Graphics gfx = Graphics.FromHdc(k_Draw.hDC);
				gfx.SmoothingMode = SmoothingMode.HighQuality;
				//gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

				if(k_Draw.itemID >= Items.Count || k_Draw.itemID < 0)
				{
					// We shouldn't ever get here, but did once for an unknown reason.
					Debug.WriteLine("LvNeighbor.WndProc() Error:   *** Index: " + k_Draw.itemID + " out of range (0 - " + (Items.Count - 1) + ")");
					return;
				}


				if(Items[k_Draw.itemID] is not LviNeighbor lvi)
				{
					if(Items[k_Draw.itemID] is ListViewItem lvi2)
						TextRenderer.DrawText(gfx, Items[k_Draw.itemID].ToString(), boldFont, lvi2.SubItems[0].Bounds, Color.Red);
					else throw new Exception($"Invalid object recieved in LvExMain: {Items[k_Draw.itemID]}");
					return;
				}

				while(lvi.SubItems.Count < cntColumns)
					lvi.SubItems.Add("");

				Neighbor n = lvi.Neighbor;
				double daysSinceAdded = (DateTime.Now - n.Added).TotalDays;
				double giftRate = (IsSend ? n.CntSend : n.CntRecv) / Math.Max(1, daysSinceAdded);


				if(ListViewItemSorter is LvExMainSort lvSort && !lvSort.IsSend && lvSort.SortType == LvExMainSortTypes.RECV_SPECIAL)
				{
					if(!n.HasRecvToday && n.LastRecv != null && (DateTime.Now - (DateTime)n.LastRecv).TotalDays < 2)
						gfx.FillRectangle(Brushes.AntiqueWhite, lvi.SubItems[0].Bounds);
				}



				Color fontCol = Color.Black;
				// If was added over 14days ago warn of low gift rate
				if(daysSinceAdded > 14 && giftRate < 0.5) fontCol = Color.Red;
				// If last gift was over 5days ago, or never gifts and added over 5 days ago warn.
				if((DateTime.Now - ((IsSend ? n.LastSend : n.LastRecv) ?? n.Added)).TotalDays > 5) fontCol = Color.Red;


				// Send/Recv button color
				Brush colBut = todoButColor;
				if(IsSend)
				{
					if(n.LastSend != null && (DateTime.Now - (DateTime)n.LastSend).TotalMilliseconds < BUT_DISABLE_MILLIS) colBut = disabledButColor;
					else if(n.HasSendToday) colBut = doneButColor;
				}
				else
				{
					if(n.LastRecv != null && (DateTime.Now - (DateTime)n.LastRecv).TotalMilliseconds < BUT_DISABLE_MILLIS) colBut = disabledButColor;
					else if(n.HasRecvToday) colBut = doneButColor;
				}

				#endregion

				string txt = "";
				int subitem = 0;	// Lvi Text/subitem[0] is ignored due to Bounds bug, therefor will always be empty.

				subitem++;     // Forum Name
				txt = n.Name;
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font, 
					lvi.SubItems[(int)LvExMainColumns.ForumName].Bounds, fontCol, 
					TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

				

				subitem++;     // Game Name
				txt = IsSend ? n.NameSend : n.NameRecv;
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.GameName].Bounds, fontCol, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;     // Button
				txt = IsSend ? "Send" : "Recv";
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width + 25);
				Rectangle rctBut = lvi.SubItems[(int)LvExMainColumns.Button].Bounds;
				Rectangle rct = new(rctBut.X + 1, rctBut.Y + 1, rctBut.Width - 2, rctBut.Height - 2);
				gfx.FillRectangle(colBut, rct);
				Pen pen = new(Color.Gray, 3) { Alignment = PenAlignment.Inset };
				gfx.DrawRectangle(pen, rct);
				TextRenderer.DrawText(gfx, txt, Font, rct, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;      // Last
				txt = Utils.GetAgeStr(IsSend ? n.LastSend : n.LastRecv);
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.Last].Bounds, fontCol, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				DateTime? last = IsSend ? n.LastSend : n.LastRecv;


				subitem++;      // Last Days
				txt = last is null ? "" : $"{(DateTime.Now - (DateTime)last).TotalDays:N1}";
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.LastDays].Bounds, fontCol, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;      // Last Hours
				txt = last is null ? "" : $"{(DateTime.Now - (DateTime)last).TotalHours:N1}";
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.LastHours].Bounds, fontCol, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;     // Count
				txt = IsSend ? n.CntSend + " " : n.CntRecv + " ";
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.Count].Bounds, fontCol, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;     // Rate
				txt = $"{giftRate:P0}";
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.Rate].Bounds, fontCol, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;     // Added
				txt = Utils.GetAgeStr(n.Added);
				colAutoWidth[subitem] = Math.Max(colAutoWidth[subitem], TextRenderer.MeasureText(txt, Font).Width);// + 25;
				TextRenderer.DrawText(gfx, txt, Font,
					lvi.SubItems[(int)LvExMainColumns.Added].Bounds, fontCol, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);




				break;
			}

		}
	}



	public void AutoResizeColumns()
	{
		// Double check we havn't just cleaared the auto widths
		int sum = 0;
		for(int i = 0; i < cntColumns; i++) sum += colAutoWidth[i];

		if(sum == 0) return;

		for(int i = 0; i < cntColumns; i++) 
			if(Columns[i].Width > 0) Columns[i].Width = colAutoWidth[i];
	}


	



	// From https://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/NativeMethods.cs,e1ab28ba69954959,references
	public static IntPtr MakeLparam(int low, int high)
	{
		return (IntPtr)((high << 16) | (low & 0xffff));
	}
	public static int SignedHIWord(IntPtr n)
	{
		return SignedHIWord(unchecked((int)(long)n));
	}
	public static int SignedLOWord(IntPtr n)
	{
		return SignedLOWord(unchecked((int)(long)n));
	}
	public static int SignedHIWord(int n)
	{
		return (short)((n >> 16) & 0xffff);
	}
	public static int SignedLOWord(int n)
	{
		return (short)(n & 0xFFFF);
	}



}
