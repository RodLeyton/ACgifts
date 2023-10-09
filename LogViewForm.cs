namespace ACgifts;



public partial class LogViewForm:Form
{
	public LogViewForm()
	{
		InitializeComponent();
		labLocation.Text = Program.GetLogFile();

		tbLog.Text = Program.GetLogs();
		Utils.ScrollToBottom(tbLog);
		tbLog.Select(0, 0);
	}

	private void LabLocation_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(Program.GetLogFile());
		labLocation.ForeColor = Color.Blue;

		Task.Run(async delegate
		{
			await Task.Delay(500);
			if(labLocation.InvokeRequired) labLocation.Invoke(() => labLocation.ForeColor = default);
			else labLocation.ForeColor = default;
		});
	}
}
