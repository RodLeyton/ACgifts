using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ACgifts;

public partial class MainForm:Form
{
	private readonly Data data;
	private readonly LVsort lvSendSort, lvRecvSort;
	private readonly DetailForm detailForm;
	private int sentGroup = 0, recvGroup = 0, sentToday = 0, recvToday = 0, cntGroup = 0;

	public MainForm()
	{
		InitializeComponent();
		data = new();
		detailForm = new();

		lvRecv.IsSend = false;
		lvRecv.ShowItemToolTips = true;
		lvRecvSort = new LVsort(false);
		lvRecv.ListViewItemSorter = lvRecvSort;
		lvRecvSort.SortType = (SortOrderTypes)Program.appConfig.SortOrder;

		lvRecv.Columns.Add("Recv Name", Program.appConfig.Col0width);
		lvRecv.Columns.Add("Button", Program.appConfig.Col1width);
		lvRecv.Columns.Add("Last", Program.appConfig.Col2width);
		lvRecv.Columns.Add("Cnt", Program.appConfig.Col3width);
		lvRecv.Columns.Add("Rate", Program.appConfig.Col4width);
		lvRecv.Columns.Add("Added", Program.appConfig.Col5width);


		lvSend.IsSend = true;
		lvSend.ShowItemToolTips = true;
		lvSendSort = new LVsort(true);
		lvSend.ListViewItemSorter = lvSendSort;
		lvSendSort.SortType = (SortOrderTypes)Program.appConfig.SortOrder;

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

		foreach(SortOrderTypes val in Enum.GetValues(typeof(SortOrderTypes)))
			cbSortOrder.Items.Add(Utils.GetEnumDescription(val));

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


		if(data.neighbors.Count == 0 && MessageBox.Show("No datafile exists.\r\nWould you like to create demo data?",
			 "ACgifts - Create demo data?", MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			data.CreateDemoData();
			UpdateGroupsLV();
		}
	}
	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		detailForm.CloseForm();
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
		bool skipSentAlready = false, skipAsked = false;
		timer1.Enabled = false;

		foreach(Neighbor n in data.neighbors)
		{
			if(lbGroups.SelectedItem?.ToString() != n.Group) continue;

			if(n.SendThisSess && !skipAsked)
			{
				string msg = @"Some gifts have already been sent to these neighbors during this session.
Do you want to skip these and not send again?

Yes => only send to those you have not sent a gift to already,
No => Send a gift to everyone";
				if(MessageBox.Show(msg, "ACgifts - Some gifts already sent", MessageBoxButtons.YesNo) == DialogResult.Yes) skipSentAlready = true;
				skipAsked = true;
			}
			if(n.SendThisSess && skipSentAlready) continue;

			n.AddSend();
			sentGroup++;
			sentToday++;
		}
		lvSend.Refresh();
		UpdateTotals();
		timer1.Enabled = true;


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
		lbGroups.Items.Add("All");
		cbSortOrder.Enabled = true;

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
		lbGroups.Sorted = false;
		if(lbGroups.Items.Count > 0) lbGroups.SelectedIndex = 1;
	}
	private void LbGroups_SelectedIndexChanged(object? sender, EventArgs? e)
	{
		if(lbGroups.SelectedItem is null) return;
		cbSortOrder.Enabled = true;

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
			if(n.HasRecvToday) recvToday++;
			if(n.HasSendToday) sentToday++;

			string selGroup = (string)lbGroups.SelectedItem;
			if(selGroup == "unassigned")
			{
				if(n.Group.Trim() != "") continue;
			}
			else if(selGroup != "All" && selGroup != n.Group) continue;

			cntGroup++;
			if(n.HasRecvToday) recvGroup++;
			if(n.HasSendToday) sentGroup++;


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
		lvRecv.EndUpdate();
		lvSend.EndUpdate();

		SortLvs();
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

			cm.Items.Add(new ToolStripMenuItem($"Find '{n.Name}' in all groups", null, CtxFindSimalar) { Tag = n });
			//cm.Items.Add(new ToolStripMenuItem($"Show not yet Recv", null, CtxNotYet));
			cm.Items.Add(new ToolStripSeparator());

			if(n.RecvThisSess)
				cm.Items.Add(new ToolStripMenuItem($"Undo Recv from '{n.NameRecv}'", null, CtxUndoRecv) { Tag = n });
			else cm.Items.Add(new ToolStripMenuItem($"Cannot Undo Recv from '{n.NameRecv}'", null) { Enabled = false });

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
				if(lvRecv.InvokeRequired)
				{
					lvRecv.Invoke(() =>
					{
						lvRecv.Refresh();
						lvRecv.Sort();
					});
					return;
				}
				lvRecv.Refresh();
				lvRecv.Sort();
			});
			return;
		}

		if(detailForm.IsDisposed) return;
		if(detailForm.Visible) detailForm.Visible = false;
		else detailForm.UpdateData(n, lvRecv.PointToScreen(e.Location), RefreshForm);
	}
	private void LvSend_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvSend.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvSend.HitTest(mousePos);
		if(hitTest.Item?.Tag is not Neighbor n) return;

		if(e.Button == MouseButtons.Right)
		{
			ContextMenuStrip cm = new();
			cm.Items.Add(new ToolStripMenuItem($"Find '{n.Name}' in all groups", null, CtxFindSimalar) { Tag = n });
			//cm.Items.Add(new ToolStripMenuItem($"Show not yet Sent", null, CtxNotYet));
			cm.Items.Add(new ToolStripSeparator());
			cm.Items.Add(new ToolStripMenuItem($"Send to all but '{n.NameSend}'", null, CtxSendAllBut) { Tag = n });
			cm.Items.Add(new ToolStripSeparator());

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
				if(lvSend.InvokeRequired)
				{
					lvSend.Invoke(() =>
					{
						lvSend.Refresh();
						lvSend.Sort();
					});
					return;
				}
				lvSend.Refresh();
				lvSend.Sort();
			});
			return;
		}

		if(detailForm.IsDisposed) return;
		if(detailForm.Visible) detailForm.Visible = false;
		else detailForm.UpdateData(n, lvSend.PointToScreen(e.Location), RefreshForm);
	}
	private void CtxSendAllBut(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not Neighbor nNoSend) return;

		bool skipSentAlready = false, skipAsked = false;
		timer1.Enabled = false;


		foreach(Neighbor n in data.neighbors)
		{
			if(n.Equals(nNoSend)) continue;
			if(lbGroups.SelectedItem?.ToString() != n.Group) continue;

			if(n.SendThisSess && !skipAsked)
			{
				string msg = @"Some gifts have already been sent to these neighbors during this session.
Do you want to skip these and not send again?

Yes => only send to those you have not sent a gift to already,
No => Send a gift to everyone";
				if(MessageBox.Show(msg, "ACgifts - Some gifts already sent", MessageBoxButtons.YesNo) == DialogResult.Yes) skipSentAlready = true;
				skipAsked = true;
			}
			if(n.SendThisSess && skipSentAlready) continue;

			n.AddSend();
			sentGroup++;
			sentToday++;
		}
		lvSend.Refresh();
		UpdateTotals();
		timer1.Enabled = true;


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
	private void CtxFindSimalar(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not Neighbor nFind) return;

		gbGroup.Text = "";
		lbGroups.ClearSelected();
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
			if(n.HasSendToday) sentToday++;
			if(n.HasRecvToday) recvToday++;

			bool include = false;
			if(nFind.IdRecv == n.IdRecv) include = true;
			else if(nFind.IdSend == n.IdSend) include = true;
			else if(nFind.Name == n.Name) include = true;
			else if(nFind.NameSend == n.NameSend) include = true;
			else if(nFind.NameRecv == n.NameRecv) include = true;
			if(!include) continue;


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

		lvRecv.EndUpdate();
		lvSend.EndUpdate();
		UpdateTotals();
		SortLvs();
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
		SortLvs();
	}

	private void SortLvs()
	{
		lvSendSort.SortType = (SortOrderTypes)cbSortOrder.SelectedIndex;
		lvRecvSort.SortType = (SortOrderTypes)cbSortOrder.SelectedIndex;

		lvSend.Sort();
		lvRecv.Sort();
	}

	private void RefreshForm()
	{
		lvSend.Refresh();
		lvRecv.Refresh();
		lvSend.Sort();
		lvRecv.Sort();
	}

	private void ButAnalysis_Click(object sender, EventArgs e)
	{
		new StatsForm(data).ShowDialog();
	}

	private void LvRecv_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		Debug.WriteLine($"{e.OldDisplayIndex} -> {e.NewDisplayIndex}");
	}
}
