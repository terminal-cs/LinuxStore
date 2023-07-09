namespace LinuxStore.Interfaces;

public class Pacman : PackageManager
{
	#region Methods

	public override IEnumerable<string> Search(string Package)
	{
		// List all packages.
		StreamReader? Stream = Execute("/bin/pacman", $"-Ss {Package}", false, true);

		// Check if the return value is null.
		Stream = Stream ?? throw new InvalidDataException("The return value was null!");

		// Run only until the output has stopped.
		while (!Stream.EndOfStream)
		{
			// Read the next line of data.
			string? Line = Stream.ReadLine();

			// Skip the description line.
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
		Execute("/bin/pacman", $"-S {Package}", true, false);
	}

	public override void Remove(string Package)
	{
		Execute("/bin/pacman", $"-Rns {Package}", true, false);
	}

	public override void About(string Package)
	{
		Execute("/bin/pacman", $"-Qi {Package}", false, false);
	}

	public override void Update()
	{
		Execute("/bin/pacman", "-Syu", true, false);
	}

	public override void Clean()
	{
		Execute("/bin/pacman", "-Rsn $(pacman -Qdtq)", true, false);
	}

	#endregion
}