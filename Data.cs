using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;

namespace ACgifts;
internal class Data
{
	public BindingList<Neighbor> neighbors;

	private bool dataBackupDone = false;
	private readonly string DATA_FILE, DATA_FILE_NO_EXT;

	public Data() 
	{
		neighbors = new();

		DATA_FILE_NO_EXT = Program.GetDataDir() + "neighbors";
		DATA_FILE = DATA_FILE_NO_EXT + ".dat";
	}



	public void Load()
	{
		int cnt = 0, fail = 0;
		Program.Log("Data.Load","Loading neighbors from datafile.");

		neighbors.Clear();
		string dataFile = Program.GetDataDir() + "neighbors.dat";
		if(!File.Exists(dataFile))
		{
			Program.Log("Data.Load", "Datafile does not exist.");
			return;
		}

		StreamReader sr = new(dataFile);
		while(!sr.EndOfStream)
		{
			string? line = "";
			try
			{
				cnt++;
				line = sr.ReadLine();
				Neighbor? n = null;
				if(line != null) n = (Neighbor?)JsonSerializer.Deserialize(line, typeof(Neighbor));

				if(n != null) neighbors.Add(n);
				else
				{
					Program.Log("Data.Load", $"Line {cnt} failed parse: {line}");
					fail++;
				}
			}
			catch(Exception ex)
			{
				fail++;
				Program.Log("Data.Load",$"Exception caught during processing of line {cnt} line={line}");
				Program.Log("Data.Load", ex);
			}
		}
		sr.Close();
		if(fail > 0)
		{
			MessageBox.Show($"Not all data was imported, {fail}/{cnt} failed. Contact the developer for assistance.");
			Program.Log("Data.Load", $"***  Load complete. {fail}/{cnt} failed.");
		}
		Program.Log("Data.Load", $"{neighbors.Count} loaded.");
		ReOrder();
	}
	public void Save()
	{
		if(neighbors.Count == 0)
		{
			Program.Log("Data.Save", "Save Aborted, neighbors is empty.");
			return;
		}

		Program.Log("Data.Save", "Saving datafile.");

		if(!dataBackupDone)
		{
			Program.Log("Data.Save", "Rotating backup data files");
			try
			{
				for(int i = 30; i > 0; i--)
				{
					if(File.Exists($"{DATA_FILE_NO_EXT}.bak{i}.dat"))
						File.Delete($"{DATA_FILE_NO_EXT}.bak{i}.dat");
					if(File.Exists($"{DATA_FILE_NO_EXT}.bak{i-1}.dat"))
						File.Move($"{DATA_FILE_NO_EXT}.bak{i-1}.dat", $"{DATA_FILE_NO_EXT}.bak{i}.dat");
				}
				if(File.Exists(DATA_FILE))
					File.Move(DATA_FILE, $"{DATA_FILE_NO_EXT}.bak1.dat");

				Program.Log($"Data.Save", "Rotated backup files successfully.");
			}
			catch (Exception ex)
			{
				Program.Log($"Data.Save", "Exception moving backup data files");
				Program.Log("Data.Save", ex);
			}
			dataBackupDone = true;
		}

		StreamWriter? sFile = null;
		try
		{
			sFile = new StreamWriter(DATA_FILE, append: false) { AutoFlush = false };

			foreach(Neighbor n in neighbors)
				sFile.WriteLine(JsonSerializer.Serialize(n));

			Program.Log($"Data.Save", "Save completed successfully");
		}
		catch (Exception ex)
		{
			Program.Log("Data.Save", "Exception saving current data file");
			Program.Log("Data.Save", ex);
		}
		finally
		{
			sFile?.Flush();
			sFile?.Close();
		}
	}

