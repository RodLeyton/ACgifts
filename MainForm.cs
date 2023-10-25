using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Security.Policy;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ACgifts;

public partial class MainForm:Form
{
	private readonly Data data;
	private readonly LvExMainSort lvSendSort, lvRecvSort;
	private readonly DetailForm detailForm;
	private int sentGroup = 0, recvGroup = 0, sentToday = 0, recvToday = 0, cntGroup = 0;


	public MainForm()
	{
		InitializeComponent();
		data = new();
		detailForm = new();


		lvRecv.IsSend = false;
		lvRecvSort = new LvExMainSort(false);
		lvRecv.ListViewItemSorter = lvRecvSort;
		lvRecvSort.SortType = (LvExMainSortTypes)Program.appConfig.SortOrder;

		lvRecv.HeaderContextMenu = new() { Tag = lvRecv };
		lvRecv.HeaderContextMenu.Opening += LvEx_HeadCtxMenu_Opening;
		lvRecv.HeaderContextMenu.Closing += LvEx_HeadCtxMenu_Closing;


		lvSend.IsSend = true;
		lvSendSort = new LvExMainSort(true);
		lvSend.ListViewItemSorter = lvSendSort;
		lvSendSort.SortType = (LvExMainSortTypes)Program.appConfig.SortOrder;

		lvSend.HeaderContextMenu = new() { Tag = lvSend };
		lvSend.HeaderContextMenu.Opening += LvEx_HeadCtxMenu_Opening;
		lvSend.HeaderContextMenu.Closing += LvEx_HeadCtxMenu_Closing;


		// Configure columns for listviews
		int cntCols = 0;
		foreach(LvExMainColumns val in Enum.GetValues(typeof(LvExMainColumns)))
		{
			string desc = Utils.GetEnumDescription(val);
			lvRecv.Columns.Add(desc, 50);
			lvSend.Columns.Add(desc, 50);
			cntCols++;
		}


		List<KeyValuePair<LvExMainColumns, ColumnConfig>> ordRecvCols = Program.appConfig.RecvCols.OrderBy(col => col.Value.Order).ToList();
		foreach(var kvp in ordRecvCols)
		{
			lvRecv.Columns[(int)kvp.Key].Width = kvp.Value.Width;
			lvRecv.Columns[(int)kvp.Key].DisplayIndex = kvp.Value.Order;
		}

		List<KeyValuePair<LvExMainColumns, ColumnConfig>> ordSendCols = Program.appConfig.SendCols.OrderBy(col => col.Value.Order).ToList();
		foreach(var kvp in ordSendCols)
		{
			lvSend.Columns[(int)kvp.Key].Width = kvp.Value.Width;
			lvSend.Columns[(int)kvp.Key].DisplayIndex = kvp.Value.Order;
		}
	}


	private void MainForm_Load(object sender, EventArgs e)
	{
		// Restore saved form size/position
		string geo = Program.appConfig.MainFormGeo;
		if(geo == null || geo == "")
		{
			Width = MinimumSize.Width;
			Height = MinimumSize.Height + 200;
		}
		else WindowRestore.GeometryFromString(geo, this);
		MainForm_Resize(null, null);


		if(Program.appConfig.SplitDistance > Math.Max(50, spliter.Panel1MinSize))
			spliter.SplitterDistance = Program.appConfig.SplitDistance;
		else spliter.SplitterDistance = (spliter.Width - spliter.SplitterWidth) / 2;


		// Load avaliable sort orders
		foreach(LvExMainSortTypes val in Enum.GetValues(typeof(LvExMainSortTypes)))
			cbSortOrder.Items.Add(Utils.GetEnumDescription(val));
		cbSortOrder.SelectedIndex = Program.appConfig.SortOrder;

		// Load data
		data.Load();
		UpdateGroupsLV();
	}
	private void MainForm_Shown(object sender, EventArgs e)
	{
		// Just for convience and to reduce accidents while debugging
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

		// Offer to create demo data if none exists
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

		// Save form config
		Program.appConfig.SplitDistance = spliter.SplitterDistance;
		Program.appConfig.SortOrder = cbSortOrder.SelectedIndex;
		Program.appConfig.MainFormGeo = WindowRestore.GeometryToString(this);
		Program.SaveConfig();
	}
	private void MainForm_Resize(object? sender, EventArgs? e)
	{
		butSendAll.Left = Math.Max(spliter.Left + spliter.Panel2.Left, cbSortOrder.Right + 20);
	}
	private void MainForm_ResizeEnd(object sender, EventArgs e)
	{
		Invalidate();  // Required to remove border artifacts while resizing
		spliter.Invalidate();
	}
	private void Timer1_Tick(object sender, EventArgs e)
	{
		// Periodic update for age string in listviews
		lvSend.Refresh();
		lvRecv.Refresh();
	}





