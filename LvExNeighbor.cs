﻿using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace ACgifts;


public class LvExNeighbor:ListView
{
	private readonly Brush doneButColor = Brushes.LightGreen;
	private readonly Brush todoButColor = Brushes.NavajoWhite;
	private readonly Brush disabledButColor = Brushes.Gray;
	public const int TODAY_HOURS = 6;
	public const int BUT_DISABLE_MILLIS = 800;


	bool mb_Measured = false;
	int ms32_RowHeight = 20;

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



	#endregion


	public LvExNeighbor()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
	}


	[Category("Appearance")]
	[Description("Sets the height of the ListView rows in Details view in pixels.")]
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

	public bool IsSend = false;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams k_Params = base.CreateParams;
			k_Params.Style |= LVS_OWNERDRAWFIXED;
			return k_Params;
		}
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


		//try
		//{
		base.WndProc(ref k_Msg); // This throws a ArgOutOfRangeEx when a click is performed to the right of any subitem (empty space)
							//}
							//catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "index" && (int?)ex.ActualValue == -1 && ex.TargetSite?.DeclaringType?.Name == "ListViewSubItemCollection")
							//{
							//	Program.Log(LogLevel.Normal, "ListViewExWatch.WndProc()", "ArgumentOutOfRangeException: A click has been perfored outside of a valid subitem. This has been handled and indicates column witdth calcs were wrong.");
							//	return;
							//}

		switch(k_Msg.Msg)
		{
			case WM_SHOWWINDOW:
			{ // called when the ListView becomes visible
				View = View.Details;
				OwnerDraw = false;
				break;
			}
			case WM_REFLECT + WM_MEASUREITEM:
			{ // called once when the ListView is created, but only in Details view
				mb_Measured = true;

				// Overwrite itemHeight, which is the fifth integer in MEASUREITEMSTRUCT 
				Marshal.WriteInt32(k_Msg.LParam + 4 * sizeof(int), ms32_RowHeight);
				k_Msg.Result = (IntPtr)1;
				break;
			}
			case WM_REFLECT + WM_DRAWITEM:
			{ // called for each ListViewItem to be drawn
				object? lParam = k_Msg.GetLParam(typeof(DRAWITEMSTRUCT)) ?? throw new Exception("lParam shouldn't be null");
				DRAWITEMSTRUCT k_Draw = (DRAWITEMSTRUCT)lParam;

				using Font boldFont = new(Font, FontStyle.Bold);
				using Graphics gfx = Graphics.FromHdc(k_Draw.hDC);
				gfx.SmoothingMode = SmoothingMode.HighQuality;

				if(k_Draw.itemID >= Items.Count || k_Draw.itemID < 0)
				{
					// We shouldn't ever get here, but did once for an unknown reason.
					Debug.WriteLine("LvNeighbor.WndProc() Error:   *** Index: " + k_Draw.itemID + " out of range (0 - " + (Items.Count - 1) + ")");
					return;
				}
				ListViewItem lvi = Items[k_Draw.itemID];
				Neighbor n = (Neighbor)lvi.Tag;


				int subitem = 0; // Name Send/Recv
				TextRenderer.DrawText(gfx, IsSend ? n.NameSend : n.NameRecv, Font, lvi.SubItems[subitem].Bounds, Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;  // Send/Recv button
				Brush colBut = todoButColor;
				if(IsSend)
				{
					if(n.LastSend != null && (DateTime.Now - (DateTime)n.LastSend).TotalMilliseconds < BUT_DISABLE_MILLIS) colBut = disabledButColor;
					else if(n.SendThisSess) colBut = doneButColor;
					else if(n.LastSend != null && (DateTime.Now - (DateTime)n.LastSend).TotalHours < TODAY_HOURS) colBut = doneButColor;
				}
				else
				{
					if(n.LastRecv != null && (DateTime.Now - (DateTime)n.LastRecv).TotalMilliseconds < BUT_DISABLE_MILLIS) colBut = disabledButColor;
					else if(n.RecvThisSess) colBut = doneButColor;
					else if(n.LastRecv != null && (DateTime.Now - (DateTime)n.LastRecv).TotalHours < TODAY_HOURS) colBut = doneButColor;
				}

				Rectangle rct = new(lvi.SubItems[subitem].Bounds.X + 1, lvi.SubItems[subitem].Bounds.Y + 1, lvi.SubItems[subitem].Bounds.Width - 2, lvi.SubItems[subitem].Bounds.Height - 2);
				gfx.FillRectangle(colBut, rct);

				Pen pen = new(Color.Gray, 3) { Alignment = PenAlignment.Inset };
				gfx.DrawRectangle(pen, rct);
				TextRenderer.DrawText(gfx, lvi.SubItems[subitem].Text, Font, rct, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);


				subitem++;  // Age
				TextRenderer.DrawText(gfx, GetAgeStr(IsSend ? n.LastSend : n.LastRecv), Font, lvi.SubItems[subitem].Bounds, Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
				
				subitem++;  // Count
				TextRenderer.DrawText(gfx, IsSend ? n.CntSend+" " : n.CntRecv+" ", Font, lvi.SubItems[subitem].Bounds, Color.Black, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

				subitem++;  // Rate
				double rate = (IsSend ? n.CntSend : n.CntRecv) / Math.Max(1, (DateTime.Now - n.Added).TotalDays);
				TextRenderer.DrawText(gfx, $"{rate:P0}", Font, lvi.SubItems[subitem].Bounds, Color.Black, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
				break;
			}

		}
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


	public static string GetAgeStr(DateTime? dt)
	{
		if(dt == null) return "";
		if(dt == DateTime.MinValue) return "";

		TimeSpan ts = DateTime.Now - (DateTime)dt;
		if(ts.Duration().Days > 60) return $" {ts.Days / 30} months";
		if(ts.Duration().Days > 1) return $" {ts.Days} days";
		if(ts.Duration().Hours > 0) return $" {ts.Hours}:{ts.Minutes}";
		if(ts.Duration().Minutes > 0) return $" {ts.Minutes} mins";
		return " just now";
	}
}
