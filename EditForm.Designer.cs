﻿namespace ACgifts;

partial class EditForm
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditForm));
		cbGroup = new ComboBox();
		butImport = new Button();
		dgvData = new DataGridView();
		label1 = new Label();
		butBackup = new Button();
		butRestore = new Button();
		butMoveUp = new Button();
		butMoveDown = new Button();
		butExport = new Button();
		butSave = new Button();
		((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
		SuspendLayout();
		// 
		// cbGroup
		// 
		cbGroup.FormattingEnabled = true;
		cbGroup.Location = new Point(48, 12);
		cbGroup.Name = "cbGroup";
		cbGroup.Size = new Size(149, 23);
		cbGroup.TabIndex = 1;
		cbGroup.SelectedIndexChanged += CbGroup_SelectedIndexChanged;
		// 
		// butImport
		// 
		butImport.Location = new Point(543, 12);
		butImport.Name = "butImport";
		butImport.Size = new Size(60, 23);
		butImport.TabIndex = 2;
		butImport.Text = "Import";
		butImport.UseVisualStyleBackColor = true;
		butImport.Click += ButImport_Click;
		// 
		// dgvData
		// 
		dgvData.AllowUserToResizeRows = false;
		dgvData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
		dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dgvData.Location = new Point(12, 41);
		dgvData.Name = "dgvData";
		dgvData.RowTemplate.Height = 25;
		dgvData.Size = new Size(760, 408);
		dgvData.TabIndex = 3;
		dgvData.CellParsing += DgvData_CellParsing;
		dgvData.DataError += DgvData_DataError;
		dgvData.MouseUp += DgvData_MouseUp;
		// 
		// label1
		// 
		label1.AutoSize = true;
		label1.Location = new Point(9, 15);
		label1.Name = "label1";
		label1.Size = new Size(33, 15);
		label1.TabIndex = 4;
		label1.Text = "Filter";
		// 
		// butBackup
		// 
		butBackup.Location = new Point(327, 12);
		butBackup.Name = "butBackup";
		butBackup.Size = new Size(60, 23);
		butBackup.TabIndex = 5;
		butBackup.Text = "Backup";
		butBackup.UseVisualStyleBackColor = true;
		butBackup.Click += ButBackup_Click;
		// 
		// butRestore
		// 
		butRestore.Location = new Point(393, 12);
		butRestore.Name = "butRestore";
		butRestore.Size = new Size(60, 23);
		butRestore.TabIndex = 6;
		butRestore.Text = "Restore";
		butRestore.UseVisualStyleBackColor = true;
		butRestore.Click += ButRestore_Click;
		// 
		// butMoveUp
		// 
		butMoveUp.Location = new Point(226, 12);
		butMoveUp.Name = "butMoveUp";
		butMoveUp.Size = new Size(36, 23);
		butMoveUp.TabIndex = 8;
		butMoveUp.Text = "Up";
		butMoveUp.UseVisualStyleBackColor = true;
		butMoveUp.Click += ButMoveUp_Click;
		// 
		// butMoveDown
		// 
		butMoveDown.Location = new Point(268, 12);
		butMoveDown.Name = "butMoveDown";
		butMoveDown.Size = new Size(36, 23);
		butMoveDown.TabIndex = 9;
		butMoveDown.Text = "Dn";
		butMoveDown.UseVisualStyleBackColor = true;
		butMoveDown.Click += ButMoveDown_Click;
		// 
		// butExport
		// 
		butExport.Location = new Point(609, 12);
		butExport.Name = "butExport";
		butExport.Size = new Size(60, 23);
		butExport.TabIndex = 11;
		butExport.Text = "Export";
		butExport.UseVisualStyleBackColor = true;
		butExport.Click += ButExport_Click;
		// 
		// butSave
		// 
		butSave.Location = new Point(459, 12);
		butSave.Name = "butSave";
		butSave.Size = new Size(60, 23);
		butSave.TabIndex = 12;
		butSave.Text = "Save";
		butSave.UseVisualStyleBackColor = true;
		butSave.Click += ButSave_Click;
		// 
		// EditForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(784, 461);
		Controls.Add(butSave);
		Controls.Add(butExport);
		Controls.Add(butMoveDown);
		Controls.Add(butMoveUp);
		Controls.Add(butRestore);
		Controls.Add(butBackup);
		Controls.Add(label1);
		Controls.Add(dgvData);
		Controls.Add(butImport);
		Controls.Add(cbGroup);
		Icon = (Icon)resources.GetObject("$this.Icon");
		MinimumSize = new Size(800, 500);
		Name = "EditForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "ACgifts - Editor";
		((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private ComboBox cbGroup;
	private Button butImport;
	private DataGridView dgvData;
	private Label label1;
	private Button butBackup;
	private Button butRestore;
	private Button butMoveUp;
	private Button butMoveDown;
	private Button butExport;
	private Button butSave;
}