	// Menu clicks
	private void Menu_Website_Click(object sender, EventArgs e)
	{
		Utils.OpenUrl("https://rodleyton.github.io/ACgifts/");
	}
	private void Menu_FAQ_Click(object sender, EventArgs e)
	{
		Utils.OpenUrl("https://rodleyton.github.io/ACgifts/faq.htm");
	}
	private void Menu_GitHub_Click(object sender, EventArgs e)
	{
		Utils.OpenUrl("https://github.com/RodLeyton/ACgifts");
	}
	private void Menu_Forum_Click(object sender, EventArgs e)
	{
		Utils.OpenUrl("https://www.airportcitygame.com/members/crash-and-burn.29038/#about");
	}
	private void Menu_Stats_Click(object sender, EventArgs e)
	{
		new StatsForm(data).ShowDialog();
	}
	private void Menu_Edit_Click(object sender, EventArgs e)
	{
		new EditForm(data).ShowDialog();
	}
	private void MenuBackup_Click(object sender, EventArgs e)
	{
		data.Backup();
	}
	private void MenuRestore_Click(object sender, EventArgs e)
	{
		data.Restore();
	}
	private void MenuImport_Click(object sender, EventArgs e)
	{
		OpenFileDialog ofd = new() { Filter = "csv|*.csv" };
		if(ofd.ShowDialog() == DialogResult.OK)
			data.Import(ofd.FileName);
	}
	private void MenuExport_Click(object sender, EventArgs e)
	{
		data.Export();
	}
	private void MenuSave_Click(object sender, EventArgs e)
	{
		data.Save();
	}
	private void MenuExit_Click(object sender, EventArgs e)
	{
		Close();
	}
	private void MenuAutoFormWidth_Click(object sender, EventArgs e)
	{
		lvRecv.AutoResizeColumns();
		lvSend.AutoResizeColumns();


		int wA = 0, wB = 0;
		foreach(ColumnHeader c in lvRecv.Columns) wA += c.Width;
		foreach(ColumnHeader c in lvSend.Columns) wB += c.Width;
		wA += Utils.GetVertScrollbarWidth(lvRecv) + 5;
		wB += Utils.GetVertScrollbarWidth(lvSend) + 5;

		Width = spliter.Left + wA + spliter.SplitterWidth + wB + Width - spliter.Right;
		spliter.SplitterDistance = wA;
	}
	private void MenuSaveLayout_Click(object sender, EventArgs e)
	{
		Program.SaveConfig();
	}






