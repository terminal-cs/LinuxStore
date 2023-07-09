using System.Diagnostics;

namespace LinuxStore.Interfaces;

public abstract class PackageManager
{
	#region Methods

	/// <summary>
	/// Executes a one-off command to the specified binary.
	/// </summary>
	/// <returns>The command's output.</returns>
	internal static StreamReader? Execute(string BinaryPath, string Arguments, bool AsAdmin, bool UseRedirect)
	{
		// Creates a new process instance.
		Process Command = new()
		{
			StartInfo = new()
			{
				Arguments = AsAdmin ? $"{BinaryPath} {Arguments}" : Arguments,
				FileName = AsAdmin ? "sudo" : BinaryPath,
				RedirectStandardOutput = UseRedirect,
			},
		};

		// Starts the command.
		Command.Start();

		// Waits for the command to exit.
		Command.WaitForExit();

		// Returns the command's output.
		return UseRedirect ? Command.StandardOutput : null;
	}

	/// <summary>
	/// Executes a one-off command to the specified binary.
	/// </summary>
	/// <returns>The command's output to file.</returns>
	internal static StreamReader? ExecuteLogged(string BinaryPath, string Arguments, bool AsAdmin, bool UseRedirect, string path)
	{
		// Creates a new process instance.
		Process Command = new()
		{
			StartInfo = new()
			{
				Arguments = AsAdmin ? $"{BinaryPath} {Arguments}" : Arguments,
				FileName = AsAdmin ? "sudo" : BinaryPath,
				RedirectStandardOutput = UseRedirect,
			},
		};

		// Starts the command.
		Command.Start();

		// Waits for the command to exit.
		Command.WaitForExit();
		//write to file
		using (StreamWriter sw = File.CreateText(path))
		{
			        using (StreamReader reader = Command.StandardOutput)
        			{
        				string result = reader.ReadToEnd();
						sw.WriteLine(result);
					}
		}
		// Returns the command's output.
		return UseRedirect ? Command.StandardOutput : null;
	}
	

	/// <summary>
	/// Searches for packages matching the specified search queries.
	/// </summary>
	/// <returns>A list of packages matching the search term.</returns>
	public abstract IEnumerable<string> Search(string Package);

	/// <summary>
	/// Installs a package to the system.
	/// </summary>
	public abstract void Install(string Package);

	/// <summary>
	/// Removes a package from the system.
	/// </summary>
	public abstract void Remove(string Package);

	/// <summary>
	/// Prints information about a package.
	/// </summary>
	public abstract void About(string Package);

	/// <summary>
	/// Starts all updates that are available for the system.
	/// </summary>
	public abstract void Update();

	/// <summary>
	/// Removes unwanted packages from the system.
	/// </summary>
	public abstract void Clean();

	#endregion
}