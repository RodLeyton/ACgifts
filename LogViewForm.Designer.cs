namespace ACgifts;

partial class LogViewForm
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if(disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewForm));
		tbLog = new TextBox();
		labLocation = new Label();
		SuspendLayout();
		// 
		// tbLog
		// 
		tbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		tbLog.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
		tbLog.Location = new Point(12, 12);
		tbLog.Multiline = true;
		tbLog.Name = "tbLog";
		tbLog.ScrollBars = ScrollBars.Both;
		tbLog.Size = new Size(760, 447);
		tbLog.TabIndex = 0;
		// 
		// labLocation
		// 
		labLocation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		labLocation.AutoSize = true;
		labLocation.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labLocation.Location = new Point(12, 462);
		labLocation.Name = "labLocation";
		labLocation.Size = new Size(138, 17);
		labLocation.TabIndex = 2;
		labLocation.Text = "location (click to copy)";
		labLocation.Click += LabLocation_Click;
		// 
		// LogViewForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(784, 488);
		Controls.Add(labLocation);
		Controls.Add(tbLog);
		Icon = (Icon)resources.GetObject("$this.Icon");
		MinimumSize = new Size(600, 300);
		Name = "LogViewForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "ACgifts - Logs";
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private TextBox tbLog;
	private Label labLocation;
}