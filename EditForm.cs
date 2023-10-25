using System.Data;
using System.Diagnostics;

namespace ACgifts;

internal partial class EditForm:Form
{
	private readonly Data data;
	private readonly BindingSource bindSource;
	private class ContextEventArgs
	{
		public Neighbor Neighbor { get; set; }
		public int RowIndex = -1;

		public ContextEventArgs(Neighbor neighbor, int rowIndex)
		{
			Neighbor = neighbor;
			RowIndex = rowIndex;
		}
	}


	public EditForm(Data data)
	{
		InitializeComponent();
		this.data = data;
		Utils.SetDoubleBuffered(dgvData, true);

		bindSource = new BindingSource(data.neighbors, null);
		dgvData.DataSource = bindSource;

		cbGroup.Items.Add("All");
		cbGroup.Items.Add("Unassigned");
		foreach(Neighbor n in data.neighbors)
			if(n.Group != null && n.Group?.Trim() != "" && !cbGroup.Items.Contains(n.Group)) cbGroup.Items.Add(n.Group);

		cbGroup.SelectedIndex = 0;
	}
	private void CbGroup_SelectedIndexChanged(object sender, EventArgs e)
	{
		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
		bindSource.SuspendBinding();

		foreach(DataGridViewRow row in dgvData.Rows)
		{
			if(row.IsNewRow) continue;

			if(cbGroup.SelectedIndex < 1)
			{
				row.Visible = true;
				continue;
			}

			if(row.Cells[1].Value is not string group) group = "";

			if(cbGroup.SelectedIndex == 1)
			{
				if(group.Trim() == "") row.Visible = true;
				else row.Visible = false;
				continue;
			}
			row.Visible = (string)row.Cells[1].Value == cbGroup.Text;
		}

		bindSource.ResumeBinding();
		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
	}

	private void ButImport_Click(object sender, EventArgs e)
	{
		OpenFileDialog ofd = new() { Filter = "csv|*.csv" };
		if(ofd.ShowDialog() == DialogResult.OK)
		{
			bindSource.SuspendBinding();
			data.Import(ofd.FileName);
			bindSource.ResumeBinding();
			bindSource.ResetBindings(true);
		}
	}
	private void ButExport_Click(object sender, EventArgs e)
	{
		data.Export();
	}
	private void ButBackup_Click(object sender, EventArgs e)
	{
		data.Backup();
	}
	private void ButRestore_Click(object sender, EventArgs e)
	{
		this.Enabled = false;
		data.Restore();
		this.Enabled = true;
	}
	private void ButMoveUp_Click(object sender, EventArgs e)
	{
		if(dgvData.SelectedRows.Count == 0) return;

		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
		List<int> indexs = new();

		foreach(DataGridViewRow row in dgvData.SelectedRows)
			indexs.Add(row.Index);

		foreach(int inx in indexs.OrderBy(x => x))
		{
			int rowToMove = inx - 1;
			while(rowToMove >= 0)
			{
				if(dgvData.Rows[rowToMove].Visible)
				{
					data.neighbors.Insert(inx + 1, data.neighbors[rowToMove]);
					data.neighbors.RemoveAt(rowToMove);
					break;
				}
				rowToMove--;
			}
		}
		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		data.ReOrder();
	}
	private void ButMoveDown_Click(object sender, EventArgs e)
	{
		if(dgvData.SelectedRows.Count == 0) return;

		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
		List<int> indexs = new();

		foreach(DataGridViewRow row in dgvData.SelectedRows)
			indexs.Add(row.Index);

		foreach(int inx in indexs.OrderBy(x => x).Reverse())
		{
			int rowToMove = inx + 1;
			while(rowToMove < dgvData.Rows.Count - 1)
			{
				if(dgvData.Rows[rowToMove].Visible)
				{
					data.neighbors.Insert(inx, data.neighbors[rowToMove]);
					data.neighbors.RemoveAt(rowToMove + 1);
					break;
				}
				rowToMove++;
			}
		}
		dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		data.ReOrder();
	}
	private void ButSave_Click(object sender, EventArgs e)
	{
		data.Save();
		MessageBox.Show("Datafile saved.\r\n\r\nThis is automatically done on program exit,\r\nbut sometimes it is nice to do it after major changes.", "ACgifts Save completed");
	}