	private void ButSendAll_Click(object sender, EventArgs e)
	{
		bool skipSentAlready = false, skipAsked = false;
		timer1.Enabled = false;


		foreach(object obj in lvSend.Items)
		{
			if(obj is not LviNeighbor lvi) throw new Exception($"lvSend contains an invalid item. {obj}");

			if(lvi.SendThisSess && !skipAsked)
			{
				string msg = @"Some gifts have already been sent to these neighbors during this session.
Do you want to skip these and not send again?

Yes => only send to those you have not sent a gift to already,
No => Send a gift to everyone";
				if(MessageBox.Show(msg, "ACgifts - Some gifts already sent", MessageBoxButtons.YesNo) == DialogResult.Yes) skipSentAlready = true;
				skipAsked = true;
			}
			if(lvi.SendThisSess && skipSentAlready) continue;

			lvi.AddSend();
			sentGroup++;
			sentToday++;
		}

		lvSend.Refresh();
		UpdateTotals();
		timer1.Enabled = true;


		Task.Run(async delegate
		{
			await Task.Delay(LvExMain.BUT_DISABLE_MILLIS + 10);
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
	private void ButAnalysis_Click(object sender, EventArgs e)
	{
		new StatsForm(data).ShowDialog();
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
				if(!lbGroups.Items.Contains("Unassigned")) lbGroups.Items.Add("Unassigned");
				continue;
			}
			if(!lbGroups.Items.Contains(n.Group))
				lbGroups.Items.Add(n.Group);
		}
		lbGroups.Sorted = false;
		if(lbGroups.Items.Count > 0) lbGroups.SelectedIndex = 1;
	}
	private void SortLvs()
	{
		lvSendSort.SortType = (LvExMainSortTypes)cbSortOrder.SelectedIndex;
		lvRecvSort.SortType = (LvExMainSortTypes)cbSortOrder.SelectedIndex;

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
	private static void SaveLvColConfig(LvExMain lv)
	{
		if(lv.Columns[0].DisplayIndex != 0) lv.Columns[0].DisplayIndex = 0;
		if(lv.Columns[0].Width > 0) lv.Columns[0].Width = 0;

		Dictionary<LvExMainColumns, ColumnConfig> colConfig = lv.IsSend ? Program.appConfig.SendCols : Program.appConfig.RecvCols;

		for(int i = 1; i < lv.Columns.Count; i++)
		{
			if(!colConfig.TryGetValue((LvExMainColumns)i, out ColumnConfig? cc))
				colConfig.Add((LvExMainColumns)i, new(lv.Columns[i].DisplayIndex, lv.Columns[i].Width));
			else
			{
				cc.Order = lv.Columns[i].DisplayIndex;
				cc.Width = lv.Columns[i].Width;
			}
		}
	}



	private void Spliter_SplitterMoved(object sender, SplitterEventArgs e)
	{
		butSendAll.Left = Math.Max(spliter.Left + spliter.Panel2.Left, cbSortOrder.Right + 20);
		spliter.Invalidate();
	}
	private void Spliter_MouseUp(object sender, MouseEventArgs e)
	{
		ContextMenuStrip cm = new();
		cm.Items.Add(new ToolStripMenuItem("Make widths equal", null, CtxSpliterMid));
		Utils.ShowContextOnScreen(this, cm, lvRecv.PointToScreen(e.Location));
	}
	private void CtxSpliterMid(object? sender, EventArgs? e)
	{
		spliter.SplitterDistance = (spliter.ClientSize.Width - spliter.SplitterWidth) / 2;
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
			if(selGroup == "Unassigned")
			{
				if(n.Group.Trim() != "") continue;
			}
			else if(selGroup != "All" && selGroup != n.Group) continue;

			cntGroup++;
			if(n.HasRecvToday) recvGroup++;
			if(n.HasSendToday) sentGroup++;

			lvRecv.Items.Add(new LviNeighbor(n));
			lvSend.Items.Add(new LviNeighbor(n));
		}
		lvRecv.EndUpdate();
		lvSend.EndUpdate();

		SortLvs();
		UpdateTotals();
	}
	private void CbSortOrder_SelectedIndexChanged(object sender, EventArgs e)
	{
		SortLvs();
	}



	private void LvRecv_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvRecv.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvRecv.HitTest(mousePos);
		if(hitTest.Item is not LviNeighbor lvi) throw new Exception($"LvRecv contained an invalid item. {hitTest.Item}");

		if(e.Button == MouseButtons.Right)
		{
			ContextMenuStrip cm = new();

			cm.Items.Add(new ToolStripMenuItem($"Find '{lvi.Name}' in all groups", null, CtxFindSimalar) { Tag = lvi.Neighbor });
			cm.Items.Add(new ToolStripSeparator());

			if(lvi.RecvThisSess)
				cm.Items.Add(new ToolStripMenuItem($"Undo Recv from '{lvi.NameRecv}'", null, CtxUndoRecv) { Tag = lvi.Neighbor });
			else cm.Items.Add(new ToolStripMenuItem($"Cannot Undo Recv from '{lvi.NameRecv}'", null) { Enabled = false });

			Utils.ShowContextOnScreen(this, cm, lvRecv.PointToScreen(e.Location));
			return;
		}


		if(hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == (int)LvExMainColumns.Button)
		{
			// Reject double accidental clicks
			if(lvi.LastRecv != null && (DateTime.Now - (DateTime)lvi.LastRecv).TotalMilliseconds < LvExMain.BUT_DISABLE_MILLIS) return;

			lvi.AddRecv();
			lvRecv.Refresh();
			recvGroup++;
			recvToday++;
			UpdateTotals();

			Task.Run(async delegate
			{
				await Task.Delay(LvExMain.BUT_DISABLE_MILLIS + 10);
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
		else detailForm.UpdateData(lvi.Neighbor, lvRecv.PointToScreen(e.Location), RefreshForm);
	}
	private void LvSend_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvSend.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvSend.HitTest(mousePos);
		if(hitTest.Item is not LviNeighbor lvi) throw new Exception($"LvSend contained an invalid item. {hitTest.Item}");

		if(e.Button == MouseButtons.Right)
		{
			ContextMenuStrip cm = new();
			cm.Items.Add(new ToolStripMenuItem($"Find '{lvi.Name}' in all groups", null, CtxFindSimalar) { Tag = lvi.Neighbor });
			cm.Items.Add(new ToolStripSeparator());
			cm.Items.Add(new ToolStripMenuItem($"Send to all but '{lvi.NameSend}'", null, CtxSendAllBut) { Tag = lvi.Neighbor });
			cm.Items.Add(new ToolStripSeparator());

			if(lvi.SendThisSess)
				cm.Items.Add(new ToolStripMenuItem($"Undo Send to '{lvi.NameSend}'", null, CtxUndoSend) { Tag = lvi.Neighbor });
			else cm.Items.Add(new ToolStripMenuItem($"Cannot Undo Send to '{lvi.NameSend}'", null) { Enabled = false });

			Utils.ShowContextOnScreen(this, cm, lvSend.PointToScreen(e.Location));
			return;
		}

		if(hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == (int)LvExMainColumns.Button)
		{
			// Reject double accidental clicks
			if(lvi.LastSend != null && (DateTime.Now - (DateTime)lvi.LastSend).TotalMilliseconds < LvExMain.BUT_DISABLE_MILLIS) return;

			lvi.AddSend();
			lvSend.Refresh();
			sentGroup++;
			sentToday++;
			UpdateTotals();

			Task.Run(async delegate
			{
				await Task.Delay(LvExMain.BUT_DISABLE_MILLIS + 10);
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
		else detailForm.UpdateData(lvi.Neighbor, lvSend.PointToScreen(e.Location), RefreshForm);
	}
	private void LvEx_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
	{
		if(e.ColumnIndex == 0) e.Cancel = true;
	}
	private void LvEx_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		if(sender is LvExMain lv) SaveLvColConfig(lv);
	}
	private void LvEx_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		if(e.OldDisplayIndex == 0) e.Cancel = true;
		else
		{
			if(sender is not LvExMain lv) return;
			Task.Run(async delegate
			{
				await Task.Delay(100);
				if(IsDisposed || !IsHandleCreated || Disposing) return;

				if(lv.InvokeRequired) lv.Invoke(() => SaveLvColConfig(lv));
				else SaveLvColConfig(lv);
			});
		}
	}





	private void CtxSendAllBut(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not Neighbor nNoSend) return;

		bool skipSentAlready = false, skipAsked = false;
		timer1.Enabled = false;


		foreach(object obj in lvSend.Items)
		{
			if(obj is not LviNeighbor lvi) throw new Exception($"lvSend contains an invalid item. {obj}");

			if(lvi.Neighbor.Equals(nNoSend)) continue;

			if(lvi.SendThisSess && !skipAsked)
			{
				string msg = @"Some gifts have already been sent to these neighbors during this session.
Do you want to skip these and not send again?

Yes => only send to those you have not sent a gift to already,
No => Send a gift to everyone";
				if(MessageBox.Show(msg, "ACgifts - Some gifts already sent", MessageBoxButtons.YesNo) == DialogResult.Yes) skipSentAlready = true;
				skipAsked = true;
			}
			if(lvi.SendThisSess && skipSentAlready) continue;

			lvi.AddSend();
			sentGroup++;
			sentToday++;
		}

		lvSend.Refresh();
		UpdateTotals();
		timer1.Enabled = true;


		Task.Run(async delegate
		{
			await Task.Delay(LvExMain.BUT_DISABLE_MILLIS + 10);
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

			lvRecv.Items.Add(new LviNeighbor(n));
			lvSend.Items.Add(new LviNeighbor(n));
		}

		lvRecv.EndUpdate();
		lvSend.EndUpdate();
		UpdateTotals();
		SortLvs();
	}



	private void LvEx_HeadCtxMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		if(sender is not ContextMenuStrip cms) return;

		cms.Items.Clear();
		if(cms.Tag is not LvExMain lv) throw new Exception("Head Context Menu had incorrect Tag");

		for(int i = 1; i < lv.Columns.Count; i++)
		{
			cms.Items.Add(new ToolStripMenuItem(lv.Columns[i].Text)
			{
				Checked = lv.Columns[i].Width > 0,
				CheckOnClick = true,
				Tag = i
			});
		}

		ToolStripButton tsBut = new("Apply")
		{
			AutoSize = true,
			BackColor = SystemColors.ButtonFace,
			Margin = new(0, 8, 0, 3),
			Padding = new(15, 0, 15, 0)
		};
		tsBut.Click += ((object? sender, EventArgs e) => cms.Close());
		cms.Items.Add(tsBut);
		e.Cancel = false;
	}
	private void LvEx_HeadCtxMenu_Closing(object? sender, ToolStripDropDownClosingEventArgs e)
	{
		if(e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
		{
			e.Cancel = true;
			return;
		}

		if(sender is not ContextMenuStrip cms) return;
		if(cms.Tag is not LvExMain lv) throw new Exception("Head Context Menu had incorrect Tag");


		foreach(object obj in cms.Items)
		{
			if(obj is not ToolStripMenuItem tsmi) continue;
			if(tsmi.Tag is not int inx || inx <= 0 || inx >= lv.Columns.Count) continue;

			if(lv.Columns[inx].Width <= 0 && tsmi.Checked) lv.Columns[inx].Width = 100;
			else if(lv.Columns[inx].Width > 0 && !tsmi.Checked) lv.Columns[inx].Width = 0;
		}
	}





}
