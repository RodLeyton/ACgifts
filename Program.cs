using System.Diagnostics;

namespace ACgifts;

internal static class Program
{
	private static StreamWriter? logfile;
	private static readonly object _locker = new();

#if DEBUG
	public static string LOG_FILE = "log-debug.txt";
#else
	public static string LOG_FILE = "log-release.txt";
#endif



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
					throw new TimeoutException("Timeout waiting for exclusive access");
			}
			catch(AbandonedMutexException)
			{
				hasHandle = true;
			}
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
		Directory.CreateDirectory("data");
		SetupLogfile();
		Log("", $"App startup {DateTime.Now:u}");

		Application.Run(new MainForm());

		Log("",$"App closed {DateTime.Now:u}\r\n");
		logfile?.Flush();
		logfile?.Close();

#else


		try
		{
			Directory.CreateDirectory("data");
			SetupLogfile();
			Log("", $"App startup {DateTime.Now:u}");

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
			logfile?.Flush();
			logfile?.Close();
		}
#endif

	}



	public static void Log(string from, string msg)
	{
		lock(_locker)
		{
			if(from != "") logfile?.Write(from.PadRight(20));
			logfile?.WriteLine(msg);
#if DEBUG
			Debug.WriteLine(msg);
#endif
		}
	}
	public static void Log(string from, Exception ex)
	{
		lock(_locker)
		{
			logfile?.WriteLine($"----- {from}  {ex.Message}  ".PadRight(80,'-'));
			logfile?.WriteLine(ex.Message);
			logfile?.WriteLine(ex.StackTrace);
			logfile?.WriteLine(new string('-',80));
#if DEBUG
			Debug.WriteLine($"----- {from}  {ex.Message}");
			Debug.WriteLine(ex.StackTrace);
#endif
		}
	}

	public static string GetLogs()
	{
		lock(_locker)
		{
			if(logfile == null) return "";
			try
			{
				logfile.Flush();
				logfile.Close();
				string content = File.ReadAllText(GetFullLogPath());
				return content;
			}
			catch (Exception ex)
			{
				return ex.Message + "\r\nError reading logfile, click the Dir button and navigate to the data directory.";
			}
			finally
			{
				logfile = new StreamWriter(GetFullLogPath(), append: true) { AutoFlush = true };
			}
		}
	}
	private static void SetupLogfile()
	{
		if(logfile != null) throw new Exception("Cannot rotate logs when they are open!");
		string noExt = Path.ChangeExtension(GetFullLogPath(), null);

		try
		{
			if(!File.Exists(GetFullLogPath()))
			{
				logfile = new StreamWriter(GetFullLogPath(), append: true) { AutoFlush = true };
				Log("Program",$"logfile created {DateTime.Now:u}");
				return;
			}

			FileInfo fi = new(GetFullLogPath());
			if(fi.Length > 10000)
			{
				if(File.Exists(noExt + ".bak.txt"))
					File.Delete(noExt + ".bak.txt");

				File.Move(GetFullLogPath(), noExt + ".bak.txt");
				logfile = new StreamWriter(GetFullLogPath(), append: true) { AutoFlush = true };
				Log("Program", $"logfile rotated {DateTime.Now:u}");
				return;
			}
			logfile = new StreamWriter(GetFullLogPath(), append: true) { AutoFlush = true };
		}
		catch (Exception ex)
		{
			MessageBox.Show("ACgifts encountered an error rotating logs: \r\n" + ex.Message);
		}
	}
	
	
	public static string GetFullLogPath()
	{
		return GetDataDir() + LOG_FILE;
	}
	public static string GetAppDir()
	{
		return Directory.GetCurrentDirectory().TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
	}
	public static string GetDataDir()
	{
		return GetAppDir() + "data" + Path.DirectorySeparatorChar;
	}


#if !DEBUG
	static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
	{
		logfile?.WriteLine("\r\nThreadException, caught in Program.Main()");
		logfile?.WriteLine("From: " + sender.ToString());
		logfile?.WriteLine(e.ToString());
		logfile?.WriteLine(e.Exception.ToString());
		logfile?.WriteLine(" ");
		logfile?.Flush();
		MessageBox.Show("It appears ACgifts has encountered an unexpected event.\nPlease check the logfile for details.");
	}
	static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		logfile?.WriteLine("\r\nUnhandledException, caught in Program.Main()");
		logfile?.WriteLine("From: " + sender.ToString());
		logfile?.WriteLine(e.ToString());
		logfile?.WriteLine(e.ExceptionObject.ToString());
		logfile?.WriteLine(" ");
		logfile?.Flush();
		MessageBox.Show("It appears ACgifts has encountered an unexpected event.\nPlease check the logfile for details.");
	}
#endif



}