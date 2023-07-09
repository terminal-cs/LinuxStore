using System.Text;
using System.Collections.Generic;
namespace LinuxStore.Interfaces;

public class Yay : PackageManager
{
	#region Methods
    public static string temp_file = "files.txt";

	public override IEnumerable<string> Search(string Package)
	{
		// List all packages.
		StreamReader? Stream = Execute("/usr/bin/yay", $"-Ss {Package}", false, true);

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
		Execute("/usr/bin/yay", $"-S {Package}", false, false);
	}

	public override void Remove(string Package)
	{
		Execute("/usr/bin/yay", $"-Rns {Package}", false, false);
	}

	public override void About(string Package)
	{
		Execute("/usr/bin/yay", $"-Qi {Package}", false, false);
	}

	public override void Update()
	{
		Execute("/usr/bin/yay", "-Syu", false, false);
	}

	public override void Clean()
	{
        //Yay does not like to be piped into a second command, so we use hack it by writing to a file , then reading from it and executing the second command
        ExecuteLogged("/usr/bin/yay", " -Qdtq", false, true,temp_file);
        List<string>Packages = new List<string>();
        const Int32 BufferSize = 128;
        String line;
        using (var fileStream = File.OpenRead(temp_file)) 

        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) 
        {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if(!String.IsNullOrEmpty(line))
                        {
                            Packages.Add(line);
                        } 
                    }
                    string result = String.Join(" ", Packages);
                    Execute("/usr/bin/yay", "-Rsn --noconfirm "+result, false, false);
        if(File.Exists(temp_file))
        {
            File.Delete(temp_file);
        }
            Packages.Clear();
    	}
    }

	#endregion
}