namespace LinuxStore.Interfaces;

public class XBPS : PackageManager
{
	#region Methods

	public override IEnumerable<string> Search(string Package)
	{
		Console.WriteLine("Search is not implemented.");
		return null;
	}

	public override void Install(string Package)
	{
		Execute("/bin/xbps-install", Package, true, false);
	}

	public override void Remove(string Package)
	{
		Execute("/bin/xbps-remove", $"-R {Package}", true, false);
	}

	public override void About(string Package)
	{
		Execute("/bin/xbps-query", Package, false, false);
	}

	public override void Update()
	{
		Execute("/bin/xbps-install", "-Su", true, false);
	}

	public override void Clean()
	{
		Console.WriteLine("WARNING: This action could potentially harm your system.");
		Console.WriteLine("See here: https://www.reddit.com/r/voidlinux/comments/xofavk/comment/ipye2li/?utm_source=share&utm_medium=web2x&context=3");
		Console.Write("Are you sure you want to continue? [ Y/N ] > ");

		if (Console.ReadLine() != "Y")
		{
			Console.WriteLine("Operation cancled.");
			return;
		}

		Execute("/bin/xbps-remove", "-Oo", true, false);
	}

	#endregion
}