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
	public bool IsDefaults { get; set; }
	public string InstalledVersion { get; set; }
	public string MainFormGeo { get; set; }
	public int SplitDistance { get; set; }
	public int SortOrder { get; set; }

	public Dictionary<LvExMainColumns, ColumnConfig> RecvCols { get; set; }
	public Dictionary<LvExMainColumns, ColumnConfig> SendCols { get; set; }






	public AppConfig() 
	{
		InstalledVersion = "";
		MainFormGeo = "";
		RecvCols = new();
		SendCols = new();
		Reset();
	}


	public void Reset()
	{
		// Setup defaults
		IsDefaults = true;
		SplitDistance = -1;
		SortOrder = (int)LvExMainSortTypes.GAME_NAME;

		RecvCols.Clear();
		RecvCols.Add(LvExMainColumns.None, new(0, 0));
		RecvCols.Add(LvExMainColumns.ForumName, new(1, 0));
		RecvCols.Add(LvExMainColumns.GameName, new(2, 130));
		RecvCols.Add(LvExMainColumns.Button, new(3, 60));
		RecvCols.Add(LvExMainColumns.Last, new(4, 80));
		RecvCols.Add(LvExMainColumns.LastDays, new(5, 0));
		RecvCols.Add(LvExMainColumns.LastHours, new(6, 0));
		RecvCols.Add(LvExMainColumns.Count, new(7, 60));
		RecvCols.Add(LvExMainColumns.Rate, new(8, 60));
		RecvCols.Add(LvExMainColumns.Added, new(9, 0));


		SendCols.Clear();
		SendCols.Add(LvExMainColumns.None, new(0, 0));
		SendCols.Add(LvExMainColumns.ForumName, new(1, 0));
		SendCols.Add(LvExMainColumns.GameName, new(2, 130));
		SendCols.Add(LvExMainColumns.Button, new(3, 60));
		SendCols.Add(LvExMainColumns.Last, new(4, 80));
		SendCols.Add(LvExMainColumns.LastDays, new(5, 0));
		SendCols.Add(LvExMainColumns.LastHours, new(6, 0));
		SendCols.Add(LvExMainColumns.Count, new(7, 0));
		SendCols.Add(LvExMainColumns.Rate, new(8, 0));
		SendCols.Add(LvExMainColumns.Added, new(9, 0));

	}
}