	private void DgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
	{
		e.ThrowException = false;
		e.Cancel = true;

		switch(e.ColumnIndex)
		{
			case 5:
			case 9:
				MessageBox.Show("The entered value must be a number\n\nPress [ESC] to undo");
				break;
			case 2:
			case 6:
			case 10:
				MessageBox.Show("The entered value must be a date\n\nPress [ESC] to undo");
				break;
			default:
				MessageBox.Show($"An unknown error has occured in col {e.ColumnIndex}\n\nPress [ESC] to undo");
				break;
		}
	}
	private void DgvData_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
	{
		switch(e.ColumnIndex)
		{
			case 5:   // All numbers default to 0
			case 9:
				if(e.Value is null || (string)e.Value == "") e.Value = 0;
				e.ParsingApplied = true;
				return;

			case 2: // Added column defaults to DateTime.Now
				if(e.Value is null || (string)e.Value == "") e.Value = DateTime.Now;
				e.ParsingApplied = true;
				return;
		}
	}
	private void DgvData_MouseUp(object sender, MouseEventArgs e)
	{
		if(e.Button == MouseButtons.Right)
		{
			int rowIndex = dgvData.HitTest(e.X, e.Y).RowIndex;
			dgvData.ClearSelection();
			if(rowIndex == -1) return;

			dgvData.CurrentCell = dgvData.Rows[rowIndex].Cells[0];
			dgvData.Rows[rowIndex].Selected = true;

			if(dgvData.Rows[rowIndex].DataBoundItem is not Neighbor n)
			{
				Debug.WriteLine(dgvData.Rows[rowIndex].DataBoundItem.GetType());
				return;
			}


			ContextMenuStrip cm = new();

			cm.Items.Add(new ToolStripMenuItem($"Insert Row", null, CtxInsert)
			{
				Tag = new ContextEventArgs(n, rowIndex)
			});


			cm.Items.Add(new ToolStripSeparator());


			cm.Items.Add(new ToolStripMenuItem("Delete Row", null, CtxDelete)
			{
				Tag = new ContextEventArgs(n, rowIndex)
			});


			cm.Items.Add(new ToolStripSeparator());

			if(rowIndex > 0)
				cm.Items.Add(new ToolStripMenuItem("Move Row Up", null, CtxMoveUp)
				{
					Tag = new ContextEventArgs(n, rowIndex)
				});


			cm.Items.Add(new ToolStripSeparator());

			if(rowIndex < data.neighbors.Count)
				cm.Items.Add(new ToolStripMenuItem("Move Row Down", null, CtxMoveDown)
				{
					Tag = new ContextEventArgs(n, rowIndex)
				});

			Utils.ShowContextOnScreen(this, cm, dgvData.PointToScreen(e.Location));
			return;
		}
	}


	private static ContextEventArgs? GetContextArgs(string method, object? sender)
	{
		if(sender is null)
		{
			Program.Log("EditForm.GetCxtArgs", $"*** {method} Sender was null");
			return null;
		}
		if(sender is not ToolStripMenuItem tsmi)
		{
			Program.Log("EditForm.GetCxtArgs", $"*** {method} Sender was {sender.GetType()} expecting ToolStripMenuItem");
			return null;
		}
		if(tsmi.Tag is not ContextEventArgs ctxa)
		{
			Program.Log("EditForm.GetCxtArgs", $"*** {method} Sender.Tag was {tsmi.Tag.GetType()} expecting ContextEventArgs");
			return null;
		}
		return ctxa;
	}

	private void CtxInsert(object? sender, EventArgs? e)
	{
		ContextEventArgs? ctxa = GetContextArgs("EditForm.CtxInsert", sender);
		if(ctxa is null) return;

		Neighbor newNeighbor = new()
		{
			Order = ctxa.Neighbor.Order,
			Group = ctxa.Neighbor.Group
		};

		bindSource.Insert(ctxa.RowIndex, newNeighbor);

		dgvData.CurrentCell = dgvData.Rows[ctxa.RowIndex].Cells[0];
		data.ReOrder();
	}
	private void CtxDelete(object? sender, EventArgs? e)
	{
		ContextEventArgs? ctxa = GetContextArgs("EditForm.CtxDelete", sender);
		if(ctxa is null) return;

		bindSource.Remove(ctxa.Neighbor);
		data.ReOrder();
	}
	private void CtxMoveUp(object? sender, EventArgs? e)
	{
		ContextEventArgs? ctxa = GetContextArgs("EditForm.CtxMoveUp", sender);
		if(ctxa is null) return;

		int rowToMove = ctxa.RowIndex - 1;
		while(rowToMove >= 0)
		{
			if(dgvData.Rows[rowToMove].Visible)
			{
				data.neighbors.Insert(ctxa.RowIndex + 1, data.neighbors[rowToMove]);
				data.neighbors.RemoveAt(rowToMove);
				break;
			}
			rowToMove--;
		}
		data.ReOrder();
	}
	private void CtxMoveDown(object? sender, EventArgs? e)
	{
		ContextEventArgs? ctxa = GetContextArgs("EditForm.CtxMoveDown", sender);
		if(ctxa is null) return;

		int rowToMove = ctxa.RowIndex + 1;
		while(rowToMove < dgvData.Rows.Count - 1)
		{
			if(dgvData.Rows[rowToMove].Visible)
			{
				data.neighbors.Insert(ctxa.RowIndex, data.neighbors[rowToMove]);
				data.neighbors.RemoveAt(rowToMove + 1);
				break;
			}
			rowToMove++;
		}
		data.ReOrder();
	}

}
