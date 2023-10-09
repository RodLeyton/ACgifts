namespace ACgifts;


[Serializable]
internal class AppConfig
{
	public bool IsDefaults { get; set; } = true;
	public int SortOrder { get; set; } = 0;
	public int Col0width { get; set; } = 130;
	public int Col1width { get; set; } = 60;
	public int Col2width { get; set; } = 80;
	public int Col3width { get; set; } = 50;
	public int Col4width { get; set; } = 50;
	public int Col5width { get; set; } = 130;
	public string MainFormGeo { get; set; } = "";


	public AppConfig() { }

}
