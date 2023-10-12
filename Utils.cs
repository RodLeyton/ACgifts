using System.Reflection;
using System.Runtime.InteropServices;

namespace ACgifts;
internal class Utils
{
	public static void ShowContextOnScreen(Form frm, ContextMenuStrip ctxMenu, Point ptScrn)
	{
		Rectangle rcScrn = Screen.FromControl(frm).Bounds;

		// Multi monitor will have screens with -Y.
		if(ptScrn.X + ctxMenu.Width > rcScrn.X + rcScrn.Width + 10)
			ptScrn.X = rcScrn.X + rcScrn.Width - ctxMenu.Width - 10;
		if(ptScrn.Y + ctxMenu.Height > rcScrn.Y + rcScrn.Height + 10)
			ptScrn.Y = rcScrn.Y + rcScrn.Height - ctxMenu.Height - 10;

		if(ptScrn.X < rcScrn.X)
			ptScrn.X = rcScrn.X + 10;
		if(ptScrn.Y < rcScrn.Y)
			ptScrn.Y = rcScrn.Y + 10;

		ctxMenu.Show(ptScrn);
	}

	public static void SetDoubleBuffered(DataGridView dgv, bool setting)
	{
		try
		{
			Type dgvType = dgv.GetType();
			PropertyInfo? pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			if(pi is null)
			{
				Program.Log("Utils.SetDblBuf", "*** Failed to get PropertyInfo ***");
				return;
			}
			pi.SetValue(dgv, setting, null);
		}
		catch(Exception ex)
		{
			Program.Log("EditForm.SetDblBuf", ex);
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

	private const int WM_VSCROLL = 0x115;
	private const int SB_BOTTOM = 7;

	public static void ScrollToBottom(TextBox tb)
	{
		_ = SendMessage(tb.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
	}


	public static string EscapeCSV(object? obj)
	{
		if(obj is null) return "";
		if(obj is DateTime dt)
		{
			if(dt == DateTime.MinValue) return "";
			return dt.ToString("s");
		}
		if(obj is int i) return i.ToString();
		if(obj is long l) return l.ToString();
		if(obj is double d) return d.ToString();

		if(obj is string str)
		{
			if(str.Contains(',') ||
				str.Contains('"') ||
				str.Contains('\r') ||
				str.Contains('\n') ||
				str.StartsWith(' ') ||
				str.EndsWith(' '))
			{
				str = str.Replace("\"","\"\"");
				return $"\"{str}\"";
			}
			return str;
		}
		Program.Log("Utils.EscapeCSV()", $"Unexpected type: {obj.GetType()} is not implemented.");
		throw new NotImplementedException("Unexpected type " + obj.GetType());
	}

	public static string GetAgeStr(DateTime? dt)
	{
		if(dt == null) return "";
		if(dt == DateTime.MinValue) return "";

		TimeSpan ts = DateTime.Now - (DateTime)dt;
		if(ts.Duration().Days > 60) return $" {ts.Days / 30} months";
		if(ts.Duration().Days > 1) return $" {ts.Days} days";
		if(ts.Duration().Hours > 0) return $" {ts.Hours}:{ts.Minutes:00}";
		if(ts.Duration().Minutes > 0) return $" {ts.Minutes} mins";
		return " just now";
	}
}
