using System.Collections;
using System.ComponentModel;
using System.Xml.Linq;

namespace ACgifts;




class LVsort:IComparer
{
	public const int AC_GAME_NAME = 0;
	public const int FORUM_NAME = 1;
	public const int LIST_ORDER = 2;

	public bool IsSend { get; set; }
	public int SortType { get; set; }

	private readonly CaseInsensitiveComparer ciCompare;

	public LVsort(bool isSend)
	{
		IsSend = isSend;
		SortType = AC_GAME_NAME;
		ciCompare = new CaseInsensitiveComparer();
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



	public int SortNeighbor(Neighbor x, Neighbor y)
	{
		switch(SortType)
		{
			case AC_GAME_NAME:
				if(IsSend) return ciCompare.Compare(x.NameSend, y.NameSend);
				return ciCompare.Compare(x.NameRecv, y.NameRecv);

			case FORUM_NAME:
				return Comparer.Default.Compare(x.Name, y.Name);

			case LIST_ORDER:
				return Comparer.Default.Compare(x.Order, y.Order);
		}
		return 0;
	}


}
