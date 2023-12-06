namespace ACgifts;

partial class SearchForm
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
		label9 = new Label();
		tbGroup = new TextBox();
		label1 = new Label();
		label3 = new Label();
		tbCode = new TextBox();
		label2 = new Label();
		tbGameName = new TextBox();
		tbForumName = new TextBox();
		butCancel = new Button();
		butOK = new Button();
		labRes = new Label();
		SuspendLayout();
		// 
		// label9
		// 
		label9.AutoSize = true;
		label9.Location = new Point(14, 73);
		label9.Name = "label9";
		label9.Size = new Size(40, 15);
		label9.TabIndex = 20;
		label9.Text = "Group";
		// 
		// tbGroup
		// 
		tbGroup.Location = new Point(97, 70);
		tbGroup.Name = "tbGroup";
		tbGroup.Size = new Size(146, 23);
		tbGroup.TabIndex = 19;
		tbGroup.TextChanged += Search;
		// 
		// label1
		// 
		label1.AutoSize = true;
		label1.Location = new Point(14, 16);
		label1.Name = "label1";
		label1.Size = new Size(77, 15);
		label1.TabIndex = 16;
		label1.Text = "Forum Name";
		// 
		// label3
		// 
		label3.AutoSize = true;
		label3.Location = new Point(14, 102);
		label3.Name = "label3";
		label3.Size = new Size(35, 15);
		label3.TabIndex = 8;
		label3.Text = "Code";
		// 
		// tbCode
		// 
		tbCode.Location = new Point(97, 99);
		tbCode.Name = "tbCode";
		tbCode.Size = new Size(146, 23);
		tbCode.TabIndex = 7;
		tbCode.TextChanged += Search;
		// 
		// label2
		// 
		label2.AutoSize = true;
		label2.Location = new Point(14, 44);
		label2.Name = "label2";
		label2.Size = new Size(73, 15);
		label2.TabIndex = 6;
		label2.Text = "Game Name";
		// 
		// tbGameName
		// 
		tbGameName.Location = new Point(97, 41);
		tbGameName.Name = "tbGameName";
		tbGameName.Size = new Size(146, 23);
		tbGameName.TabIndex = 5;
		tbGameName.TextChanged += Search;
		// 
		// tbForumName
		// 
		tbForumName.Location = new Point(97, 12);
		tbForumName.Name = "tbForumName";
		tbForumName.Size = new Size(146, 23);
		tbForumName.TabIndex = 14;
		tbForumName.TextChanged += Search;
		// 
		// butCancel
		// 
		butCancel.Location = new Point(14, 169);
		butCancel.Name = "butCancel";
		butCancel.Size = new Size(98, 29);
		butCancel.TabIndex = 21;
		butCancel.Text = "Cancel";
		butCancel.UseVisualStyleBackColor = true;
		butCancel.Click += ButCancel_Click;
		// 
		// butOK
		// 
		butOK.Location = new Point(145, 169);
		butOK.Name = "butOK";
		butOK.Size = new Size(98, 29);
		butOK.TabIndex = 22;
		butOK.Text = "OK";
		butOK.UseVisualStyleBackColor = true;
		butOK.Click += ButtonOK_Click;
		// 
		// labRes
		// 
		labRes.AutoSize = true;
		labRes.Location = new Point(14, 142);
		labRes.Name = "labRes";
		labRes.Size = new Size(104, 15);
		labRes.TabIndex = 23;
		labRes.Text = "0 neighbors found";
		// 
		// SearchForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(260, 210);
		Controls.Add(labRes);
		Controls.Add(butOK);
		Controls.Add(butCancel);
		Controls.Add(label9);
		Controls.Add(tbGroup);
		Controls.Add(label3);
		Controls.Add(tbCode);
		Controls.Add(label2);
		Controls.Add(label1);
		Controls.Add(tbGameName);
		Controls.Add(tbForumName);
		Name = "SearchForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "SearchForm";
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Label label9;
	private TextBox tbGroup;
	private Label label1;
	private Label label3;
	private TextBox tbCode;
	private Label label2;
	private TextBox tbGameName;
	private TextBox tbForumName;
	private Button butCancel;
	private Button butOK;
	private Label labRes;
}