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
		ToolStripSeparator toolStripSeparator1;
		ToolStripSeparator toolStripSeparator2;
		ToolStripSeparator toolStripSeparator3;
		ToolStripSeparator toolStripSeparator4;
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
		menuFile = new ToolStripMenuItem();
		menuBackup = new ToolStripMenuItem();
		menuRestore = new ToolStripMenuItem();
		menuImport = new ToolStripMenuItem();
		menuExport = new ToolStripMenuItem();
		menuSave = new ToolStripMenuItem();
		menuExit = new ToolStripMenuItem();
		menuEdit = new ToolStripMenuItem();
		menuDisplay = new ToolStripMenuItem();
		menuSaveLayout = new ToolStripMenuItem();
		menuResetLayout = new ToolStripMenuItem();
		menuAutoWidthColumns = new ToolStripMenuItem();
		menuAutoWidthForm = new ToolStripMenuItem();
		menuAutoWidthAll = new ToolStripMenuItem();
		menuStats = new ToolStripMenuItem();
		menuHelp = new ToolStripMenuItem();
		menuHelpHeader = new ToolStripMenuItem();
		menuWebsite = new ToolStripMenuItem();
		menuFAQ = new ToolStripMenuItem();
		menuGitHub = new ToolStripMenuItem();
		menuForum = new ToolStripMenuItem();
		menuViewLog = new ToolStripMenuItem();
		menuDataDir = new ToolStripMenuItem();
		menuAppDir = new ToolStripMenuItem();
		butSendAll = new Button();
		lvRecv = new LvExMain();
		lvSend = new LvExMain();
		timer1 = new System.Windows.Forms.Timer(components);
		lbGroups = new ListBox();
		cbSortOrder = new ComboBox();
		labSent = new Label();
		labRecv = new Label();
		labSentToday = new Label();
		labRecvToday = new Label();
		groupBox1 = new GroupBox();
		label3 = new Label();
		label2 = new Label();
		gbGroup = new GroupBox();
		label4 = new Label();
		label6 = new Label();
		label1 = new Label();
		label5 = new Label();
		spliter = new SplitContainer();
		menuStrip = new MenuStrip();
		toolStripSeparator1 = new ToolStripSeparator();
		toolStripSeparator2 = new ToolStripSeparator();
		toolStripSeparator3 = new ToolStripSeparator();
		toolStripSeparator4 = new ToolStripSeparator();
		groupBox1.SuspendLayout();
		gbGroup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)spliter).BeginInit();
		spliter.Panel1.SuspendLayout();
		spliter.Panel2.SuspendLayout();
		spliter.SuspendLayout();
		menuStrip.SuspendLayout();
		SuspendLayout();
		// 
		// toolStripSeparator1
		// 
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(183, 6);
		// 
		// toolStripSeparator2
		// 
		toolStripSeparator2.Name = "toolStripSeparator2";
		toolStripSeparator2.Size = new Size(152, 6);
		// 
		// toolStripSeparator3
		// 
		toolStripSeparator3.Name = "toolStripSeparator3";
		toolStripSeparator3.Size = new Size(152, 6);
		// 
		// toolStripSeparator4
		// 
		toolStripSeparator4.Name = "toolStripSeparator4";
		toolStripSeparator4.Size = new Size(163, 6);
		// 
		// menuFile
		// 
		menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuBackup, menuRestore, toolStripSeparator2, menuImport, menuExport, toolStripSeparator3, menuSave, menuExit });
		menuFile.Name = "menuFile";
		menuFile.Size = new Size(37, 20);
		menuFile.Text = "File";
		// 
		// menuBackup
		// 
		menuBackup.Name = "menuBackup";
		menuBackup.Size = new Size(155, 22);
		menuBackup.Text = "Backup data";
		menuBackup.Click += MenuBackup_Click;
		// 
		// menuRestore
		// 
		menuRestore.Name = "menuRestore";
		menuRestore.Size = new Size(155, 22);
		menuRestore.Text = "Restore backup";
		menuRestore.Click += MenuRestore_Click;
		// 
		// menuImport
		// 
		menuImport.Name = "menuImport";
		menuImport.Size = new Size(155, 22);
		menuImport.Text = "Import CSV";
		menuImport.Click += MenuImport_Click;
		// 
		// menuExport
		// 
		menuExport.Name = "menuExport";
		menuExport.Size = new Size(155, 22);
		menuExport.Text = "Export CSV";
		menuExport.Click += MenuExport_Click;
		// 
		// menuSave
		// 
		menuSave.Name = "menuSave";
		menuSave.Size = new Size(155, 22);
		menuSave.Text = "Save";
		menuSave.Click += MenuSave_Click;
		// 
		// menuExit
		// 
		menuExit.Name = "menuExit";
		menuExit.Size = new Size(155, 22);
		menuExit.Text = "Exit";
		menuExit.Click += MenuExit_Click;
		// 
		// menuEdit
		// 
		menuEdit.Name = "menuEdit";
		menuEdit.Size = new Size(39, 20);
		menuEdit.Text = "Edit";
		menuEdit.Click += Menu_Edit_Click;
		// 
		// menuDisplay
		// 
		menuDisplay.DropDownItems.AddRange(new ToolStripItem[] { menuSaveLayout, menuResetLayout, toolStripSeparator1, menuAutoWidthColumns, menuAutoWidthForm, menuAutoWidthAll });
		menuDisplay.Name = "menuDisplay";
		menuDisplay.Size = new Size(57, 20);
		menuDisplay.Text = "Display";
		// 
		// menuSaveLayout
		// 
		menuSaveLayout.Name = "menuSaveLayout";
		menuSaveLayout.Size = new Size(186, 22);
		menuSaveLayout.Text = "Save layout";
		menuSaveLayout.Click += MenuSaveLayout_Click;
		// 
		// menuResetLayout
		// 
		menuResetLayout.Name = "menuResetLayout";
		menuResetLayout.Size = new Size(186, 22);
		menuResetLayout.Text = "Reset layout";
		menuResetLayout.Click += MenuResetLayout_Click;
		// 
		// menuAutoWidthColumns
		// 
		menuAutoWidthColumns.Name = "menuAutoWidthColumns";
		menuAutoWidthColumns.Size = new Size(186, 22);
		menuAutoWidthColumns.Text = "Auto Width Columns";
		menuAutoWidthColumns.Click += MenuAutoWidthColumns_Click;
		// 
		// menuAutoWidthForm
		// 
		menuAutoWidthForm.Name = "menuAutoWidthForm";
		menuAutoWidthForm.Size = new Size(186, 22);
		menuAutoWidthForm.Text = "Auto Width Form";
		menuAutoWidthForm.Click += MenuAutoWidthForm_Click;
		// 
		// menuAutoWidthAll
		// 
		menuAutoWidthAll.Name = "menuAutoWidthAll";
		menuAutoWidthAll.Size = new Size(186, 22);
		menuAutoWidthAll.Text = "Auto Width All";
		menuAutoWidthAll.Click += MenuAutoWidthAll_Click;
		// 
		// menuStats
		// 
		menuStats.Name = "menuStats";
		menuStats.Size = new Size(65, 20);
		menuStats.Text = "Statistics";
		menuStats.Click += Menu_Stats_Click;
		// 
		// menuHelp
		// 
		menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuHelpHeader, menuWebsite, menuFAQ, menuGitHub, menuForum, toolStripSeparator4, menuViewLog, menuDataDir, menuAppDir });
		menuHelp.Name = "menuHelp";
		menuHelp.Size = new Size(44, 20);
		menuHelp.Text = "Help";
		// 
		// menuHelpHeader
		// 
		menuHelpHeader.Enabled = false;
		menuHelpHeader.Name = "menuHelpHeader";
		menuHelpHeader.Size = new Size(166, 22);
		menuHelpHeader.Text = "Opens in browser";
		// 
		// menuWebsite
		// 
		menuWebsite.Name = "menuWebsite";
		menuWebsite.Size = new Size(166, 22);
		menuWebsite.Text = "Website";
		menuWebsite.Click += Menu_Website_Click;
		// 
		// menuFAQ
		// 
		menuFAQ.Name = "menuFAQ";
		menuFAQ.Size = new Size(166, 22);
		menuFAQ.Text = "FAQ";
		menuFAQ.Click += Menu_FAQ_Click;
		// 
		// menuGitHub
		// 
		menuGitHub.Name = "menuGitHub";
		menuGitHub.Size = new Size(166, 22);
		menuGitHub.Text = "Github";
		menuGitHub.Click += Menu_GitHub_Click;
		// 
		// menuForum
		// 
		menuForum.Name = "menuForum";
		menuForum.Size = new Size(166, 22);
		menuForum.Text = "Forum";
		menuForum.Click += Menu_Forum_Click;
		// 
		// menuViewLog
		// 
		menuViewLog.Name = "menuViewLog";
		menuViewLog.Size = new Size(166, 22);
		menuViewLog.Text = "View logfile";
		menuViewLog.Click += MenuViewLog_Click;
		// 
		// menuDataDir
		// 
		menuDataDir.Name = "menuDataDir";
		menuDataDir.Size = new Size(166, 22);
		menuDataDir.Text = "Open Data Dir";
		menuDataDir.Click += MenuDataDir_Click;
		// 
		// menuAppDir
		// 
		menuAppDir.Name = "menuAppDir";
		menuAppDir.Size = new Size(166, 22);
		menuAppDir.Text = "Open App Dir";
		menuAppDir.Click += MenuAppDir_Click;
		// 
		// butSendAll
		// 
		butSendAll.Location = new Point(324, 30);
		butSendAll.Name = "butSendAll";
		butSendAll.Size = new Size(80, 26);
		butSendAll.TabIndex = 1;
		butSendAll.Text = "Send All";
		butSendAll.UseVisualStyleBackColor = true;
		butSendAll.Click += ButSendAll_Click;
		// 
		// lvRecv
		// 
		lvRecv.AllowColumnReorder = true;
		lvRecv.Dock = DockStyle.Fill;
		lvRecv.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		lvRecv.FullRowSelect = true;
		lvRecv.HeaderContextMenu = null;
		lvRecv.Location = new Point(0, 0);
		lvRecv.MultiSelect = false;
		lvRecv.Name = "lvRecv";
		lvRecv.OwnerDraw = true;
		lvRecv.RowHeight = 32;
		lvRecv.Size = new Size(106, 312);
		lvRecv.TabIndex = 7;
		lvRecv.UseCompatibleStateImageBehavior = false;
		lvRecv.View = View.Details;
		lvRecv.ColumnReordered += LvEx_ColumnReordered;
		lvRecv.ColumnWidthChanged += LvEx_ColumnWidthChanged;
		lvRecv.ColumnWidthChanging += LvEx_ColumnWidthChanging;
		lvRecv.MouseClick += LvRecv_MouseClick;
		// 
		// lvSend
		// 
		lvSend.AllowColumnReorder = true;
		lvSend.Dock = DockStyle.Fill;
		lvSend.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		lvSend.HeaderContextMenu = null;
		lvSend.Location = new Point(0, 0);
		lvSend.Name = "lvSend";
		lvSend.RowHeight = 32;
		lvSend.Size = new Size(283, 312);
		lvSend.TabIndex = 8;
		lvSend.UseCompatibleStateImageBehavior = false;
		lvSend.View = View.Details;
		lvSend.ColumnReordered += LvEx_ColumnReordered;
		lvSend.ColumnWidthChanged += LvEx_ColumnWidthChanged;
		lvSend.ColumnWidthChanging += LvEx_ColumnWidthChanging;
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
		lbGroups.Location = new Point(12, 33);
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
		cbSortOrder.Location = new Point(121, 33);
		cbSortOrder.Name = "cbSortOrder";
		cbSortOrder.Size = new Size(127, 23);
		cbSortOrder.TabIndex = 9;
		cbSortOrder.SelectedIndexChanged += CbSortOrder_SelectedIndexChanged;
		// 
		// labSent
		// 
		labSent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labSent.BackColor = Color.Transparent;
		labSent.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labSent.Location = new Point(1, 17);
		labSent.Name = "labSent";
		labSent.Size = new Size(95, 17);
		labSent.TabIndex = 10;
		labSent.Text = "0/0";
		labSent.TextAlign = ContentAlignment.MiddleRight;
		// 
		// labRecv
		// 
		labRecv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labRecv.BackColor = Color.Transparent;
		labRecv.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labRecv.Location = new Point(1, 36);
		labRecv.Name = "labRecv";
		labRecv.Size = new Size(95, 17);
		labRecv.TabIndex = 11;
		labRecv.Text = "0/0";
		labRecv.TextAlign = ContentAlignment.MiddleRight;
		// 
		// labSentToday
		// 
		labSentToday.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labSentToday.BackColor = Color.Transparent;
		labSentToday.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labSentToday.Location = new Point(1, 17);
		labSentToday.Name = "labSentToday";
		labSentToday.Size = new Size(94, 17);
		labSentToday.TabIndex = 12;
		labSentToday.Text = "0/0";
		labSentToday.TextAlign = ContentAlignment.MiddleRight;
		// 
		// labRecvToday
		// 
		labRecvToday.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labRecvToday.BackColor = Color.Transparent;
		labRecvToday.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		labRecvToday.Location = new Point(1, 36);
		labRecvToday.Name = "labRecvToday";
		labRecvToday.Size = new Size(94, 17);
		labRecvToday.TabIndex = 13;
		labRecvToday.Text = "0/0";
		labRecvToday.TextAlign = ContentAlignment.MiddleRight;
		// 
		// groupBox1
		// 
		groupBox1.Controls.Add(label3);
		groupBox1.Controls.Add(label2);
		groupBox1.Controls.Add(labRecvToday);
		groupBox1.Controls.Add(labSentToday);
		groupBox1.Location = new Point(12, 315);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new Size(96, 58);
		groupBox1.TabIndex = 14;
		groupBox1.TabStop = false;
		groupBox1.Text = "Today";
		// 
		// label3
		// 
		label3.AutoSize = true;
		label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		label3.Location = new Point(1, 35);
		label3.Name = "label3";
		label3.Size = new Size(35, 17);
		label3.TabIndex = 17;
		label3.Text = "Recv";
		label3.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// label2
		// 
		label2.AutoSize = true;
		label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		label2.Location = new Point(1, 17);
		label2.Name = "label2";
		label2.Size = new Size(33, 17);
		label2.TabIndex = 16;
		label2.Text = "Sent";
		label2.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// gbGroup
		// 
		gbGroup.Controls.Add(label4);
		gbGroup.Controls.Add(label6);
		gbGroup.Controls.Add(label1);
		gbGroup.Controls.Add(label5);
		gbGroup.Controls.Add(labSent);
		gbGroup.Controls.Add(labRecv);
		gbGroup.Location = new Point(12, 248);
		gbGroup.Name = "gbGroup";
		gbGroup.Size = new Size(97, 58);
		gbGroup.TabIndex = 15;
		gbGroup.TabStop = false;
		gbGroup.Text = "Group";
		// 
		// label4
		// 
		label4.AutoSize = true;
		label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		label4.Location = new Point(1, 35);
		label4.Name = "label4";
		label4.Size = new Size(35, 17);
		label4.TabIndex = 19;
		label4.Text = "Recv";
		label4.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// label6
		// 
		label6.AutoSize = true;
		label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
		label6.Location = new Point(1, 17);
		label6.Name = "label6";
		label6.Size = new Size(33, 17);
		label6.TabIndex = 18;
		label6.Text = "Sent";
		label6.TextAlign = ContentAlignment.MiddleLeft;
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
		// spliter
		// 
		spliter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		spliter.Location = new Point(121, 64);
		spliter.Name = "spliter";
		// 
		// spliter.Panel1
		// 
		spliter.Panel1.Controls.Add(lvRecv);
		spliter.Panel1MinSize = 100;
		// 
		// spliter.Panel2
		// 
		spliter.Panel2.Controls.Add(lvSend);
		spliter.Panel2MinSize = 100;
		spliter.Size = new Size(401, 312);
		spliter.SplitterDistance = 106;
		spliter.SplitterWidth = 12;
		spliter.TabIndex = 17;
		spliter.SplitterMoved += Spliter_SplitterMoved;
		spliter.MouseUp += Spliter_MouseUp;
		// 
		// menuStrip
		// 
		menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuEdit, menuDisplay, menuStats, menuHelp });
		menuStrip.Location = new Point(0, 0);
		menuStrip.Name = "menuStrip";
		menuStrip.Size = new Size(534, 24);
		menuStrip.TabIndex = 18;
		menuStrip.Text = "Main Menu";
		// 
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(534, 388);
		Controls.Add(spliter);
		Controls.Add(gbGroup);
		Controls.Add(groupBox1);
		Controls.Add(cbSortOrder);
		Controls.Add(lbGroups);
		Controls.Add(butSendAll);
		Controls.Add(menuStrip);
		DoubleBuffered = true;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MainMenuStrip = menuStrip;
		MaximumSize = new Size(1600, 1800);
		MinimumSize = new Size(550, 427);
		Name = "MainForm";
		Text = "ACgifts";
		FormClosing += MainForm_FormClosing;
		Load += MainForm_Load;
		Shown += MainForm_Shown;
		ResizeEnd += MainForm_ResizeEnd;
		Resize += MainForm_Resize;
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		gbGroup.ResumeLayout(false);
		gbGroup.PerformLayout();
		spliter.Panel1.ResumeLayout(false);
		spliter.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)spliter).EndInit();
		spliter.ResumeLayout(false);
		menuStrip.ResumeLayout(false);
		menuStrip.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion
	private Button butSendAll;
	private LvExMain lvRecv;
	private LvExMain lvSend;
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
	private Label label2;
	private Label label3;
	private Label label4;
	private Label label6;
	private SplitContainer spliter;
	private MenuStrip menuStrip;
	private ToolStripMenuItem menuFile;
	private ToolStripMenuItem menuEdit;
	private ToolStripMenuItem menuDisplay;
	private ToolStripMenuItem menuStats;
	private ToolStripMenuItem menuHelp;
	private ToolStripMenuItem menuHelpHeader;
	private ToolStripMenuItem menuFAQ;
	private ToolStripMenuItem menuGitHub;
	private ToolStripMenuItem menuForum;
	private ToolStripMenuItem menuWebsite;
	private ToolStripMenuItem menuBackup;
	private ToolStripMenuItem menuRestore;
	private ToolStripMenuItem menuImport;
	private ToolStripMenuItem menuExport;
	private ToolStripMenuItem menuSave;
	private ToolStripMenuItem menuExit;
	private ToolStripMenuItem menuAutoWidthForm;
	private ToolStripMenuItem menuSaveLayout;
	private ToolStripMenuItem menuAutoWidthColumns;
	private ToolStripMenuItem menuAutoWidthAll;
	private ToolStripMenuItem menuViewLog;
	private ToolStripMenuItem menuDataDir;
	private ToolStripMenuItem menuAppDir;
	private ToolStripMenuItem menuResetLayout;
}