using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace ACgifts;

internal static class Program
{
	private static StreamWriter? logfile;
	private static readonly object _locker = new();




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
		RotateLogs("data/log-debug");
		logfile = new StreamWriter("data/log-debug.txt", append: true) { AutoFlush = true };
		Log($"App startup {DateTime.Now}");

		try
		{
			Application.Run(new MainForm());
		}
		catch(Exception ex)
		{
			MessageBox.Show("Program.Main() Exception: " + ex.Message);
			Log("*** Program.Main() Fatal Exception ***");
			Log(ex);
		}
		finally
		{
			Log("App closed " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff \"GMT\"zzz") + "\r\n");
			logfile?.Flush();
			logfile?.Close();
		}


#else


		try
		{
			Directory.CreateDirectory("data");
			RotateLogs("data/log-release");
			logfile = new StreamWriter("data/log-release.txt", append: true) { AutoFlush = true };
			Log($"App startup {DateTime.Now}");

			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.Run(new MainForm());

		}	
		catch(Exception ex)
		{
			MessageBox.Show("ACgifts Unhandled Exception. Sorry this is fatal, Exiting.\n" + ex.Message, "ACgifts Fatal Error.");
			Log("*** Program.Main() Fatal Exception ***");
			Log(ex);
		}
		finally
		{
			Log("App closed " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff \"GMT\"zzz") + "\r\n");
			logfile?.Flush();
			logfile?.Close();
		}

#endif

	}







	public static void Log(string msg)
	{
		lock(_locker)
		{
			logfile?.WriteLine(msg);
#if DEBUG
			Debug.WriteLine(msg);
#endif
		}
	}
	public static void Log(Exception ex)
	{
		lock(_locker)
		{
			logfile?.WriteLine(ex.Message);
			logfile?.WriteLine(ex.StackTrace);
#if DEBUG
			Debug.WriteLine(ex.Message);
			Debug.WriteLine(ex.StackTrace);
#endif
		}
	}

	private static void RotateLogs(string filename)
	{
		try
		{
			if(!File.Exists(filename + ".txt")) return;
				
			FileInfo fi = new(filename + ".txt");
			if(fi.Length < 10000) return;


			if(File.Exists(filename + ".bak.txt"))
				File.Delete(filename + ".bak.txt");

			File.Move(filename + ".txt", filename + ".bak.txt");
		}
		catch (Exception ex)
		{
			MessageBox.Show("ACgifts encountered an error rotating logs: \r\n" + ex.Message);
		}
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