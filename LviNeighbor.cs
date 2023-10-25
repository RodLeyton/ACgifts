namespace ACgifts;


internal class LviNeighbor:ListViewItem
{
	public Neighbor Neighbor { get; }


	// Shortcuts 
	public string ForumName => Neighbor.Name;
	public string NameSend => Neighbor.NameSend;
	public string NameRecv => Neighbor.NameRecv;
	public string GroupName => Neighbor.Group;
	public DateTime Added => Neighbor.Added;


	public int CntRecv => Neighbor.CntRecv;
	public int CntSend => Neighbor.CntSend;

	public DateTime? LastRecv => Neighbor.LastRecv;
	public DateTime? LastSend => Neighbor.LastSend;



	public bool RecvThisSess => Neighbor.RecvThisSess;
	public bool SendThisSess => Neighbor.SendThisSess;

	public void AddRecv() => Neighbor.AddRecv();
	public void AddSend() => Neighbor.AddSend();




	public LviNeighbor(Neighbor neighbor)
	{
		this.Neighbor = neighbor;
		UseItemStyleForSubItems = false;
	}
}
