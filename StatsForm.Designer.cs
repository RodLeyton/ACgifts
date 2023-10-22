namespace ACgifts;

partial class StatsForm
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
		lvGroups = new ListView();
		lvNeighbors = new ListView();
		SuspendLayout();
		// 
		// lvGroups
		// 
		lvGroups.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		lvGroups.FullRowSelect = true;
		lvGroups.Location = new Point(12, 12);
		lvGroups.Name = "lvGroups";
		lvGroups.Size = new Size(460, 228);
		lvGroups.TabIndex = 0;
		lvGroups.UseCompatibleStateImageBehavior = false;
		lvGroups.View = View.Details;
		lvGroups.SelectedIndexChanged += LvGroup_SelectedIndexChanged;
		// 
		// lvNeighbors
		// 
		lvNeighbors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		lvNeighbors.Location = new Point(12, 258);
		lvNeighbors.Name = "lvNeighbors";
		lvNeighbors.Size = new Size(460, 191);
		lvNeighbors.TabIndex = 1;
		lvNeighbors.UseCompatibleStateImageBehavior = false;
		lvNeighbors.View = View.Details;
		// 
		// StatsForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(484, 461);
		Controls.Add(lvNeighbors);
		Controls.Add(lvGroups);
		MinimumSize = new Size(500, 500);
		Name = "StatsForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Group and Neighbor Statistics";
		ResumeLayout(false);
	}

	#endregion

	private ListView lvGroups;
	private ListView lvNeighbors;
}