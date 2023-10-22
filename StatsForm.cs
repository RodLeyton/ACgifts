using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACgifts;
internal partial class StatsForm:Form
{
	private readonly Data data;
	class GrpData
	{
		public int CntDays { get; set; } = 0;
		public int CntMembers { get; set; } = 0;
		public int CntSend { get; set; } = 0;
		public int CntRecv { get; set; } = 0;
	}


	public StatsForm(Data data)
	{
		InitializeComponent();
		this.data = data;

		lvGroups.Columns.Add("Group", 100);
		lvGroups.Columns.Add("Neighbors", 100);
		lvGroups.Columns.Add("Cnt Recv", 100);
		lvGroups.Columns.Add("Recv %", 100);
		lvGroups.Columns.Add("Cnt Send", 100);
		lvGroups.Columns.Add("Send %", 100);
		//lvGroups.Columns.Add("", Program.appConfig.Col4width);
		//lvGroups.Columns.Add("", Program.appConfig.Col5width);

		lvNeighbors.Columns.Add("Name", 100);
		lvNeighbors.Columns.Add("Age days", 100);
		lvNeighbors.Columns.Add("Cnt Recv", 100);
		lvNeighbors.Columns.Add("Recv %", 100);
		lvNeighbors.Columns.Add("Cnt Send", 100);
		lvNeighbors.Columns.Add("Send %", 100);
		//lvGroups.Columns.Add("", Program.appConfig.Col4width);
		//lvGroups.Columns.Add("", Program.appConfig.Col5width);


		Dictionary<string, GrpData> grps = new()
		{
			{ "All", new() },
			{ "Unassigned", new() }
		};

		foreach(Neighbor n in data.neighbors)
		{
			string grpStr = n.Group?.Trim() ?? "";
			if(grpStr == "") grpStr = "Unassigned";
			if(!grps.ContainsKey(grpStr)) grps.Add(grpStr, new());

			grps["All"].CntDays += (int)(DateTime.Now - n.Added).TotalDays;
			grps["All"].CntMembers++;
			grps["All"].CntSend += n.CntSend;
			grps["All"].CntRecv += n.CntRecv;

			grps[grpStr].CntDays += (int)(DateTime.Now - n.Added).TotalDays;
			grps[grpStr].CntMembers++;
			grps[grpStr].CntSend += n.CntSend;
			grps[grpStr].CntRecv += n.CntRecv;
		}

		foreach(KeyValuePair<string, GrpData> kvp in grps)
		{
			GrpData grpData = kvp.Value;
			if(grpData.CntDays < 1) continue;

			ListViewItem lvi = new(kvp.Key);
			lvi.SubItems.Add($"{grpData.CntMembers}");
			lvi.SubItems.Add($"{grpData.CntRecv}");
			lvi.SubItems.Add($"{(double)grpData.CntRecv / grpData.CntDays:P0}");
			lvi.SubItems.Add($"{grpData.CntSend}");
			lvi.SubItems.Add($"{(double)grpData.CntSend / grpData.CntDays:P0}");
			lvGroups.Items.Add(lvi);
		}
		lvGroups.Items[0].Selected = true;
	}




	private void LvGroup_SelectedIndexChanged(object sender, EventArgs e)
	{
		lvNeighbors.BeginUpdate();
		lvNeighbors.Items.Clear();
		if(lvGroups.SelectedItems.Count < 1)
		{
			lvNeighbors.EndUpdate();
			return;
		}

		string selGroup = (string)lvGroups.SelectedItems[0].Text;


		foreach(Neighbor n in data.neighbors)
		{
			if(selGroup == "Unassigned")
			{
				if(n.Group.Trim() != "") continue;
			}
			else if(selGroup != "All" && selGroup != n.Group) continue;

			int age = (int)(DateTime.Now - n.Added).TotalDays;
			ListViewItem lvi = new(n.Name);
			lvi.SubItems.Add($"{age}");
			lvi.SubItems.Add($"{n.CntRecv}");
			lvi.SubItems.Add($"{(double)n.CntRecv / age:P0}");
			lvi.SubItems.Add($"{n.CntSend}");
			lvi.SubItems.Add($"{(double)n.CntSend / age:P0}");
			lvNeighbors.Items.Add(lvi);
		}
		lvNeighbors.EndUpdate();
	}
}
