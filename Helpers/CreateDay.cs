using System.Diagnostics;

namespace AdventOfCode2024.Helpers;

public static class CreateDay
{
    private const string ProgramDayInsertToken = "//// NEW DAYS";
    public static void Command(int dayNumber)
    {
        Console.WriteLine($"Creating day {dayNumber:00}");

        var currentDir = Directory.GetCurrentDirectory();
        var programFile = new FileInfo(Path.Join(currentDir, "Program.cs"));
        if (!programFile.Exists)
        {
            Console.Error.WriteLine("Unable to find Program.cs, are you in the right directory?");
            return;
        }

        var programContent = File.ReadAllText(programFile.FullName);

        var tokenLocation = programContent.IndexOf(ProgramDayInsertToken, StringComparison.InvariantCulture);
        if (tokenLocation == -1)
        {
            Console.Error.WriteLine($"Could not find \"{ProgramDayInsertToken}\" in Program.cs");
            return;
        }

        var newDayDir = new DirectoryInfo(Path.Join(currentDir, $"Day{dayNumber:00}"));
        if (newDayDir.Exists)
        {
            Console.Error.WriteLine($"Day directory already exists");
            return;
        }
        
        newDayDir.Create();
        
        var dayTemplateDir = new DirectoryInfo(Path.Join(currentDir, $"DayTemplate"));

        foreach (var file in dayTemplateDir.GetFiles())
        {
            var newFile = file.CopyTo(Path.Combine(newDayDir.FullName, file.Name));

            var fileContent = File.ReadAllText(newFile.FullName);
            fileContent = fileContent.Replace("__DAYNUM__", $"{dayNumber:00}");
            File.WriteAllText(newFile.FullName, fileContent);
        }

        programContent = programContent.Insert(tokenLocation,
            $"new({dayNumber}, typeof(AdventOfCode2024.Day{dayNumber:00}.Problem)),\n    ");
        File.WriteAllText(programFile.FullName, programContent);

        try
        {
            var res = Process.Start("git", ["add", programFile.FullName, newDayDir.FullName]);
            res.WaitForExit();

            if (res.ExitCode != 0) throw new ApplicationException();
        }
        catch
        {
            Console.WriteLine("WARN: Unable to add day files to Git");
        }
    }
}