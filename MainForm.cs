
namespace ACgifts;

public partial class MainForm:Form
{
	private readonly Data data;
	private readonly LVsort lvSendSort, lvRecvSort;

	//todo add sent/recv cnt per group and total today
	//todo reject second click on send/recv if within x sec

	public MainForm()
	{
		InitializeComponent();
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
		cbSortOrder.Items.Add(new KeyValuePair<int, string>(LVsort.AC_GAME_NAME, "Game name"));
		cbSortOrder.Items.Add(new KeyValuePair<int, string>(LVsort.FORUM_NAME, "Forum name"));
		cbSortOrder.Items.Add(new KeyValuePair<int, string>(LVsort.LIST_ORDER, "List Order"));
		cbSortOrder.SelectedIndex = Properties.Settings.Default.SortOrder;
		lvSendSort.SortType = cbSortOrder.SelectedIndex;
		lvRecvSort.SortType = cbSortOrder.SelectedIndex;

		WindowRestore.GeometryFromString(Properties.Settings.Default.MainFormGeo, this);
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
	private void MainForm_Resize(object sender, EventArgs e)
	{
		lvRecv.Left = lbGroups.Right + 10;
		cbSortOrder.Left = lvRecv.Left;
		lvRecv.Width = (this.ClientSize.Width - lvRecv.Left - 20) / 2 - 10;
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
		}
		lvSend.Refresh();
	}
	private void ButEdit_Click(object sender, EventArgs e)
	{
		new EditForm(data).ShowDialog(this);
		UpdateGroupsLV();
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
		lvRecv.BeginUpdate();
		lvRecv.Items.Clear();

		lvSend.BeginUpdate();
		lvSend.Items.Clear();

		foreach(Neighbor n in data.neighbors)
		{
			if("" + lbGroups.SelectedItem == "unassigned")
			{
				if(n.Group?.Trim() != "") continue;
			}
			else if("" + lbGroups.SelectedItem != n.Group) continue;


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
	}




	private void LvRecv_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvRecv.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvRecv.HitTest(mousePos);

		if(hitTest.SubItem?.Text == "Recv")
		{
			if(hitTest.Item?.Tag is Neighbor n) n.AddRecv();
			lvRecv.Refresh();
			return;
		}
	}
	private void LvSend_MouseClick(object sender, MouseEventArgs e)
	{
		Point mousePos = lvSend.PointToClient(Control.MousePosition);
		ListViewHitTestInfo hitTest = lvSend.HitTest(mousePos);

		if(hitTest.SubItem?.Text == "Send")
		{
			if(hitTest.Item?.Tag is Neighbor n) n.AddSend();
			lvSend.Refresh();
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
