using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace ACgifts;


public enum SortOrderTypes:int
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


class LVsort:IComparer
{
	public bool IsSend { get; set; }
	public SortOrderTypes SortType { get; set; }

	public LVsort(bool isSend)
	{
		IsSend = isSend;
		SortType = SortOrderTypes.GAME_NAME;
	}


	/// <summary>
	/// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
	/// </summary>
	/// <param name="x">First object to be compared</param>
	/// <param name="y">Second object to be compared</param>
	/// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
	public int Compare(object? x, object? y)
	{
		if(x is ListViewItem lviX && y is ListViewItem lviY)
			if(lviX.Tag is Neighbor xn && lviY.Tag is Neighbor yn)
				return SortNeighbor(xn, yn);

		return Comparer.Default.Compare(x, y);
	}



	private int SortNeighbor(Neighbor x, Neighbor y)
	{
		switch(SortType)
		{
			case SortOrderTypes.GAME_NAME:
				if(IsSend) return Comparer.Default.Compare(x.NameSend, y.NameSend);
				return Comparer.Default.Compare(x.NameRecv, y.NameRecv);

			case SortOrderTypes.FORUM_NAME:
				return Comparer.Default.Compare(x.Name, y.Name);

			case SortOrderTypes.LIST_ORDER:
				return Comparer.Default.Compare(x.Order, y.Order);

			case SortOrderTypes.LAST_TIME:
				if(IsSend) return Comparer.Default.Compare(x.LastSend, y.LastSend);
				return Comparer.Default.Compare(x.LastRecv, y.LastRecv);

			case SortOrderTypes.RECV_SPECIAL:
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

			case SortOrderTypes.RELIABILITY:
				return Comparer.Default.Compare(GetRate(x), GetRate(y));

			case SortOrderTypes.ADDED:
				return Comparer.Default.Compare(x.Added, y.Added);
		}
		throw new NotImplementedException($"Sort order type {SortType} => '{Utils.GetEnumDescription(SortType)}' was not handled!!!");
	}

	private double GetRate(Neighbor n)
	{
		double daysSinceAdded = (DateTime.Now - n.Added).TotalDays;
		return (IsSend ? n.CntSend : n.CntRecv) / Math.Max(1, daysSinceAdded);
	}


}
