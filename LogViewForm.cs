namespace ACgifts;



public partial class LogViewForm:Form
{
	readonly HttpClient? httpClient = null;
	private readonly string? loadFile = null;
	private readonly Task<string>? stringTask = null;
	private readonly bool scrollBottom = false;


	/// <summary>
	/// Opens the file and shows the contents.
	/// </summary>
	public LogViewForm(string title, string location, bool scrollBottom = true)
	{
		InitializeComponent();
		this.loadFile = location;
		this.scrollBottom = scrollBottom;
		Text = "ACgifts - " + title;
		labLocation.Text = "location (click to copy)  " + location;
		labLocation.Tag = location;
	}



	/// <summary>
	/// Shows the string passed in a new form, without attempting to open the file.
	/// Used when a file may be opened elsewhere (e.g. the program log file).
	/// </summary>
	public LogViewForm(string title, string location, string contents, bool scrollBottom = true)
	{
		InitializeComponent();
		this.scrollBottom = scrollBottom;
		Text = "ACgifts - " + title;
		labLocation.Text = "location (click to copy)  " + location;
		labLocation.Tag = location;
		tbLog.Text = contents;

		if(scrollBottom) Utils.ScrollToBottom(tbLog);
		tbLog.Select(0, 0);
	}




	/// <summary>
	/// Retrieves the remote file and displays the contents, alternately displays the backup location on timeout or error.
	/// </summary>
	public LogViewForm(string title, Uri uri, string backupFile, bool scrollBottom = false)
	{
		InitializeComponent();
		Text = "ACgifts - " + title;
		this.loadFile = backupFile;
		this.scrollBottom = scrollBottom;
		labLocation.Text = "location (click to copy)  " + uri.ToString();
		labLocation.Tag = uri.ToString();
		tbLog.Text = "Loading...";

		HttpClient client = new();

		try
		{
			client.Timeout = new(0,0,5);
			stringTask = client.GetStringAsync(uri);
		}
		catch(Exception ex)
		{
			Program.Log("LogViewForm.Show Uri", ex);
			stringTask?.Dispose();
			stringTask = null;
			LoadFile();
		}
	}


	private void LogViewForm_Shown(object sender, EventArgs e)
	{
		// If we have a Async task display that, else if we a valid loadFile display that, else display error.
		if(stringTask != null)
		{
			RecieveContentAsync(stringTask);
			return;
		}

		if(loadFile != null) LoadFile();
	}

	private void LogViewForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		stringTask?.Dispose();
		httpClient?.Dispose();
	}



	private async void RecieveContentAsync(Task<string> task)
	{
		try
		{
			await task;
			tbLog.Text = task.Result.ReplaceLineEndings();

			if(scrollBottom) Utils.ScrollToBottom(tbLog);
			tbLog.Select(0, 0);
		}
		catch(Exception ex)
		{
			Program.Log("LogViewForm.RecieveContentAsync", ex);
			LoadFile();
		}
	}


	private void LoadFile()
	{
		if(!File.Exists(loadFile)) tbLog.Text = $"Error: '{loadFile}' does not exist!";
		else
		{
			try
			{
				tbLog.Text = File.ReadAllText(loadFile);
			}
			catch(Exception ex)
			{
				tbLog.Text = $"Error reading '{loadFile}'\r\n{ex.Message}\r\n{ex.StackTrace}\r\n";
			}
		}

		if(scrollBottom) Utils.ScrollToBottom(tbLog);
		tbLog.Select(0, 0);
	}


	private void LabLocation_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(labLocation.Tag.ToString());
		labLocation.ForeColor = Color.Blue;

		Task.Run(async delegate
		{
			await Task.Delay(500);
			if(labLocation.InvokeRequired) labLocation.Invoke(() => labLocation.ForeColor = default);
			else labLocation.ForeColor = default;
		});
	}

}
