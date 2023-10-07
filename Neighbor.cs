using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ACgifts;


[Serializable]
internal class Neighbor
{
	public string Name { get; set; }
	public string Group { get; set; }
	public DateTime Added { get; set; }


	public string NameSend { get; set; }
	public string IdSend { get; set; }
	public int CntSend { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? LastSend { get; set; }


	public string NameRecv { get; set; }
	public string IdRecv { get; set; }
	public int CntRecv { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? LastRecv { get; set; }



	[JsonIgnore]
	[Browsable(false)]
	public int Order { get; set; }
	[JsonIgnore]
	[Browsable(false)]
	public bool RecvThisSess { get; private set; } = false;
	[JsonIgnore]
	[Browsable(false)]
	public bool SendThisSess { get; private set; } = false;




	public Neighbor()
	{
		Order = 0;
		Name = "";
		NameSend = "";
		NameRecv = "";
		IdSend = "";
		IdRecv = "";
		Group = "";
		Added = DateTime.Now;
		LastSend = null;
		LastRecv = null;
		CntSend = 0;
		CntRecv = 0;
	}

	public Neighbor(int order, string name, string nameSend, string nameRecv, string idSend, string idRecv, string group, DateTime added, DateTime? lastSend, DateTime? lastRecv, int cntSend, int cntRecv)
	{
		Order = order;
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
		LastRecv = DateTime.Now;
		RecvThisSess = true;
	}
	public void AddSend()
	{
		CntSend++;
		LastSend = DateTime.Now;
		SendThisSess = true;
	}

}
