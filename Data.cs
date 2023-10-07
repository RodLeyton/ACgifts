using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;

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

		neighbors.Clear();
		if(!File.Exists(FILENAME + FILEEXT)) return;

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
					Program.Log($"Data.Load() Line {cnt} failed parse: {line}");
					fail++;
				}
			}
			catch(Exception ex)
			{
				fail++;
				Program.Log("Exception caught in Data.Load()");
				Program.Log($"Line {cnt} => {line}");
				Program.Log(ex);
			}
		}
		sr.Close();
		if(fail > 0)
		{
			MessageBox.Show($"Not all data was imported, {fail}/{cnt} failed. Contact the developer for assistance.");
			Program.Log($"*** Data.Load() complete. {fail}/{cnt} failed.");
		}
	}

	public void Save()
	{
		if(neighbors.Count == 0)
		{
			Program.Log($"Data.Save() Aborted, neighbors is empty.");
			return;
		}

		Program.Log($"Data.Save() Saving datafile.");
		if(!dataBackupDone)
		{
			Program.Log($"Data.Save() Moving backups");
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

				Program.Log($"Data.Save() Moved backupfiles successfully.");
			}
			catch (Exception ex)
			{
				Program.Log($"*** Data.Save() Exception moving backup data files ***");
				Program.Log(ex);
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
			Program.Log($"*** Data.Save() Exception saving current data file ***");
			Program.Log(ex);
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
			FileName = $"ACgifts_backup_{DateTime.Now:yyyy-MM-dd}{FILEEXT}"
		};
		if(sfd.ShowDialog() != DialogResult.OK) return;

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
			Program.Log($"Data.Restore() Restore started src='{ofd.FileName}'");
			if(!File.Exists(ofd.FileName))
			{
				MessageBox.Show($"Couldn't access {ofd.FileName}, please check permissions.");
				Program.Log($"*** Data.Restore() Couldn't access {ofd.FileName}' restore aborted.");
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
						Program.Log($"Data.Restore() Line {cnt} failed parse: {line}");
						fail++;
					}
				}
				catch(Exception ex)
				{
					fail++;
					Program.Log("*** Data.Restore() Exception caught in Data.Load() ***");
					Program.Log($"Line {cnt} => {line}");
					Program.Log(ex);
					break;
				}
			}
			sr.Close();
			if(fail > 0)
			{
				MessageBox.Show($"Backup file is corrupt, {fail}/{cnt} failed.\r\nAll data from restore was rejected, current data has been maintained.\r\nContact the developer for assistance.");
				Program.Log($"Backup file '{ofd.FileName}' is corrupt, {fail}/{cnt} failed. Restore aborted.");
				return;
			}

			string backup = $"ACgifts_backup_pre_restore_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{FILEEXT}";

			Program.Log($"Data.Restore() Backed file parsed successfully. {cnt} records found.");

			if(neighbors.Count > 0 && File.Exists(FILENAME + FILEEXT))
			{
				Program.Log($"Data.Restore() Backing up current data file to '{backup}'");
				Save();
				File.Move(FILENAME + FILEEXT, backup);
			}

			neighbors.Clear();
			foreach(Neighbor n in temp) neighbors.Add(n);
			temp.Clear();
			MessageBox.Show($"Restored all {cnt} neighbors successfully!");


			Program.Log($"Data.Restore() Saving new data file.");
			Save();
			Program.Log($"Data.Restore() Complete.");
		}
	}
	public void Import(string file)
	{
		DialogResult res = MessageBox.Show("Do you want to clear existing data?", "Delete data?", MessageBoxButtons.YesNoCancel);
		if(res == DialogResult.Cancel) return;

		Program.Log($"Data.Import() Importing data: src='{file}'");


		if(res == DialogResult.Yes)
		{
			neighbors.Clear();
			Program.Log($"Data.Import() Cleared existing data.");
		}
		else
		{
			Program.Log($"Data.Import() Retaining {neighbors.Count} existing records.");

			string backup = $"ACgifts_backup_pre_import_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{FILEEXT}";

			Program.Log($"Data.Import() Backing up current data file to '{backup}'");
			Save();
			File.Move(FILENAME + FILEEXT, backup);
		}


		using TextFieldParser parser = new(file);
		parser.TextFieldType = FieldType.Delimited;
		parser.HasFieldsEnclosedInQuotes = true;
		parser.SetDelimiters(",");
		parser.ReadLine();
		int cnt = 0, fail = 0;
		int impDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));


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
				Program.Log($"*** Data.Import() Exception caught for parser.ReadFields() on line {cnt} ***");
				Program.Log(ex);
				cnt++;
				continue;
			}

			if(fields is null)
			{
				fail++;
				Program.Log($"*** Data.Import() TextFieldParser Couldn't Parse line {cnt}");
				cnt++;
				continue;
			}
			if(fields.Length != 12)
			{
				fail++;
				Program.Log($"*** Data.Import() Only {fields.Length} fields found on line {cnt}. Dropped data = {string.Join(',', fields)}");
				cnt++;
				continue;
			}

			cnt++;
			//Order,Name,NameSend,CodeSend,LastSend,CntSent,NameRecv,CodeRecv,LastRecv,CntRecv,Group,Added
			string sOrder = fields[0];
			string name = fields[1];
			string nameSend = fields[2];
			string codeSend = fields[3];
			string sLastSend = fields[4];
			string sCntSent = fields[5];
			string nameRecv = fields[6];
			string codeRecv = fields[7];
			string sLastRecv = fields[8];
			string sCntRecv = fields[9];
			string group = fields[10];
			string sAdded = fields[11];

			if(!int.TryParse(sOrder, out int order)) order = 0;
			if(!int.TryParse(sCntSent, out int cntSent)) cntSent = 0;
			if(!int.TryParse(sCntRecv, out int cntRecv)) cntRecv = 0;

			DateTime lastSend = DateTime.MinValue, lastRecv = DateTime.MinValue;
			if(DateTime.TryParse(sLastSend, null, DateTimeStyles.None, out DateTime dtS))
				lastSend = dtS;
			if(DateTime.TryParse(sLastRecv, null, DateTimeStyles.None, out DateTime dtR))
				lastRecv = dtR;
			if(!DateTime.TryParse(sLastRecv, null, DateTimeStyles.None, out DateTime added))
				added = DateTime.Now;


			//if(DateTime.TryParseExact(sLastSend, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime dtS))
			//	lastSend = dtS;
			//if(DateTime.TryParseExact(sLastRecv, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime dtR))
			//	lastRecv = dtR;
			//if(!DateTime.TryParseExact(sLastRecv, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime added))
			//	added = DateTime.Now;

			neighbors.Add(new(order, name, nameSend, nameRecv, codeSend, codeRecv, group, added, lastSend, lastRecv, cntSent, cntRecv));
		}
		Save();

		if(fail > 0)
		{
			MessageBox.Show($"Import file is corrupt, {fail}/{cnt} lines failed.\r\nPlease check the file format.");
			Program.Log($"Data.Import() File '{file}' is malformed, {fail}/{cnt} lines failed.");
			return;
		}


		MessageBox.Show($"Import Added {cnt} neighbors successfully!");
		Program.Log($"Data.Import() Added {cnt} neighbors successfully!");
	}
}
