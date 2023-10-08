using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic.Logging;

namespace ACgifts;
internal class Data
{
	public BindingList<Neighbor> neighbors;

	private const string FILENAME = "data/nieghbors";
	private const string FILEEXT = ".dat";
	private bool dataBackupDone = false;

	public Data() 
	{
		neighbors = new();
	}



	public void Load()
	{
		int cnt = 0, fail = 0;
		Program.Log("Data.Load()","Loading neighbors from datafile.");

		neighbors.Clear();
		if(!File.Exists(FILENAME + FILEEXT))
		{

			return;
		}

		StreamReader sr = new(FILENAME + FILEEXT);
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
					Program.Log("Data.Load()", $"Line {cnt} failed parse: {line}");
					fail++;
				}
			}
			catch(Exception ex)
			{
				fail++;
				Program.Log("Data.Load()",$"Exception caught during processing of line {cnt} line={line}");
				Program.Log("Data.Load()", ex);
			}
		}
		sr.Close();
		if(fail > 0)
		{
			MessageBox.Show($"Not all data was imported, {fail}/{cnt} failed. Contact the developer for assistance.");
			Program.Log("Data.Load()", $"***  Load complete. {fail}/{cnt} failed.");
		}
		Program.Log("Data.Load()", $"{neighbors.Count} loaded.");
	}

	public void Save()
	{
		if(neighbors.Count == 0)
		{
			Program.Log("Data.Save()", "Save Aborted, neighbors is empty.");
			return;
		}

		Program.Log("Data.Save()", "Saving datafile.");
		if(!dataBackupDone)
		{
			Program.Log("Data.Save()", "Rotating backup data files");
			try
			{
				for(int i = 10; i > 0; i--)
				{
					if(File.Exists(FILENAME + ".bak" + i + FILEEXT))
						File.Delete(FILENAME + ".bak" + i + FILEEXT);
					if(File.Exists(FILENAME + ".bak" + (i - 1) + FILEEXT))
						File.Move(FILENAME + ".bak" + (i - 1) + FILEEXT, FILENAME + ".bak" + i + FILEEXT);
				}
				if(File.Exists(FILENAME + FILEEXT))
					File.Move(FILENAME + FILEEXT, FILENAME + ".bak1" + FILEEXT);

				Program.Log($"Data.Save()", "Rotated backup files successfully.");
			}
			catch (Exception ex)
			{
				Program.Log($"Data.Save()", "Exception moving backup data files");
				Program.Log("Data.Save()", ex);
			}
			dataBackupDone = true;
		}

		StreamWriter? sFile = null;
		try
		{
			sFile = new StreamWriter(FILENAME + FILEEXT, append: false) { AutoFlush = false };

			foreach(Neighbor n in neighbors)
				sFile.WriteLine(JsonSerializer.Serialize(n));
		}
		catch (Exception ex)
		{
			Program.Log("Data.Save()", "Exception saving current data file");
			Program.Log("Data.Save()", ex);
		}
		finally
		{
			sFile?.Flush();
			sFile?.Close();
		}
	}

	public void Backup()
	{
		SaveFileDialog sfd = new()
		{
			Filter = "Data File|*.dat",
			FileName = $"ACgifts_backup_{DateTime.Now:yyyy-MM-dd}{FILEEXT}"
		};
		if(sfd.ShowDialog() != DialogResult.OK) return;

		if(File.Exists(sfd.FileName)) File.Delete(sfd.FileName);

		Save();
		File.Copy(FILENAME + FILEEXT, sfd.FileName);

	}
	public void Restore()
	{
		OpenFileDialog ofd = new()
		{
			Filter = "Data File|*.dat"
		};
		if(ofd.ShowDialog() == DialogResult.OK)
		{
			Program.Log("Data.Restore()", $"Restore started src='{ofd.FileName}'");
			if(!File.Exists(ofd.FileName))
			{
				MessageBox.Show($"Couldn't access {ofd.FileName}, please check permissions.");
				Program.Log($"Data.Restore()", $"'{ofd.FileName}' does not exist.");
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
						Program.Log("Data.Restore()", $"*** Line {cnt} failed parse: {line}");
						fail++;
					}
				}
				catch(Exception ex)
				{
					fail++;
					Program.Log("Data.Restore()", $"Exception caught on line {cnt} => {line}");
					Program.Log("Data.Restore()", ex);
					break;
				}
			}
			sr.Close();
			if(fail > 0)
			{
				MessageBox.Show($"Backup file is corrupt, {fail}/{cnt} failed.\r\nAll data from restore was rejected, current data has been maintained.\r\nContact the developer for assistance.");
				Program.Log("Data.Restore()", $"Backup file '{ofd.FileName}' is corrupt, {fail}/{cnt} failed. Restore aborted.");
				return;
			}

			string backup = $"ACgifts_backup_pre_restore_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{FILEEXT}";

			Program.Log("Data.Restore()", $"Backup file parsed successfully. {cnt} records found.");

			if(neighbors.Count > 0 && File.Exists(FILENAME + FILEEXT))
			{
				Program.Log("Data.Restore()", $"Backing up current data file to '{backup}'");
				Save();
				File.Move(FILENAME + FILEEXT, backup);
			}

			neighbors.Clear();
			foreach(Neighbor n in temp) neighbors.Add(n);
			temp.Clear();
			MessageBox.Show($"Restored all {cnt} neighbors successfully!");


			Program.Log("Data.Restore()", "Saving new data file.");
			Save();
			Program.Log("Data.Restore()", "Restore Complete.");
		}
	}
	public void Import(string file)
	{
		Program.Log($"Data.Import()", $"Importing data from '{file}'");

		if(neighbors.Count == 0)
		{
			Program.Log($"Data.Import()", "Skiped clear data prompt since neighbors.Count == 0");
		}
		else
		{
			DialogResult res = MessageBox.Show("Do you want to clear existing data?", "Delete data?", MessageBoxButtons.YesNoCancel);
			if(res == DialogResult.Cancel)
			{
				Program.Log($"Data.Import()", "Import cancelled by user.");
				return;
			}

			if(res == DialogResult.Yes)
			{
				neighbors.Clear();
				Program.Log("Data.Import()", "User requested to clear existing data.");
			}
			else
			{
				Program.Log("Data.Import()", $"Retaining {neighbors.Count} existing records.");

				string backup = $"ACgifts_backup_pre_import_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{FILEEXT}";

				Program.Log("Data.Import()", $"Backing up current data file to '{backup}'");
				Save();
				File.Move(FILENAME + FILEEXT, backup);
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
			//Process row
			string[]? fields;
			try
			{
				fields = parser.ReadFields();
			}
			catch(Exception ex)
			{
				fail++;
				Program.Log($"Data.Import()", $"Exception caught for parser.ReadFields() on line {cnt}");
				Program.Log("Data.Import()", ex);
				cnt++;
				continue;
			}
			if(fields is null)
			{
				fail++;
				Program.Log("Data.Import()", $"TextFieldParser Couldn't Parse line {cnt}");
				cnt++;
				continue;
			}
			if(fields.Length != 11)
			{
				fail++;
				Program.Log($"Data.Import()", $"Only {fields.Length} fields found on line {cnt}. Dropped: {string.Join(',', fields)}");
				cnt++;
				continue;
			}

			cnt++;
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

			DateTime lastSend = DateTime.MinValue, lastRecv = DateTime.MinValue, added = DateTime.Now;

			if(sLastSend != "")
			{
				if(DateTime.TryParse(sLastSend, null, DateTimeStyles.None, out DateTime dtS)) lastSend = dtS;
				else {
					parseError++;
					Program.Log("Data.Import()", $"Line {cnt}: LastSend date format was incorrect. Val={sLastSend}");
				}
			}

			if(sLastRecv != "")
			{
				if(DateTime.TryParse(sLastRecv, null, DateTimeStyles.None, out DateTime dtR)) lastRecv = dtR;
				else {
					parseError++;
					Program.Log("Data.Import()", $"Line {cnt}: LastRecv date format was incorrect. Val={sLastRecv}");
				}
			}

			if(sAdded != "")
			{
				if(DateTime.TryParse(sAdded, null, DateTimeStyles.None, out DateTime dtA)) added = dtA;
				else {
					parseError++;
					Program.Log("Data.Import()", $"Line {cnt}: 'Added' date format was incorrect, set to DateTime.Now   Val={sAdded}");
				}
			}

			neighbors.Add(new(name, nameSend, nameRecv, codeSend, codeRecv, group, added, lastSend, lastRecv, cntSent, cntRecv));


		}
		ReOrder();
		Save();

		if(fail > 0)
		{
			MessageBox.Show($"Import file is corrupt, {fail}/{cnt} lines failed.\r\nPlease check the file format.");
			Program.Log("Data.Import()", $"*** File '{file}' is malformed, {fail}/{cnt} lines failed.");
			return;
		}

		if(parseError > 0)
		{
			MessageBox.Show($"Import file had some datetime errors, but all lines were imported.\r\nPlease check the log for details.");
			Program.Log("Data.Import()", $"*** File '{file}' had parse errors, {fail}/{cnt} lines failed.");
			return;
		}


		MessageBox.Show($"Import Added {cnt} neighbors successfully!");
		Program.Log("Data.Import()", $"Added {cnt} neighbors successfully!");
	}

	public void ReOrder()
	{
		int ord = 0;
		foreach(Neighbor n in neighbors)
			n.Order = ord++;
	}

}
