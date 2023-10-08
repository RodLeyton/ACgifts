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
		tbLog = new TextBox();
		labLocation = new Label();
		SuspendLayout();
		// 
		// tbLog
		// 
		tbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		tbLog.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
		tbLog.Location = new Point(12, 42);
		tbLog.Multiline = true;
		tbLog.Name = "tbLog";
		tbLog.ScrollBars = ScrollBars.Both;
		tbLog.Size = new Size(776, 396);
		tbLog.TabIndex = 0;
		// 
		// labLocation
		// 
		labLocation.AutoSize = true;
		labLocation.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		labLocation.Location = new Point(12, 9);
		labLocation.Name = "labLocation";
		labLocation.Size = new Size(50, 20);
		labLocation.TabIndex = 1;
		labLocation.Text = "label1";
		labLocation.Click += LabLocation_Click;
		// 
		// LogViewForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(800, 450);
		Controls.Add(labLocation);
		Controls.Add(tbLog);
		Name = "LogViewForm";
		Text = "LogViewForm";
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private TextBox tbLog;
	private Label labLocation;
}