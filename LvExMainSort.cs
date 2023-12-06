using System.Collections;
using System.ComponentModel;

namespace ACgifts;


public enum LvExMainSortTypes:int
{
	[Description("Game Name")]
	GAME_NAME = 0,
	[Description("Forum Name")]
	FORUM_NAME = 1,
	[Description("List Order")]
	LIST_ORDER = 2,
	[Description("Last Send/Recv")]
	LAST_TIME = 3,
	[Description("Recv Special")]
	RECV_SPECIAL = 4,
	[Description("Reliability")]
	RELIABILITY = 5,
	[Description("Date Added.")]
	ADDED = 6,
}


class LvExMainSort:IComparer
{
	public bool IsSend { get; set; }
	public LvExMainSortTypes SortType { get; set; }



	public LvExMainSort(bool isSend)
	{
		IsSend = isSend;
		SortType = LvExMainSortTypes.GAME_NAME;
	
	}


	/// <summary>
	/// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
	/// </summary>
	/// <param name="x">First object to be compared</param>
	/// <param name="y">Second object to be compared</param>
	/// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
	public int Compare(object? x, object? y)
	{
		if(x is LviNeighbor lviX && y is LviNeighbor lviY)
				return SortNeighbor(lviX.Neighbor, lviY.Neighbor);

		return Comparer.Default.Compare(x, y);
	}



	private int SortNeighbor(Neighbor x, Neighbor y)
	{
		switch(SortType)
		{
			case LvExMainSortTypes.GAME_NAME:
				if(IsSend) return AC_Sort(x.NameSend, y.NameSend);
				return AC_Sort(x.NameRecv, y.NameRecv);

			case LvExMainSortTypes.FORUM_NAME:
				return Comparer.Default.Compare(x.Name, y.Name);

			case LvExMainSortTypes.LIST_ORDER:
				return Comparer.Default.Compare(x.Order, y.Order);

			case LvExMainSortTypes.LAST_TIME:
				if(IsSend) return Comparer.Default.Compare(x.LastSend, y.LastSend);
				return Comparer.Default.Compare(x.LastRecv, y.LastRecv);

			case LvExMainSortTypes.RECV_SPECIAL:
				// Send is simply sorted by time
				if(IsSend) return Comparer.Default.Compare(x.LastSend, y.LastSend);

				// Recv today goes to bottom.
				if(x.HasRecvToday && y.HasRecvToday) return Comparer.Default.Compare(x.NameRecv, y.NameRecv);
				if(x.HasRecvToday) return 1;
				if(y.HasRecvToday) return -1;

				double xDays = 10, yDays = 10;
				if(x.LastRecv != null) xDays = (DateTime.Now - (DateTime)x.LastRecv).TotalDays;
				if(y.LastRecv != null) yDays = (DateTime.Now - (DateTime)y.LastRecv).TotalDays;

				if(xDays >= 2 && yDays >= 2) return Comparer.Default.Compare(x.NameRecv, y.NameRecv);
				if(xDays >= 2) return -1;
				if(yDays >= 2) return 1;

				return Comparer.Default.Compare(x.NameRecv, y.NameRecv);

			case LvExMainSortTypes.RELIABILITY:
				return Comparer.Default.Compare(GetRate(x), GetRate(y));

			case LvExMainSortTypes.ADDED:
				return Comparer.Default.Compare(x.Added, y.Added);
		}
		throw new NotImplementedException($"Sort order type {SortType} => '{Utils.GetEnumDescription(SortType)}' was not handled!!!");
	}

	private double GetRate(Neighbor n)
	{
		double daysSinceAdded = (DateTime.Now - n.Added).TotalDays;
		return (IsSend ? n.CntSend : n.CntRecv) / Math.Max(1, daysSinceAdded);
	}


	private int AC_Sort(string a, string b)
	{
		int pos = 0;

		while(true)
		{
			if(a.Length <= pos) return 1;
			if(b.Length <= pos) return -1;

			int ia = (int)(char)a[pos];
			int ib = (int)(char)b[pos];
			if(ia != ib) return ia < ib ? -1 : 1;
			pos++;
		}
	}


}
