namespace LinuxStore.Interfaces;

public class Aptitude : PackageManager
{
	#region Methods

	public override IEnumerable<string> Search(string Package)
	{
		// List all packages.
		Execute("/bin/apt", "update", true, true);
		StreamReader? Stream = Execute("/bin/apt", $"search {Package}", false, true);

		// Check if the return value is null.
		Stream = Stream ?? throw new InvalidDataException("The return value was null!");

		// Skip first two lines.
		Stream.ReadLine();
		Stream.ReadLine();

		// Run only until the output has stopped.
		while (!Stream.EndOfStream)
		{
			// Read the next line of data.
			string? Line = Stream.ReadLine();

			// Skip the description lines.
			Stream.ReadLine();
			Stream.ReadLine();

			// Check if line is valid.
			if (string.IsNullOrEmpty(Line))
			{
				continue;
			}

			// Add the package listing to the yield return array - this is handled by the runtime.
			yield return Line;
		}
	}

	public override void Install(string Package)
	{
		Execute("/bin/apt", "update", true, false);
		Execute("/bin/apt", $"install {Package}", true, false);
	}

	public override void Remove(string Package)
	{
		Execute("/bin/apt", $"remove {Package}", true, false);
	}

	public override void About(string Package)
	{
		Execute("/bin/apt", "show {Package}", false, false);
	}

	public override void Update()
	{
		Execute("/bin/apt", "update", true, false);
		Execute("/bin/apt", "upgrade", true, false);
	}

	public override void Clean()
	{
		Execute("/bin/apt", "autoclean", true, false);
		Execute("/bin/apt", "clean", true, false);
	}

	#endregion
}