namespace ACgifts;

partial class MainForm
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
		components = new System.ComponentModel.Container();
		butSendAll = new Button();
		butEdit = new Button();
		lvRecv = new LvExNeighbor();
		lvSend = new LvExNeighbor();
		timer1 = new System.Windows.Forms.Timer(components);
		lbGroups = new ListBox();
		cbSortOrder = new ComboBox();
		labSent = new Label();
		labRecv = new Label();
		labSentToday = new Label();
		labRecvToday = new Label();
		groupBox1 = new GroupBox();
		gbGroup = new GroupBox();
		label1 = new Label();
		label5 = new Label();
		groupBox1.SuspendLayout();
		gbGroup.SuspendLayout();
		SuspendLayout();
		// 
		// butSendAll
		// 
		butSendAll.Location = new Point(336, 9);
		butSendAll.Name = "butSendAll";
		butSendAll.Size = new Size(80, 26);
		butSendAll.TabIndex = 1;
		butSendAll.Text = "Send All";
		butSendAll.UseVisualStyleBackColor = true;
		butSendAll.Click += ButSendAll_Click;
		// 
		// butEdit
		// 
		butEdit.Location = new Point(10, 9);
		butEdit.Name = "butEdit";
		butEdit.Size = new Size(97, 26);
		butEdit.TabIndex = 5;
		butEdit.Text = "Edit";
		butEdit.UseVisualStyleBackColor = true;
		butEdit.Click += ButEdit_Click;
		// 
		// lvRecv
		// 
		lvRecv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
		lvRecv.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		lvRecv.Location = new Point(129, 44);
		lvRecv.Name = "lvRecv";
		lvRecv.RowHeight = 32;
		lvRecv.Size = new Size(156, 332);
		lvRecv.TabIndex = 7;
		lvRecv.UseCompatibleStateImageBehavior = false;
		lvRecv.View = View.Details;
		lvRecv.ColumnWidthChanging += LvRecv_ColumnWidthChanging;
		lvRecv.MouseClick += LvRecv_MouseClick;
		// 
		// lvSend
		// 
		lvSend.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
		lvSend.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		lvSend.Location = new Point(336, 44);
		lvSend.Name = "lvSend";
		lvSend.RowHeight = 32;
		lvSend.Size = new Size(150, 332);
		lvSend.TabIndex = 8;
		lvSend.UseCompatibleStateImageBehavior = false;
		lvSend.View = View.Details;
		lvSend.ColumnWidthChanging += LvSend_ColumnWidthChanging;
		lvSend.MouseClick += LvSend_MouseClick;
		// 
		// timer1
		// 
		timer1.Enabled = true;
		timer1.Interval = 5000;
		timer1.Tick += Timer1_Tick;
		// 
		// lbGroups
		// 
		lbGroups.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		lbGroups.FormattingEnabled = true;
		lbGroups.ItemHeight = 20;
		lbGroups.Location = new Point(10, 44);
		lbGroups.Name = "lbGroups";
		lbGroups.Size = new Size(97, 204);
		lbGroups.TabIndex = 6;
		lbGroups.SelectedIndexChanged += LbGroups_SelectedIndexChanged;
		// 
		// cbSortOrder
		// 
		cbSortOrder.DisplayMember = "Value";
		cbSortOrder.DropDownStyle = ComboBoxStyle.DropDownList;
		cbSortOrder.FormattingEnabled = true;
		cbSortOrder.Location = new Point(129, 12);
		cbSortOrder.Name = "cbSortOrder";
		cbSortOrder.Size = new Size(127, 23);
		cbSortOrder.TabIndex = 9;
		cbSortOrder.SelectedIndexChanged += CbSortOrder_SelectedIndexChanged;
		// 
		// labSent
		// 
		labSent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labSent.AutoSize = true;
		labSent.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labSent.Location = new Point(4, 17);
		labSent.Name = "labSent";
		labSent.Size = new Size(44, 17);
		labSent.TabIndex = 10;
		labSent.Text = "Sent 0";
		labSent.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// labRecv
		// 
		labRecv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labRecv.AutoSize = true;
		labRecv.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labRecv.Location = new Point(4, 36);
		labRecv.Name = "labRecv";
		labRecv.Size = new Size(46, 17);
		labRecv.TabIndex = 11;
		labRecv.Text = "Recv 0";
		labRecv.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// labSentToday
		// 
		labSentToday.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labSentToday.AutoSize = true;
		labSentToday.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labSentToday.Location = new Point(4, 17);
		labSentToday.Name = "labSentToday";
		labSentToday.Size = new Size(44, 17);
		labSentToday.TabIndex = 12;
		labSentToday.Text = "Sent 0";
		labSentToday.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// labRecvToday
		// 
		labRecvToday.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labRecvToday.AutoSize = true;
		labRecvToday.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labRecvToday.Location = new Point(4, 35);
		labRecvToday.Name = "labRecvToday";
		labRecvToday.Size = new Size(46, 17);
		labRecvToday.TabIndex = 13;
		labRecvToday.Text = "Recv 0";
		labRecvToday.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// groupBox1
		// 
		groupBox1.Controls.Add(labRecvToday);
		groupBox1.Controls.Add(labSentToday);
		groupBox1.Location = new Point(10, 318);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new Size(97, 58);
		groupBox1.TabIndex = 14;
		groupBox1.TabStop = false;
		groupBox1.Text = "Today";
		// 
		// gbGroup
		// 
		gbGroup.Controls.Add(labSent);
		gbGroup.Controls.Add(labRecv);
		gbGroup.Controls.Add(label1);
		gbGroup.Controls.Add(label5);
		gbGroup.Location = new Point(10, 254);
		gbGroup.Name = "gbGroup";
		gbGroup.Size = new Size(97, 58);
		gbGroup.TabIndex = 15;
		gbGroup.TabStop = false;
		gbGroup.Text = "Group";
		// 
		// label1
		// 
		label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		label1.Location = new Point(6, 19);
		label1.Name = "label1";
		label1.Size = new Size(10, 15);
		label1.TabIndex = 12;
		label1.Text = "label1";
		// 
		// label5
		// 
		label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		label5.Location = new Point(6, 34);
		label5.Name = "label5";
		label5.Size = new Size(10, 15);
		label5.TabIndex = 13;
		label5.Text = "label5";
		// 
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(534, 388);
		Controls.Add(gbGroup);
		Controls.Add(groupBox1);
		Controls.Add(cbSortOrder);
		Controls.Add(lvSend);
		Controls.Add(lvRecv);
		Controls.Add(lbGroups);
		Controls.Add(butEdit);
		Controls.Add(butSendAll);
		MaximumSize = new Size(1000, 1800);
		MinimumSize = new Size(550, 427);
		Name = "MainForm";
		Text = "ACgifts";
		FormClosing += MainForm_FormClosing;
		Load += MainForm_Load;
		Resize += MainForm_Resize;
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		gbGroup.ResumeLayout(false);
		gbGroup.PerformLayout();
		ResumeLayout(false);
	}

	#endregion
	private Button butSendAll;
	private Button butEdit;
	private LvExNeighbor lvRecv;
	private LvExNeighbor lvSend;
	private System.Windows.Forms.Timer timer1;
	private ListBox lbGroups;
	private ComboBox cbSortOrder;
	private Label labSent;
	private Label labRecv;
	private Label labSentToday;
	private Label labRecvToday;
	private GroupBox groupBox1;
	private GroupBox gbGroup;
	private Label label1;
	private Label label5;
}