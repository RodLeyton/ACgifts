namespace ACgifts;

partial class DetailForm
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
		tbName = new TextBox();
		groupBox1 = new GroupBox();
		dtpLastSend = new DateTimePicker();
		label4 = new Label();
		label3 = new Label();
		tbSendCode = new TextBox();
		label2 = new Label();
		tbSendName = new TextBox();
		label1 = new Label();
		groupBox2 = new GroupBox();
		dtpLastRecv = new DateTimePicker();
		label5 = new Label();
		label6 = new Label();
		tbRecvCode = new TextBox();
		label7 = new Label();
		tbRecvName = new TextBox();
		dtpAdded = new DateTimePicker();
		label8 = new Label();
		label9 = new Label();
		tbGroup = new TextBox();
		butSave = new Button();
		butClose = new Button();
		groupBox1.SuspendLayout();
		groupBox2.SuspendLayout();
		SuspendLayout();
		// 
		// tbName
		// 
		tbName.Location = new Point(95, 12);
		tbName.Name = "tbName";
		tbName.Size = new Size(146, 23);
		tbName.TabIndex = 1;
		// 
		// groupBox1
		// 
		groupBox1.Controls.Add(dtpLastSend);
		groupBox1.Controls.Add(label4);
		groupBox1.Controls.Add(label3);
		groupBox1.Controls.Add(tbSendCode);
		groupBox1.Controls.Add(label2);
		groupBox1.Controls.Add(tbSendName);
		groupBox1.Location = new Point(12, 106);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new Size(236, 106);
		groupBox1.TabIndex = 2;
		groupBox1.TabStop = false;
		groupBox1.Text = "Send to";
		// 
		// dtpLastSend
		// 
		dtpLastSend.CustomFormat = "dd MMM yyyy  HH:mmtt";
		dtpLastSend.Format = DateTimePickerFormat.Custom;
		dtpLastSend.Location = new Point(62, 74);
		dtpLastSend.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
		dtpLastSend.Name = "dtpLastSend";
		dtpLastSend.Size = new Size(167, 23);
		dtpLastSend.TabIndex = 5;
		dtpLastSend.DropDown += Dtp_DropDown;
		dtpLastSend.MouseUp += Dtp_MouseUp;
		// 
		// label4
		// 
		label4.AutoSize = true;
		label4.Location = new Point(8, 78);
		label4.Name = "label4";
		label4.Size = new Size(49, 15);
		label4.TabIndex = 9;
		label4.Text = "Last gift";
		// 
		// label3
		// 
		label3.AutoSize = true;
		label3.Location = new Point(8, 52);
		label3.Name = "label3";
		label3.Size = new Size(35, 15);
		label3.TabIndex = 8;
		label3.Text = "Code";
		// 
		// tbSendCode
		// 
		tbSendCode.Location = new Point(62, 47);
		tbSendCode.Name = "tbSendCode";
		tbSendCode.Size = new Size(167, 23);
		tbSendCode.TabIndex = 7;
		// 
		// label2
		// 
		label2.AutoSize = true;
		label2.Location = new Point(8, 24);
		label2.Name = "label2";
		label2.Size = new Size(39, 15);
		label2.TabIndex = 6;
		label2.Text = "Name";
		// 
		// tbSendName
		// 
		tbSendName.Location = new Point(62, 19);
		tbSendName.Name = "tbSendName";
		tbSendName.Size = new Size(167, 23);
		tbSendName.TabIndex = 5;
		// 
		// label1
		// 
		label1.AutoSize = true;
		label1.Location = new Point(12, 16);
		label1.Name = "label1";
		label1.Size = new Size(77, 15);
		label1.TabIndex = 4;
		label1.Text = "Forum Name";
		// 
		// groupBox2
		// 
		groupBox2.Controls.Add(dtpLastRecv);
		groupBox2.Controls.Add(label5);
		groupBox2.Controls.Add(label6);
		groupBox2.Controls.Add(tbRecvCode);
		groupBox2.Controls.Add(label7);
		groupBox2.Controls.Add(tbRecvName);
		groupBox2.Location = new Point(12, 223);
		groupBox2.Name = "groupBox2";
		groupBox2.Size = new Size(236, 106);
		groupBox2.TabIndex = 5;
		groupBox2.TabStop = false;
		groupBox2.Text = "Recv from";
		// 
		// dtpLastRecv
		// 
		dtpLastRecv.CustomFormat = "dd MMM yyyy  HH:mmtt";
		dtpLastRecv.Format = DateTimePickerFormat.Custom;
		dtpLastRecv.Location = new Point(62, 74);
		dtpLastRecv.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
		dtpLastRecv.Name = "dtpLastRecv";
		dtpLastRecv.Size = new Size(167, 23);
		dtpLastRecv.TabIndex = 5;
		dtpLastRecv.DropDown += Dtp_DropDown;
		dtpLastRecv.MouseUp += Dtp_MouseUp;
		// 
		// label5
		// 
		label5.AutoSize = true;
		label5.Location = new Point(8, 78);
		label5.Name = "label5";
		label5.Size = new Size(49, 15);
		label5.TabIndex = 9;
		label5.Text = "Last gift";
		// 
		// label6
		// 
		label6.AutoSize = true;
		label6.Location = new Point(8, 52);
		label6.Name = "label6";
		label6.Size = new Size(35, 15);
		label6.TabIndex = 8;
		label6.Text = "Code";
		// 
		// tbRecvCode
		// 
		tbRecvCode.Location = new Point(62, 47);
		tbRecvCode.Name = "tbRecvCode";
		tbRecvCode.Size = new Size(167, 23);
		tbRecvCode.TabIndex = 7;
		// 
		// label7
		// 
		label7.AutoSize = true;
		label7.Location = new Point(8, 24);
		label7.Name = "label7";
		label7.Size = new Size(39, 15);
		label7.TabIndex = 6;
		label7.Text = "Name";
		// 
		// tbRecvName
		// 
		tbRecvName.Location = new Point(62, 19);
		tbRecvName.Name = "tbRecvName";
		tbRecvName.Size = new Size(167, 23);
		tbRecvName.TabIndex = 5;
		// 
		// dtpAdded
		// 
		dtpAdded.CustomFormat = "ddd, dd MMM yyyy";
		dtpAdded.Format = DateTimePickerFormat.Custom;
		dtpAdded.Location = new Point(95, 71);
		dtpAdded.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
		dtpAdded.Name = "dtpAdded";
		dtpAdded.Size = new Size(146, 23);
		dtpAdded.TabIndex = 10;
		// 
		// label8
		// 
		label8.AutoSize = true;
		label8.Location = new Point(12, 75);
		label8.Name = "label8";
		label8.Size = new Size(69, 15);
		label8.TabIndex = 11;
		label8.Text = "Date Added";
		// 
		// label9
		// 
		label9.AutoSize = true;
		label9.Location = new Point(12, 45);
		label9.Name = "label9";
		label9.Size = new Size(40, 15);
		label9.TabIndex = 13;
		label9.Text = "Group";
		// 
		// tbGroup
		// 
		tbGroup.Location = new Point(95, 41);
		tbGroup.Name = "tbGroup";
		tbGroup.Size = new Size(146, 23);
		tbGroup.TabIndex = 12;
		// 
		// butSave
		// 
		butSave.Location = new Point(96, 343);
		butSave.Name = "butSave";
		butSave.Size = new Size(73, 25);
		butSave.TabIndex = 14;
		butSave.Text = "Save";
		butSave.UseVisualStyleBackColor = true;
		butSave.Click += ButSave_Click;
		// 
		// butClose
		// 
		butClose.Location = new Point(175, 343);
		butClose.Name = "butClose";
		butClose.Size = new Size(73, 25);
		butClose.TabIndex = 15;
		butClose.Text = "Close";
		butClose.UseVisualStyleBackColor = true;
		butClose.Click += ButClose_Click;
		// 
		// DetailForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(260, 380);
		Controls.Add(butClose);
		Controls.Add(butSave);
		Controls.Add(label9);
		Controls.Add(tbGroup);
		Controls.Add(dtpAdded);
		Controls.Add(label8);
		Controls.Add(groupBox2);
		Controls.Add(label1);
		Controls.Add(groupBox1);
		Controls.Add(tbName);
		DoubleBuffered = true;
		FormBorderStyle = FormBorderStyle.FixedDialog;
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "DetailForm";
		ShowIcon = false;
		ShowInTaskbar = false;
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.Manual;
		Text = "DetailForm";
		FormClosing += DetailForm_FormClosing;
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		groupBox2.ResumeLayout(false);
		groupBox2.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion
	private TextBox tbName;
	private GroupBox groupBox1;
	private Label label4;
	private Label label3;
	private TextBox tbSendCode;
	private Label label2;
	private TextBox tbSendName;
	private Label label1;
	private DateTimePicker dtpLastSend;
	private GroupBox groupBox2;
	private DateTimePicker dtpLastRecv;
	private Label label5;
	private Label label6;
	private TextBox tbRecvCode;
	private Label label7;
	private TextBox tbRecvName;
	private DateTimePicker dtpAdded;
	private Label label8;
	private Label label9;
	private TextBox tbGroup;
	private Button butSave;
	private Button butClose;
}