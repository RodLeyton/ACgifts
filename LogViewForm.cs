using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace ACgifts;



public partial class LogViewForm:Form
{
	public string fileName;


	/// <summary>
	/// Reads and shows the file contents in a new form.
	/// </summary>
	public static void ShowFile(string title, string fileName, IWin32Window? owner = null, bool scrollBottom = false)
	{
		ShowFile(title, fileName, null, owner, scrollBottom);
	}
	/// <summary>
	/// Shows the file contents in a new form, without attempting to open the file.
	/// Used when a file may be opened elsewhere (e.g. the program log file).
	/// </summary>
	public static void ShowFile(string title, string fileName, string? contents, IWin32Window? owner = null, bool scrollBottom = false)
	{
		LogViewForm form = new(title, fileName, contents, scrollBottom);
		if(owner is null) form.Show(); else form.ShowDialog(owner);
		form.Dispose();
	}




	public LogViewForm(string title, string fileName, string? contents = null, bool scrollBottom = false)
	{
		InitializeComponent();
		this.fileName = fileName;
		Text = "ACgifts - " + title;
		labLocation.Text = "location (click to copy)  " + fileName;

		if(contents != null) tbLog.Text = contents;
		else
		{
			if(!File.Exists(fileName)) tbLog.Text = $"Error: '{fileName}' does not exist!";
			else
			{
				try
				{
					tbLog.Text = File.ReadAllText(fileName);
				}
				catch(Exception ex)
				{
					tbLog.Text = $"Error reading '{fileName}'\r\n{ex.Message}\r\n{ex.StackTrace}\r\n";
				}
			}
		}
		if(scrollBottom) Utils.ScrollToBottom(tbLog);
		tbLog.Select(0, 0);
	}

	private void LabLocation_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(fileName);
		labLocation.ForeColor = Color.Blue;

		Task.Run(async delegate
		{
			await Task.Delay(500);
			if(labLocation.InvokeRequired) labLocation.Invoke(() => labLocation.ForeColor = default);
			else labLocation.ForeColor = default;
		});
	}
}
