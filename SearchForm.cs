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

partial class SearchForm:Form
{
	private List<Neighbor> nList;
	private Data data;


	public static List<Neighbor> DoSearch(Data data, IWin32Window ctx)
	{
		List<Neighbor> res = new();
		new SearchForm(data, res).ShowDialog(ctx);
		return res;
	}


	public SearchForm(Data data, List<Neighbor> nList)
	{
		InitializeComponent();
		this.data = data;
		this.nList = nList;
		Search(null, null);
	}

	private void ButtonOK_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ButCancel_Click(object sender, EventArgs e)
	{
		nList.Clear();
		Close();
	}

	private void Search(object? sender, EventArgs? e)
	{
		nList.Clear();

		foreach(Neighbor n in data.neighbors)
		{
			if(tbForumName.Text.Length > 0)
			{
				string str = tbForumName.Text.ToLower();
				if(!n.Name.ToLower().Contains(str)) continue;
			}

			if(tbGameName.Text.Length > 0)
			{
				string str = tbGameName.Text.ToLower();
				if(!n.NameSend.ToLower().Contains(str) && !n.NameRecv.ToLower().Contains(str)) 
					continue; 
			}

			if(tbGroup.Text.Length > 0)
			{
				string str = tbGroup.Text.ToLower();
				if(!n.Group.ToLower().Contains(str)) continue;
			}

			if(tbCode.Text.Length > 0)
			{
				string str = tbCode.Text.ToLower();
				if(!n.IdRecv.ToLower().Contains(str) && !n.IdSend.ToLower().Contains(str)) 
					continue;
			}

			nList.Add(n);
		}


		labRes.Text = $"{nList.Count} neighbors found";
	}


}
