﻿using System.ComponentModel;
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

	public static void SetDoubleBuffered(Control dgv, bool setting)
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
		if(ts.Duration().TotalDays >= 365) return $" {ts.TotalDays / 365:N1} yrs";
		if(ts.Duration().TotalDays >= 120) return $" {ts.TotalDays / 30} mths";
		if(ts.Duration().TotalDays > 60) return $" {ts.TotalDays / 30:N1} mths";
		if(ts.Duration().TotalDays > 2) return $" {ts.TotalDays:N0} days";
		if(ts.Duration().TotalHours > 0) return $" {ts.TotalHours:N1} hrs";
		if(ts.Duration().TotalMinutes > 0) return $" {ts.TotalMinutes} mins";
		return " just now";
	}


	public static string GetEnumDescription(Enum value)
	{
		FieldInfo? fi = value.GetType().GetField(value.ToString());

		if(fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
			return attributes.First().Description;

		return value.ToString();
	}


}
