using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ACgifts;


[Serializable]
internal class Neighbor
{
	private const int TODAY_HOURS = 12;


	public string Name { get; set; } = "";
	public string Group { get; set; } = "";
	public DateTime Added { get; set; } = DateTime.Now;


	public string NameSend { get; set; } = "";
	public string IdSend { get; set; } = "";
	public int CntSend { get; set; } = 0;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? LastSend { get; set; } = null;


	public string NameRecv { get; set; } = "";
	public string IdRecv { get; set; } = "";
	public int CntRecv { get; set; } = 0;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? LastRecv { get; set; } = null;



	[JsonIgnore]
	[Browsable(false)]
	public int Order { get; set; } = 0;

	[JsonIgnore]
	[Browsable(false)]
	public bool RecvThisSess { get; private set; } = false;

	[JsonIgnore]
	[Browsable(false)]
	public bool SendThisSess { get; private set; } = false;

	[JsonIgnore]
	[Browsable(false)]
	public DateTime? PrevSend { get; set; } = null;

	[JsonIgnore]
	[Browsable(false)]
	public DateTime? PrevRecv { get; set; } = null;


	[JsonIgnore]
	[Browsable(false)]
	public bool HasSendToday
	{
		get {
			if(LastSend is null) return false;
			return SendThisSess || (DateTime.Now - (DateTime)LastSend).TotalHours < TODAY_HOURS; 
		}
	}

	[JsonIgnore]
	[Browsable(false)]
	public bool HasRecvToday
	{
		get
		{
			if(LastRecv is null) return false;
			return RecvThisSess || (DateTime.Now - (DateTime)LastRecv).TotalHours < TODAY_HOURS; 
		}
	}


	public Neighbor() { }

	public Neighbor(string name, string nameSend, string nameRecv, string idSend, string idRecv, string group, DateTime added, DateTime? lastSend, DateTime? lastRecv, int cntSend, int cntRecv)
	{
		Name = name;
		NameSend = nameSend;
		NameRecv = nameRecv;
		IdSend = idSend;
		IdRecv = idRecv;
		Group = group;
		Added = added;
		LastSend = lastSend;
		LastRecv = lastRecv;
		CntSend = cntSend;
		CntRecv = cntRecv;
	}


	public void AddRecv()
	{
		CntRecv++;
		PrevRecv = LastRecv;
		LastRecv = DateTime.Now;
		RecvThisSess = true;
	}
	public void AddSend()
	{
		CntSend++;
		PrevSend = LastSend;
		LastSend = DateTime.Now;
		SendThisSess = true;
	}

	public bool UndoSend()
	{
		if(!SendThisSess) return false;
		if(PrevSend is null) return false;
		if(CntSend < 1) return false;

		LastSend = PrevSend;
		PrevSend = null;
		SendThisSess = false;
		CntSend--;
		return true;
	}
	public bool UndoRecv()
	{
		if(!RecvThisSess) return false;
		if(PrevRecv is null) return false;
		if(CntRecv < 1) return false;

		LastRecv = PrevRecv;
		PrevRecv = null;
		RecvThisSess = false;
		CntRecv--;
		return true;
	}


	public override string ToString() => $"ACgifts.Neighbor; {Name}";
}
