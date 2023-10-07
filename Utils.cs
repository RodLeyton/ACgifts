using System.Reflection;

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
				Program.Log("*** EditForm.SetDoubleBuffered() failed to get property ***");
				return;
			}
			pi.SetValue(dgv, setting, null);
		}
		catch(Exception ex)
		{
			Program.Log("*** EditForm.SetDoubleBuffered() Exception ***");
			Program.Log(ex);
		}
	}
}
