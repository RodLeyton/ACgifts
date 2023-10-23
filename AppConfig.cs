using System.Text.Json.Serialization;

namespace ACgifts;

public class ColumnConfig
{
	public int Order { get; set; }
	public int Width { get; set; }

	public ColumnConfig(int order, int width)
	{
		Order = order;
		Width = width;
	}
}



[Serializable]
internal class AppConfig
{
	public string InstalledVersion { get; set; }
	public string MainFormGeo { get; set; }
	public int SplitDistance { get; set; }
	public int SortOrder { get; set; }

	public Dictionary<LvExMainColumns, ColumnConfig> RecvCols { get; set; }
	public Dictionary<LvExMainColumns, ColumnConfig> SendCols { get; set; }




	[JsonIgnore]
	public bool IsDefaults { get; set; }



	public AppConfig() 
	{
		// Setup defaults
		IsDefaults = true;
		InstalledVersion = "";
		MainFormGeo = "";
		SplitDistance = -1;
		SortOrder = (int)LvExMainSortTypes.GAME_NAME;


		RecvCols = new()
		{
			{ LvExMainColumns.None, new(0, 0) },
			{ LvExMainColumns.ForumName, new(1, 0) },
			{ LvExMainColumns.GameName, new(2, 130) },
			{ LvExMainColumns.Button, new(3, 60) },
			{ LvExMainColumns.Last, new(4, 80) },
			{ LvExMainColumns.LastDays, new(5, 0) },
			{ LvExMainColumns.LastHours, new(6, 0) },
			{ LvExMainColumns.Count, new(7, 60) },
			{ LvExMainColumns.Rate, new(8, 60) },
			{ LvExMainColumns.Added, new(9, 0) }
		};


		SendCols = new()
		{
			{ LvExMainColumns.None, new(0, 0) },
			{ LvExMainColumns.ForumName, new(1, 0) },
			{ LvExMainColumns.GameName, new(2, 130) },
			{ LvExMainColumns.Button, new(3, 60) },
			{ LvExMainColumns.Last, new(4, 80) },
			{ LvExMainColumns.LastDays, new(5, 0) },
			{ LvExMainColumns.LastHours, new(6, 0) },
			{ LvExMainColumns.Count, new(7, 60) },
			{ LvExMainColumns.Rate, new(8, 60) },
			{ LvExMainColumns.Added, new(9, 0) }
		};


	}
}
