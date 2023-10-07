
namespace ACgifts;

class WindowRestore
{
	public static void GeometryFromString(string thisWindowGeometry, Form formIn)
	{
		if(string.IsNullOrEmpty(thisWindowGeometry) == true) return;

		string[] numbers = thisWindowGeometry.Split('|');

		if(numbers.Length != 8) return;

		string windowString = numbers[4];
		if(windowString == "Normal")
		{
			string screens = ScreensGeometryToString();
			if(AllScreensPresent(numbers[7], screens))
			{
				formIn.Location = new(int.Parse(numbers[0]), int.Parse(numbers[1]));
				formIn.Size = new(int.Parse(numbers[2]), int.Parse(numbers[3]));
				formIn.StartPosition = FormStartPosition.Manual;
				formIn.WindowState = FormWindowState.Normal;
			}
		}
		else if(windowString == "Maximized")
		{
			formIn.Location = new Point(int.Parse(numbers[5]), int.Parse(numbers[6]));
			formIn.StartPosition = FormStartPosition.Manual;
			formIn.WindowState = FormWindowState.Maximized;
		}
	}

	public static string GeometryToString(Form mainForm)
	{
		return mainForm.Location.X.ToString() + "|" +    //0
			mainForm.Location.Y.ToString() + "|" +      //1
			mainForm.Size.Width.ToString() + "|" +      //2
			mainForm.Size.Height.ToString() + "|" +     //3
			mainForm.WindowState.ToString() + "|" +     //4
			(mainForm.Location.X + mainForm.Size.Width / 2).ToString() + "|" +  //5
			(mainForm.Location.Y + mainForm.Size.Height / 2).ToString() + "|" + //6
			ScreensGeometryToString();                  //7
	}

	public static string ScreensGeometryToString()
	{
		string screensGeometry = "";
		foreach(Screen s in Screen.AllScreens)
			screensGeometry += s.WorkingArea;
		return screensGeometry;
	}

	public static bool AllScreensPresent(string savedScreens, string currentScreens)
	{
		// When unplugging or turning on or off a screen, device order changes in Screen.AllScreens
		// We will check each device is in the list in any order here.
		bool screensPresent = true;

		string[] saved = savedScreens.Split('{', '}');

		foreach(string s in saved)
			if(s != "" && !currentScreens.Contains(s))
				screensPresent = false;

		return screensPresent;
	}


}

