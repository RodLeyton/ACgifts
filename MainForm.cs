namespace ACgifts;

public partial class MainForm:Form
{
	private readonly Data data;
	private readonly LVsort lvSendSort, lvRecvSort;
	private int sentGroup = 0, recvGroup = 0, sentToday = 0, recvToday = 0, cntGroup = 0;


	public MainForm()
	{
		InitializeComponent();
		string versionString = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion") ?? "0.0.0.0";
		Version version = Version.Parse(versionString);
		Text = $"ACgifts  v{version}";

		data = new();

		lvRecv.IsSend = false;
		lvRecv.ShowItemToolTips = true;
		lvRecvSort = new LVsort(false);
		lvRecv.ListViewItemSorter = lvRecvSort;

		lvRecv.Columns.Add("Recv Name", Properties.Settings.Default.Col0width);
		lvRecv.Columns.Add("", Properties.Settings.Default.Col1width);
		lvRecv.Columns.Add("Last", Properties.Settings.Default.Col2width);
		lvRecv.Columns.Add("Cnt", Properties.Settings.Default.Col3width);
		lvRecv.Columns.Add("Rate", Properties.Settings.Default.Col4width);

		lvSend.IsSend = true;
		lvSend.ShowItemToolTips = true;
		lvSendSort = new LVsort(true);
		lvSend.ListViewItemSorter = lvSendSort;

		lvSend.Columns.Add("Send Name", Properties.Settings.Default.Col0width);
		lvSend.Columns.Add("", Properties.Settings.Default.Col1width);
		lvSend.Columns.Add("Last", Properties.Settings.Default.Col2width);
		lvSend.Columns.Add("Cnt", Properties.Settings.Default.Col3width);
		lvSend.Columns.Add("Rate", Properties.Settings.Default.Col4width);
	}
	private void MainForm_Load(object sender, EventArgs e)
	{
		string geo = Properties.Settings.Default.MainFormGeo;
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
		cbSortOrder.SelectedIndex = Properties.Settings.Default.SortOrder;
		lvSendSort.SortType = cbSortOrder.SelectedIndex;
		lvRecvSort.SortType = cbSortOrder.SelectedIndex;

		data.Load();
		UpdateGroupsLV();
	}

	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		data.Save();
		Properties.Settings.Default.SortOrder = cbSortOrder.SelectedIndex;
		Properties.Settings.Default.Col0width = lvSend.Columns[0].Width;
		Properties.Settings.Default.Col1width = lvSend.Columns[1].Width;
		Properties.Settings.Default.Col2width = lvSend.Columns[2].Width;
		Properties.Settings.Default.Col3width = lvSend.Columns[3].Width;
		Properties.Settings.Default.Col4width = lvSend.Columns[4].Width;
		Properties.Settings.Default.MainFormGeo = WindowRestore.GeometryToString(this);
		Properties.Settings.Default.Save();
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
	}
	private void ButEdit_Click(object sender, EventArgs e)
	{
		new EditForm(data).ShowDialog(this);
		UpdateGroupsLV();
	}




	private void UpdateTotals()
	{
		labSent.Text = $"Sent   {sentGroup}/{cntGroup}";                         // Note contains U+2009   &&thinsp;
		labRecv.Text = $"Recv  {recvGroup}/{cntGroup}";
		labSentToday.Text = $"Sent   {sentToday}/{data.neighbors.Count}";          // Note contains U+2009   &&thinsp;
		labRecvToday.Text = $"Recv  {recvToday}/{data.neighbors.Count}";
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

		if(hitTest.SubItem?.Text == "Recv")
		{
			if(hitTest.Item?.Tag is not Neighbor n) return;

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
					lvRecv.Invoke(() => lvRecv.Refresh());
				}
				else
				{
					lvRecv.Refresh();
				}
			});
			return;
		}

	}
	private void LvSend_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvSend.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvSend.HitTest(mousePos);

		if(hitTest.SubItem?.Text == "Send")
		{
			if(hitTest.Item?.Tag is not Neighbor n) return;

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
					lvSend.Invoke(() => lvSend.Refresh());
				}
				else
				{
					lvSend.Refresh();
				}
			});
			return;
		}

	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		lvSend.Refresh();
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