	public void Backup()
	{
		if(!File.Exists(DATA_FILE) && neighbors.Count == 0)
		{
			Program.Log("Data.Backup", "Backup cancelled, no data exists");
			MessageBox.Show("We do not have any data to back up.", "ACgifts no data");
			return;
		}

		SaveFileDialog sfd = new()
		{
			Filter = "Data File|*.dat",
			FileName = $"ACgifts_backup_{DateTime.Now:yyyy-MM-dd}.dat"
		};
		if(sfd.ShowDialog() != DialogResult.OK) return;

		Program.Log("Data.Backup", $"Backup to '{sfd.FileName}' started");

		if(File.Exists(sfd.FileName))
		{
			Program.Log("Data.Backup", "File exists, deleting");
			File.Delete(sfd.FileName);
		}
		Save();

		File.Copy(DATA_FILE, sfd.FileName);
		Program.Log("Data.Backup", "Backup complete");
	}
	public void Restore()
	{
		OpenFileDialog ofd = new()
		{
			Filter = "Data File|*.dat"
		};
		if(ofd.ShowDialog() == DialogResult.OK)
		{
			Program.Log("Data.Restore", $"Restore started, src='{ofd.FileName}'");
			if(!File.Exists(ofd.FileName))
			{
				MessageBox.Show($"Couldn't access {ofd.FileName}, please check permissions.");
				Program.Log($"Data.Restore", $"'{ofd.FileName}' does not exist.");
				return;
			}

			int cnt = 0, fail = 0;
			List<Neighbor> temp = new();
			StreamReader sr = new(ofd.FileName);
			while(!sr.EndOfStream)
			{
				string? line = "";
				try
				{
					cnt++;
					line = sr.ReadLine();
					Neighbor? n = null;
					if(line != null) n = (Neighbor?)JsonSerializer.Deserialize(line, typeof(Neighbor));

					if(n != null) temp.Add(n);
					else
					{
						Program.Log("Data.Restore", $"*** Line {cnt} failed parse: {line}");
						fail++;
					}
				}
				catch(Exception ex)
				{
					fail++;
					Program.Log("Data.Restore", $"Exception caught on line {cnt} => {line}");
					Program.Log("Data.Restore", ex);
					break;
				}
			}
			sr.Close();
			if(fail > 0)
			{
				MessageBox.Show($"Backup file is corrupt, {fail}/{cnt} failed.\r\nAll data from restore was rejected, current data has been maintained.\r\nContact the developer for assistance.");
				Program.Log("Data.Restore", $"Backup file '{ofd.FileName}' is corrupt, {fail}/{cnt} failed. Restore aborted.");
				return;
			}

			string backup = Path.Combine(Program.GetDataDir(), $"ACgifts_backup_pre_restore_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.dat");

			Program.Log("Data.Restore", $"Backup file parsed successfully. {cnt} records found.");

			if(neighbors.Count > 0 && File.Exists(DATA_FILE))
			{
				Program.Log("Data.Restore", $"Backing up current data file to '{backup}'");
				Save();
				File.Move(DATA_FILE, backup);
			}

			neighbors.Clear();
			foreach(Neighbor n in temp) neighbors.Add(n);
			temp.Clear();
			MessageBox.Show($"Restored all {cnt} neighbors successfully!");


			Program.Log("Data.Restore", "Saving new data file.");
			Save();
			Program.Log("Data.Restore", "Restore Complete.");
		}
	}
	public void Import(string file)
	{
		Program.Log($"Data.Import", $"Importing data from '{file}'");

		if(neighbors.Count == 0)
		{
			Program.Log($"Data.Import", "Skiped clear data prompt since neighbors.Count == 0");
		}
		else
		{
			DialogResult res = MessageBox.Show("Do you want to clear existing data?", "Delete data?", MessageBoxButtons.YesNoCancel);
			if(res == DialogResult.Cancel)
			{
				Program.Log($"Data.Import", "Import cancelled by user.");
				return;
			}

			if(res == DialogResult.Yes)
			{
				neighbors.Clear();
				Program.Log("Data.Import", "User requested to clear existing data.");
			}
			else
			{
				Program.Log("Data.Import", $"Retaining {neighbors.Count} existing records.");

				string backup = Path.Combine(Program.GetDataDir(), $"ACgifts_backup_pre_import_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.dat");
				Program.Log("Data.Import", $"Backing up current data file to '{backup}'");

				Save();
				File.Move(DATA_FILE, backup);
			}
		}

		using TextFieldParser parser = new(file);
		parser.TextFieldType = FieldType.Delimited;
		parser.HasFieldsEnclosedInQuotes = true;
		parser.SetDelimiters(",");
		parser.ReadLine();
		int cnt = 0, fail = 0, parseError = 0;

		while(!parser.EndOfData)
		{
			cnt++;
			//Process row
			string[]? fields;
			try
			{
				fields = parser.ReadFields();
			}
			catch(Exception ex)
			{
				fail++;
				Program.Log($"Data.Import", $"Exception caught for parser.ReadFields() on line:{parser.LineNumber}  src={parser.ErrorLine}");
				Program.Log("Data.Import", ex);
				continue;
			}
			if(fields is null)
			{
				fail++;
				Program.Log("Data.Import", $"TextFieldParser Couldn't Parse line:{parser.LineNumber}  src={parser.ErrorLine}");
				continue;
			}
			if(fields.Length != 11)
			{
				fail++;
				Program.Log($"Data.Import", $"Only {fields.Length} fields found on line:{parser.LineNumber}  src={parser.ErrorLine}");
				continue;
			}

			//Name	Group	NameSend	CodeSend	LastSend	CntSend	NameRecv	CodeRecv	LastRecv	CntRecv	Added
			string name = fields[0].Trim();
			string group = fields[1].Trim();
			string nameSend = fields[2].Trim();
			string codeSend = fields[3].Trim();
			string sLastSend = fields[4].Trim();
			string sCntSent = fields[5].Trim();
			string nameRecv = fields[6].Trim();
			string codeRecv = fields[7].Trim();
			string sLastRecv = fields[8].Trim();
			string sCntRecv = fields[9].Trim();
			string sAdded = fields[10].Trim();

			if(!int.TryParse(sCntSent, out int cntSent)) cntSent = 0;
			if(!int.TryParse(sCntRecv, out int cntRecv)) cntRecv = 0;

			DateTime? lastSend = null, lastRecv = null;
			DateTime added = DateTime.Now;

			if(sLastSend != "")
			{
				if(DateTime.TryParse(sLastSend, null, DateTimeStyles.None, out DateTime dtS)) lastSend = dtS;
				else {
					parseError++;
					Program.Log("Data.Import", $"Line {cnt}: LastSend date format was incorrect. Val={sLastSend}");
				}
			}

			if(sLastRecv != "")
			{
				if(DateTime.TryParse(sLastRecv, null, DateTimeStyles.None, out DateTime dtR)) lastRecv = dtR;
				else {
					parseError++;
					Program.Log("Data.Import", $"Line {cnt}: LastRecv date format was incorrect. Val={sLastRecv}");
				}
			}

			if(sAdded != "")
			{
				if(DateTime.TryParse(sAdded, null, DateTimeStyles.None, out DateTime dtA)) added = dtA;
				else {
					parseError++;
					Program.Log("Data.Import", $"Line {cnt}: 'Added' date format was incorrect, set to DateTime.Now   Val={sAdded}");
				}
			}

			neighbors.Add(new(name, nameSend, nameRecv, codeSend, codeRecv, group, added, lastSend, lastRecv, cntSent, cntRecv));


		}
		ReOrder();
		Save();

		if(fail > 0)
		{
			MessageBox.Show($"Import file is corrupt, {fail}/{cnt} lines failed.\r\nPlease check the file format.");
			Program.Log("Data.Import", $"*** File '{file}' is malformed, {fail}/{cnt} lines failed.");
			return;
		}

		if(parseError > 0)
		{
			MessageBox.Show($"Import file had some datetime errors, but all lines were imported.\r\nPlease check the log for details.");
			Program.Log("Data.Import", $"*** File '{file}' had parse errors, {fail}/{cnt} lines failed.");
			return;
		}


		MessageBox.Show($"Import Added {cnt} neighbors successfully!");
		Program.Log("Data.Import", $"Added {cnt} neighbors successfully!");
	}
	public void Export()
	{
		if(!File.Exists(DATA_FILE) && neighbors.Count == 0)
		{
			Program.Log("Data.Export", "Export cancelled, no data exists");
			MessageBox.Show("We do not have any data to Export.", "ACgifts no data");
			return;
		}

		SaveFileDialog sfd = new()
		{
			Filter = "CSV File|*.csv",
			FileName = $"ACgifts_export_{DateTime.Now:yyyy-MM-dd}.csv"
		};
		if(sfd.ShowDialog() != DialogResult.OK) return;

		if(File.Exists(sfd.FileName))
		{
			Program.Log("Data.Export", "File exists, deleting");
			File.Delete(sfd.FileName);
		}



		StreamWriter? sFile = null;
		try
		{
			sFile = new StreamWriter(sfd.FileName, append: false) { AutoFlush = false };
			sFile.WriteLine("Name,Group,NameSend,CodeSend,LastSend,CntSend,NameRecv,CodeRecv,LastRecv,CntRecv,Added");

			foreach(Neighbor n in neighbors)
			{
				//Name	Group	NameSend	CodeSend	LastSend	CntSend	NameRecv	CodeRecv	LastRecv	CntRecv	Added

				sFile.Write(Utils.EscapeCSV(n.Name));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.Group));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.NameSend));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.IdSend));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.LastSend));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.CntSend));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.NameRecv));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.IdRecv));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.LastRecv));
				sFile.Write(',');
				sFile.Write(Utils.EscapeCSV(n.CntRecv));
				sFile.Write(',');
				sFile.WriteLine(Utils.EscapeCSV(n.Added));
			}
			Program.Log("Data.Export", "Export complete");
		}
		catch(Exception ex)
		{
			Program.Log("Data.Export", "Exception exporting current data to csv");
			Program.Log("Data.Export", ex);
		}
		finally
		{
			sFile?.Flush();
			sFile?.Close();
		}



	}
	public void ReOrder()
	{
		int ord = 0;
		foreach(Neighbor n in neighbors)
			n.Order = ord++;
	}

	public void CreateDemoData()
	{
		if(neighbors.Count > 0) return;

		// No fuel ring
		neighbors.Add(new Neighbor("Crystal", "Crystal", "Crystal", "yqtzwfyif", "yqtzwfyif", "",
			DateTime.Today.AddDays(-3), DateTime.Now.AddMinutes(-10), null, 3, 0));

		neighbors.Add(new Neighbor("Trisha @ home", "Trisha", "Trisha", "8cmn4tjxe", "8cmn4tjxe", "",
			DateTime.Today.AddDays(-50), DateTime.Now.AddMinutes(-10), DateTime.Now.AddHours(-14.1), 48, 55));

		neighbors.Add(new Neighbor("Android", "Android", "Android", "8cmn4tjxe", "8cmn4tjxe", "",
			DateTime.Today.AddDays(-40), DateTime.Now.AddMinutes(-10), DateTime.Now.AddHours(-2.8), 40, 36));


		// ZF3 ring
		neighbors.Add(new Neighbor("Dryden & Dale", "Dryden", "ZF3 Dryden", "2knsor285", "hu0njijw6", "ZF3",
			DateTime.Today.AddDays(-70), DateTime.Now.AddMinutes(-75), DateTime.Now.AddMinutes(-5), 64, 75));

		neighbors.Add(new Neighbor("Bad Neighbor", "Bad Neighbor", "ZF3 Bad Neighbor", "x24btm7ei", "cwzgrxzn8", "ZF3",
			DateTime.Today.AddDays(-70), DateTime.Now.AddMinutes(-10), DateTime.Now.AddHours(-26.3), 74, 25));

		neighbors.Add(new Neighbor("Fontana", "Fontana", "ZF3 Fontana", "guo4x2rna", "23ecb9cwd", "ZF3",
			DateTime.Today.AddDays(-70), DateTime.Now.AddMinutes(-10), DateTime.Now.AddHours(-4.5), 67, 67));

		neighbors.Add(new Neighbor("Genie, comma", "Genie", "ZF3 Genie", "ryv0b8z7h", "ptt6y8h68", "ZF3",
			DateTime.Today.AddDays(-70), DateTime.Now.AddMinutes(-10), DateTime.Now.AddHours(-4.1), 78, 70));

		neighbors.Add(new Neighbor("New guy", "New guy", "ZF3 New guy", "sss0b8z7h", "efd6y8h68", "ZF3",
			DateTime.Today.AddDays(-2), null, null, 0, 0));


		// ZF5 ring
		neighbors.Add(new Neighbor("Dryden & Dale", "Dryden", "ZF5 Dryden", "2knsor285", "xx0njijw6", "ZF5",
			DateTime.Today.AddDays(-25), DateTime.Now.AddHours(-22), DateTime.Now.AddHours(-5), 25, 22));

		neighbors.Add(new Neighbor("Bad Neighbor", "Bad Neighbor", "ZF5 Bad Neighbor", "x24btm7ei", "lklgrxzn8", "ZF5",
			DateTime.Today.AddDays(-25), DateTime.Now.AddHours(-22), DateTime.Now.AddHours(-35.2), 25, 6));

		neighbors.Add(new Neighbor("Genie, comma", "Genie", "ZF5 Genie", "rrv0b8z7h", "pya6u8h68", "ZF5",
			DateTime.Today.AddDays(-25), DateTime.Now.AddHours(-22), DateTime.Now.AddHours(-25), 24, 24));

		neighbors.Add(new Neighbor("Orlosky+\"quotes\"", "Orlosky", "ZF5 Orlosky", "j4k0tstox", "z1fnczsvr", "ZF5",
			DateTime.Today.AddDays(-25), DateTime.Now.AddHours(-22), DateTime.Now.AddHours(-45), 24, 20));

		neighbors.Add(new Neighbor("Penelope", "Penelope", "ZF5 Penelope", "odl1vrdyc", "o7hyr8qvb", "ZF5",
			DateTime.Today.AddDays(-25), DateTime.Now.AddHours(-22), DateTime.Now.AddHours(-15.1), 26, 27));

		neighbors.Add(new Neighbor("\"Turtle\"", "Turtle", "ZF5 Turtle", "5kuqxva1a", "tw2l7lvyr", "ZF3",
			DateTime.Today.AddDays(-25), DateTime.Now.AddHours(-22), DateTime.Now.AddMinutes(-45), 25, 25));

	}
}
