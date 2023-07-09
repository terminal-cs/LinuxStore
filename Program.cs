using LinuxStore.Interfaces;

namespace LinuxStore;

public static class Program
{
	#region Methods

	public static void Main(string[] args)
	{
		// Get any available instance of a package manager.
		// Get any available instance of a package manager.
		PackageManager? Manager =
			File.Exists("/bin/xbps-install") ? new XBPS() :
			File.Exists("/bin/apt") ? new Aptitude() :
			//change to /usr/bin for arch since that is the reported path on a pure arch install.
			File.Exists("/usr/bin/yay") ? new Yay() :
			//check for pacman last as yay can do pacman operations also and we want to use yay if it's available because of AUR.
			File.Exists("/usr/bin/pacman") ? new Pacman() :
			null;

		// Check if the package manager wasn't initialized.
		if (Manager == null)
		{
			Console.WriteLine("No compatable package manager was found! Please open an issue about this.");
			return;
		}

		if (args.Length == 0)
		{
			Console.WriteLine("GUI: Coming Soon!");
			return;
		}

		if (args[0] == "-h" || args[0] == "--help")
		{
			Console.WriteLine("LinuxStore [Options] {Package}");
			Console.WriteLine("- search {package} - Searches for a package matching the specified text.");
			Console.WriteLine("- install {package} - Installs the requested package.");
			Console.WriteLine("- remove {package} - Removes the requested package and it's dependancies.");
			Console.WriteLine("- about {package} - Gets info on a locally installed package. Use no arguments for info about this app.");
			Console.WriteLine("- update - Begins all pending updates on the system.");
			Console.WriteLine("- clean - Cleans unwanted packaged from the system.");
			return;
		}

		// Check for invalid input - exception being clean and update.
		if (args[0] != "about" && args[0] != "clean" && args[0] != "update" && args.Length < 2)
		{
			Console.WriteLine("Not enough arguments specified. Use -h/--help for command info.");
			return;
		}

		switch (args[0])
		{
			case "search":
				Console.WriteLine("Searching...");
				foreach (string Package in Manager.Search(args[1]))
				{
					Console.WriteLine(Package);
				}
				break;
			case "install":
				Manager.Install(args[1]);
				break;
			case "remove":
				Manager.Remove(args[1]);
				break;
			case "about":
				if (args.Length == 1)
				{
					Console.WriteLine("LinuxStore - Version 1.0");
					Console.WriteLine($"Using {Manager.GetType().Name}.");
					return;
				}

				Manager.About(args[1]);
				break;
			case "update":
				Manager.Update();
				break;
			case "clean":
				Manager.Clean();
				break;
			default:
				Console.WriteLine($"Unknown command '{args[0]}'. Use -h/--help for command info.");
				break;
		}
	}

	#endregion
}