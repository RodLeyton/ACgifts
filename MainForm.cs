using System;
using System.Windows.Forms;

namespace ACgifts;

public partial class MainForm:Form
{
	private readonly Data data;
	private readonly LVsort lvSendSort, lvRecvSort;
	private int sentGroup = 0, recvGroup = 0, sentToday = 0, recvToday = 0, cntGroup = 0;


	public MainForm()
	{
		InitializeComponent();
		data = new();

		lvRecv.IsSend = false;
		lvRecv.ShowItemToolTips = true;
		lvRecvSort = new LVsort(false);
		lvRecv.ListViewItemSorter = lvRecvSort;
		lvRecvSort.SortType = Program.appConfig.SortOrder;

		lvRecv.Columns.Add("Recv Name", Program.appConfig.Col0width);
		lvRecv.Columns.Add("", Program.appConfig.Col1width);
		lvRecv.Columns.Add("Last", Program.appConfig.Col2width);
		lvRecv.Columns.Add("Cnt", Program.appConfig.Col3width);
		lvRecv.Columns.Add("Rate", Program.appConfig.Col4width);
		lvRecv.Columns.Add("Added", Program.appConfig.Col5width);

		lvSend.IsSend = true;
		lvSend.ShowItemToolTips = true;
		lvSendSort = new LVsort(true);
		lvSend.ListViewItemSorter = lvSendSort;
		lvSendSort.SortType = Program.appConfig.SortOrder;

		lvSend.Columns.Add("Send Name", Program.appConfig.Col0width);
		lvSend.Columns.Add("", Program.appConfig.Col1width);
		lvSend.Columns.Add("Last", Program.appConfig.Col2width);
		lvSend.Columns.Add("Cnt", Program.appConfig.Col3width);
		lvSend.Columns.Add("Rate", Program.appConfig.Col4width);
		lvSend.Columns.Add("Added", Program.appConfig.Col5width);
	}
	private void MainForm_Load(object sender, EventArgs e)
	{
		string geo = Program.appConfig.MainFormGeo;
		if(geo == null || geo == "")
		{
			Width = MaximumSize.Width;
			Height = MinimumSize.Height;
		}
		else WindowRestore.GeometryFromString(geo, this);
		MainForm_Resize(null, null);

		cbSortOrder.Items.Add(new KeyValuePair<int, string>(LVsort.AC_GAME_NAME, "Game name"));
		cbSortOrder.Items.Add(new KeyValuePair<int, string>(LVsort.FORUM_NAME, "Forum name"));
		cbSortOrder.Items.Add(new KeyValuePair<int, string>(LVsort.LIST_ORDER, "List Order"));
		cbSortOrder.SelectedIndex = Program.appConfig.SortOrder;

		data.Load();
		UpdateGroupsLV();
	}
	private void MainForm_Shown(object sender, EventArgs e)
	{
		if(Program.IsDebug)
		{
			Text = "*** Debug ***";
			BackColor = Color.PaleGoldenrod;
		} 
		else
		{
			Text = $"ACgifts  {Program.Version}";

			if(Program.Version != Program.appConfig.InstalledVersion)
			{
				Program.appConfig.InstalledVersion = Program.Version;
				LogViewForm.ShowFile($"Updated to {Program.Version}, showing change log", Path.Combine(Program.GetAppDir(), "Changelog.txt"), this);
			}
		}
	}
	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		data.Save();
		Program.appConfig.SortOrder = cbSortOrder.SelectedIndex;
		Program.appConfig.Col0width = lvSend.Columns[0].Width;
		Program.appConfig.Col1width = lvSend.Columns[1].Width;
		Program.appConfig.Col2width = lvSend.Columns[2].Width;
		Program.appConfig.Col3width = lvSend.Columns[3].Width;
		Program.appConfig.Col4width = lvSend.Columns[4].Width;
		Program.appConfig.Col5width = lvSend.Columns[5].Width;
		Program.appConfig.MainFormGeo = WindowRestore.GeometryToString(this);
		Program.SaveConfig();
	}
	private void MainForm_Resize(object? sender, EventArgs? e)
	{
		lvRecv.Left = lbGroups.Right + 10;
		cbSortOrder.Left = lvRecv.Left;
		lvRecv.Width = (this.ClientSize.Width - lvRecv.Left - 20) / 2;
		lvSend.Width = lvRecv.Width;
		lvSend.Left = lvRecv.Left + lvRecv.Width + 10;
		butSendAll.Left = lvSend.Left;
	}
	private void Timer1_Tick(object sender, EventArgs e)
	{
		lvSend.Refresh();
		lvRecv.Refresh();
	}


	private void ButSendAll_Click(object sender, EventArgs e)
	{
		foreach(Neighbor n in data.neighbors)
		{
			if(lbGroups.SelectedItem?.ToString() != n.Group) continue;
			n.AddSend();
			sentGroup++;
			sentToday++;
		}
		lvSend.Refresh();
		UpdateTotals();
		Task.Run(async delegate
		{
			await Task.Delay(LvExNeighbor.BUT_DISABLE_MILLIS + 10);
			if(IsDisposed || !IsHandleCreated || Disposing) return;
			if(lvSend.InvokeRequired) lvSend.Invoke(() => lvSend.Refresh());
			else lvSend.Refresh();
		});
	}
	private void ButEdit_Click(object sender, EventArgs e)
	{
		new EditForm(data).ShowDialog(this);
		UpdateGroupsLV();
	}




	private void UpdateTotals()
	{
		labSent.Text = $"{sentGroup}/{cntGroup}";
		labRecv.Text = $"{recvGroup}/{cntGroup}";
		labSentToday.Text = $"{sentToday}/{data.neighbors.Count}";
		labRecvToday.Text = $"{recvToday}/{data.neighbors.Count}";
	}
	private void UpdateGroupsLV()
	{
		lbGroups.Items.Clear();

		foreach(Neighbor n in data.neighbors)
		{
			if(n.Group is null || n.Group.Trim() == "")
			{
				if(!lbGroups.Items.Contains("unassigned")) lbGroups.Items.Add("unassigned");
				continue;
			}
			if(!lbGroups.Items.Contains(n.Group))
				lbGroups.Items.Add(n.Group);
		}
		lbGroups.Sorted = true;
		if(lbGroups.Items.Count > 0) lbGroups.SelectedIndex = 0;
	}
	private void LbGroups_SelectedIndexChanged(object sender, EventArgs e)
	{
		gbGroup.Text = lbGroups.SelectedItem.ToString();
		sentGroup = 0;
		recvGroup = 0;
		sentToday = 0;
		recvToday = 0;
		cntGroup = 0;

		lvRecv.BeginUpdate();
		lvRecv.Items.Clear();

		lvSend.BeginUpdate();
		lvSend.Items.Clear();


		foreach(Neighbor n in data.neighbors)
		{
			bool sent = false, recv = false;

			if(n.SendThisSess) sent = true;
			else if(n.LastSend != null && (DateTime.Now - (DateTime)n.LastSend).TotalHours < LvExNeighbor.TODAY_HOURS) sent = true;

			if(n.RecvThisSess) recv = true;
			else if(n.LastRecv != null && (DateTime.Now - (DateTime)n.LastRecv).TotalHours < LvExNeighbor.TODAY_HOURS) recv = true;

			if(sent) sentToday++;
			if(recv) recvToday++;

			if("" + lbGroups.SelectedItem == "unassigned")
			{
				if(n.Group?.Trim() != "") continue;
			}
			else if("" + lbGroups.SelectedItem != n.Group) continue;

			cntGroup++;
			if(sent) sentGroup++;
			if(recv) recvGroup++;

			ListViewItem lvi = new(n.NameRecv)
			{
				Tag = n,
				UseItemStyleForSubItems = false,
				ToolTipText = $"{n.Name}"
			};

			lvi.SubItems.Add("Recv");
			lvi.SubItems.Add("");
			lvi.SubItems.Add("");
			lvi.SubItems.Add("");
			lvi.SubItems.Add("");
			lvi.ToolTipText = $"{n.Name}";
			lvRecv.Items.Add(lvi);

			ListViewItem lvi2 = new(n.NameSend)
			{
				Tag = n,
				UseItemStyleForSubItems = false,
				ToolTipText = $"{n.Name}"
			};
			lvi2.SubItems.Add("Send");
			lvi2.SubItems.Add("");
			lvi2.SubItems.Add("");
			lvi2.SubItems.Add("");
			lvi2.SubItems.Add("");
			lvSend.Items.Add(lvi2);
		}

		lvRecv.Sort();
		lvSend.Sort();

		lvRecv.EndUpdate();
		lvSend.EndUpdate();
		UpdateTotals();
	}




	private void LvRecv_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvRecv.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvRecv.HitTest(mousePos);
		if(hitTest.Item?.Tag is not Neighbor n) return;

		if(e.Button == MouseButtons.Right)
		{
			ContextMenuStrip cm = new();
			if(n.RecvThisSess)
				cm.Items.Add(new ToolStripMenuItem($"Undo Recv from '{n.NameSend}'", null, CtxUndoRecv) { Tag = n });
			else cm.Items.Add(new ToolStripMenuItem($"Cannot Undo Recv from '{n.NameSend}'", null) { Enabled = false });
			Utils.ShowContextOnScreen(this, cm, lvRecv.PointToScreen(e.Location));
			return;
		}


		if(hitTest.SubItem?.Text == "Recv")
		{
			// Reject double accidental clicks
			if(n.LastRecv != null && (DateTime.Now - (DateTime)n.LastRecv).TotalMilliseconds < LvExNeighbor.BUT_DISABLE_MILLIS) return;

			n.AddRecv();
			lvRecv.Refresh();
			recvGroup++;
			recvToday++;
			UpdateTotals();

			Task.Run(async delegate
			{
				await Task.Delay(LvExNeighbor.BUT_DISABLE_MILLIS + 10);
				if(IsDisposed || !IsHandleCreated || Disposing) return;
				if(lvRecv.InvokeRequired) lvRecv.Invoke(() => lvRecv.Refresh());
				else lvRecv.Refresh();
			});
			return;
		}
	}
	private void LvSend_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvSend.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvSend.HitTest(mousePos);
		if(hitTest.Item?.Tag is not Neighbor n) return;

		if(e.Button == MouseButtons.Right)
		{
			ContextMenuStrip cm = new();
			cm.Items.Add(new ToolStripMenuItem($"Send to all but '{n.NameSend}'", null, CtxSendAllBut) { Tag = n });

			if(n.SendThisSess)
				cm.Items.Add(new ToolStripMenuItem($"Undo Send to '{n.NameSend}'", null, CtxUndoSend) { Tag = n });
			else cm.Items.Add(new ToolStripMenuItem($"Cannot Undo Send to '{n.NameSend}'", null) { Enabled = false });

			Utils.ShowContextOnScreen(this, cm, lvSend.PointToScreen(e.Location));
			return;
		}

		if(hitTest.SubItem?.Text == "Send")
		{
			// Reject double accidental clicks
			if(n.LastSend != null && (DateTime.Now - (DateTime)n.LastSend).TotalMilliseconds < LvExNeighbor.BUT_DISABLE_MILLIS) return;

			n.AddSend();
			lvSend.Refresh();
			sentGroup++;
			sentToday++;
			UpdateTotals();

			Task.Run(async delegate
			{
				await Task.Delay(LvExNeighbor.BUT_DISABLE_MILLIS + 10);
				if(IsDisposed || !IsHandleCreated || Disposing) return;
				if(lvSend.InvokeRequired) lvSend.Invoke(() => lvSend.Refresh());
				else lvSend.Refresh();
			});
			return;
		}
	}
	private void CtxSendAllBut(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not Neighbor nNoSend) return;

		foreach(Neighbor n in data.neighbors)
		{
			if(n.Equals(nNoSend)) continue;
			if(lbGroups.SelectedItem?.ToString() != n.Group) continue;
			n.AddSend();
			sentGroup++;
			sentToday++;
		}
		lvSend.Refresh();
		UpdateTotals();
		Task.Run(async delegate
		{
			await Task.Delay(LvExNeighbor.BUT_DISABLE_MILLIS + 10);
			if(IsDisposed || !IsHandleCreated || Disposing) return;
			if(lvSend.InvokeRequired) lvSend.Invoke(() => lvSend.Refresh());
			else lvSend.Refresh();
		});
	}
	private void CtxUndoSend(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not Neighbor n) return;

		if(!n.UndoSend())
		{
			Program.Log("MainForm.CtxUndoSend", $"Error! {n.Name}   cnt:{n.CntSend}    sess:{n.SendThisSess}    Prev:{n.PrevSend}");
			return;
		}
		sentGroup--;
		sentToday--;
		UpdateTotals();
		lvSend.Refresh();
	}
	private void CtxUndoRecv(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not Neighbor n) return;

		if(!n.UndoRecv())
		{
			Program.Log("MainForm.CtxUndoRecv", $"Error! {n.Name}   cnt:{n.CntRecv}    sess:{n.RecvThisSess}    Prev:{n.PrevRecv}");
			return;
		}
		recvGroup--;
		recvToday--;
		UpdateTotals();
		lvRecv.Refresh();
	}



	private void LvRecv_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
	{
		lvSend.Columns[e.ColumnIndex].Width = e.NewWidth;
	}
	private void LvSend_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
	{
		lvRecv.Columns[e.ColumnIndex].Width = e.NewWidth;
	}


	private void CbSortOrder_SelectedIndexChanged(object sender, EventArgs e)
	{
		lvSendSort.SortType = cbSortOrder.SelectedIndex;
		lvRecvSort.SortType = cbSortOrder.SelectedIndex;
		lvRecv.Sort();
		lvSend.Sort();
	}

}
