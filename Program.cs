using System.Diagnostics;
using System.Text.Json;

namespace ACgifts;

internal static class Program
{
	public static AppConfig appConfig = null!;
	private static StreamWriter? swLog;
	private static readonly object _locker = new();
	private static string APP_DIR = null!, DATA_DIR = null!, LOG_FILE = null!, LOG_FILE_NO_EXT = null!;

	public static bool IsDeployed = false;
	public static bool IsDebug = false;
	public static string Version = "local build";

	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();

		using Mutex mutex = new(false, "ACgifts-782c40fa-db6b-4f0d-b7f3-38c9745e483a", out _);
		var hasHandle = false;
		try
		{
			try
			{
				hasHandle = mutex.WaitOne(5000, false);
				if(hasHandle == false)
				{
					MessageBox.Show("ACgifts is already running!","Cannot start second instance");
					return;
				}
			}
			catch(AbandonedMutexException) { hasHandle = true; }

			try
			{
				APP_DIR = GetAppDir();
				DATA_DIR = GetDataDir();
				LOG_FILE = GetLogFile();
				LOG_FILE_NO_EXT = Path.ChangeExtension(LOG_FILE, null);

				if(!Directory.Exists(DATA_DIR)) Directory.CreateDirectory(DATA_DIR);

				string[] confLines = File.ReadAllLines(DATA_DIR + "app.config");
				appConfig = (AppConfig)(JsonSerializer.Deserialize(confLines[0], typeof(AppConfig)) ?? new AppConfig());
			}
			catch(Exception ex)
			{
				Program.Log("Program.Init", $"Exception caught loading AppConfig");
				Program.Log("Program.Init", ex);
			}
			finally { appConfig ??= new(); }

			DoWork();
		}
		finally
		{
			if(hasHandle) mutex.ReleaseMutex();
		}
	}
	private static void DoWork()
	{

#if DEBUG
		IsDebug = true;
		Init();
		Application.Run(new MainForm());

		Log("Program.Main",$"App closed {DateTime.Now:u}\r\n");
		swLog?.Flush();
		swLog?.Close();

#else
		try
		{
			string? v = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion");
			if(v != null)
			{
				Version = "v" + v;
				IsDeployed = true;
			}
			Init();
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.Run(new MainForm());

		}	
		catch(Exception ex)
		{
			MessageBox.Show("ACgifts Unhandled Exception. Sorry this is fatal, Exiting.\n" + ex.Message, "ACgifts Fatal Error.");
			Log("Program.Main()", $"*****  Fatal Exception  *****");
			Log("Program.Main()", ex);
		}
		finally
		{
			Log("",$"App closed {DateTime.Now:u}\r\n");
			swLog?.Flush();
			swLog?.Close();
		}
#endif

	}


	public static string GetLogContent()
	{
		lock(_locker)
		{
			if(swLog == null) return "";
			try
			{
				swLog.Flush();
				swLog.Close();
				string content = File.ReadAllText(LOG_FILE);
				return content;
			}
			catch (Exception ex)
			{
				return ex.Message + "\r\nError reading logfile, click the Dir button and navigate to the data directory.";
			}
			finally
			{
				swLog = new StreamWriter(LOG_FILE, append: true) { AutoFlush = true };
			}
		}
	}
	private static void Init()
	{
		if(swLog != null) throw new Exception("Cannot rotate logs when they are open!");

		try
		{
			if(!File.Exists(LOG_FILE))
			{
				swLog = new StreamWriter(LOG_FILE, append: true) { AutoFlush = true };
				Log("Program.Init", $"Logfile created {DateTime.Now:u}");
			}
			else
			{
				FileInfo fi = new(LOG_FILE);
				if(fi.Length > 256000)
				{
					if(File.Exists(LOG_FILE_NO_EXT + ".bak.txt"))
						File.Delete(LOG_FILE_NO_EXT + ".bak.txt");

					File.Move(LOG_FILE, LOG_FILE_NO_EXT + ".bak.txt");
					swLog = new StreamWriter(LOG_FILE, append: true) { AutoFlush = true };
					Log("Program.Init", $"Logfile rotated {DateTime.Now:u}");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("ACgifts encountered an error rotating logs: \r\n" + ex.Message);
		}


		try
		{
			swLog ??= new StreamWriter(LOG_FILE, append: true) { AutoFlush = true };

			Log("Program.Init", $"App startup {DateTime.Now:u}");
			Log("Program.Init", $"APP_DIR {APP_DIR}");
			Log("Program.Init", $"Debug:{IsDebug}  Deployed:{IsDeployed}  Version:{Version}");
			CopyAppFiles();
		}
		catch(Exception ex)
		{
			MessageBox.Show("ACgifts encountered an initialisation error: \r\n" + ex.Message);
		}

		if(appConfig.IsDefaults) Program.Log("Program.Init", "AppConfig does not exist. Using defaults");
		else Program.Log("Program.Init", "AppConfig loaded successfully");
	}
	private static void CopyAppFiles()
	{
		DirectoryInfo dirFiles = new(@"files");
		if(!dirFiles.Exists) Log("Program.Init", $"Files directory not found!");
		else
		{
			FileInfo[] Files = dirFiles.GetFiles();
			foreach(FileInfo file in Files)
			{
				try
				{
					FileInfo fileDest = new(APP_DIR + file.Name);
					if(!fileDest.Exists || file.LastWriteTime > fileDest.LastWriteTime)
					{
						Log("Program.Init", $"Copying '{file.Name}' to APP_DIR");
						File.Copy(file.FullName, APP_DIR + file.Name, true);
					}
				}
				catch (Exception ex) { Log("Program.CopyAppFiles", $"Exception on '{file.Name}' : {ex.Message}"); }
			}
		}

		DirectoryInfo dirDocs = new(@"docs");
		if(!dirDocs.Exists) Log("Program.Init", $"Docs directory not found!");
		else
		{
			FileInfo[] Files = dirDocs.GetFiles();
			foreach(FileInfo file in Files)
			{
				try
				{
					FileInfo fileDest = new(APP_DIR + file.Name);
					if(!fileDest.Exists || file.LastWriteTime > fileDest.LastWriteTime)
					{
						Log("Program.Init", $"Copying '{file.Name}' to APP_DIR");
						File.Copy(file.FullName, APP_DIR + file.Name, true);
					}
				}
				catch(Exception ex) { Log("Program.CopyAppFiles", $"Exception on '{file.Name}' : {ex.Message}"); }
			}
		}
	}
	public static void SaveConfig()
	{
		StreamWriter? sFile = null;
		try
		{
			sFile = new StreamWriter(DATA_DIR + "app.config", append: false) { AutoFlush = true };
			appConfig.IsDefaults = false;
			sFile.WriteLine(JsonSerializer.Serialize(appConfig));
			Program.Log($"Program.SaveConfig", "Config saved successfully");
		}
		catch(Exception ex)
		{
			Program.Log("Program.SaveConfig", "Exception saving Config");
			Program.Log("Program.SaveConfig", ex);
		}
		finally
		{
			sFile?.Flush();
			sFile?.Close();
		}
	}



	public static string GetAppDir()
	{
#if DEBUG
		string subDir = "ACgifts_Debug";
#else
		string subDir = "ACgifts_Release";
#endif
		string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		return Path.Combine(localAppData, "CrashnBurn", subDir) + Path.DirectorySeparatorChar;
	}
	public static string GetDataDir()
	{
		return Path.Combine(GetAppDir(), "Data") + Path.DirectorySeparatorChar;
	}
	public static string GetLogFile()
	{
		return Path.Combine(GetAppDir(), "logfile.txt");
	}




	public static void Log(string from, string msg)
	{
		lock(_locker)
		{
			if(from != "") swLog?.Write(from.PadRight(20));
			swLog?.WriteLine(msg);
#if DEBUG
			Debug.WriteLine(msg);
#endif
		}
	}
	public static void Log(string from, Exception ex)
	{
		lock(_locker)
		{
			swLog?.WriteLine($"----- {from}  {ex.Message}  ".PadRight(80, '-'));
			swLog?.WriteLine(ex.Message);
			swLog?.WriteLine(ex.StackTrace);
			swLog?.WriteLine(new string('-', 80));
#if DEBUG
			Debug.WriteLine($"----- {from}  {ex.Message}");
			Debug.WriteLine(ex.StackTrace);
#endif
		}
	}




#if !DEBUG
	static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
	{
		swLog?.WriteLine("\r\nThreadException, caught in Program.Main()");
		swLog?.WriteLine("From: " + sender.ToString());
		swLog?.WriteLine(e.ToString());
		swLog?.WriteLine(e.Exception.ToString());
		swLog?.WriteLine(" ");
		swLog?.Flush();
		MessageBox.Show("It appears ACgifts has encountered an unexpected event.\nPlease check the logfile for details.");
	}
	static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		swLog?.WriteLine("\r\nUnhandledException, caught in Program.Main()");
		swLog?.WriteLine("From: " + sender.ToString());
		swLog?.WriteLine(e.ToString());
		swLog?.WriteLine(e.ExceptionObject.ToString());
		swLog?.WriteLine(" ");
		swLog?.Flush();
		MessageBox.Show("It appears ACgifts has encountered an unexpected event.\nPlease check the logfile for details.");
	}
#endif



}