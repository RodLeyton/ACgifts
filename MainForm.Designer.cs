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
		butEdit.Size = new Size(80, 26);
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
		lvRecv.Size = new Size(156, 285);
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
		lvSend.Size = new Size(150, 285);
		lvSend.TabIndex = 8;
		lvSend.UseCompatibleStateImageBehavior = false;
		lvSend.View = View.Details;
		lvSend.ColumnWidthChanging += LvSend_ColumnWidthChanging;
		lvSend.MouseClick += LvSend_MouseClick;
		// 
		// timer1
		// 
		timer1.Enabled = true;
		timer1.Interval = 3000;
		timer1.Tick += Timer1_Tick;
		// 
		// lbGroups
		// 
		lbGroups.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
		lbGroups.FormattingEnabled = true;
		lbGroups.ItemHeight = 20;
		lbGroups.Location = new Point(10, 44);
		lbGroups.Name = "lbGroups";
		lbGroups.Size = new Size(80, 204);
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
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(534, 341);
		Controls.Add(cbSortOrder);
		Controls.Add(lvSend);
		Controls.Add(lvRecv);
		Controls.Add(lbGroups);
		Controls.Add(butEdit);
		Controls.Add(butSendAll);
		MaximumSize = new Size(1000, 1800);
		MinimumSize = new Size(550, 350);
		Name = "MainForm";
		Text = "MainForm";
		FormClosing += MainForm_FormClosing;
		Load += MainForm_Load;
		Resize += MainForm_Resize;
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
}