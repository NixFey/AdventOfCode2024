using System.CommandLine;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AdventOfCode2024;
using AdventOfCode2024.Helpers;
using Spectre.Console;

var days = new Day[]
{
    new(1, typeof(AdventOfCode2024.Day01.Problem)),
    new(2, typeof(AdventOfCode2024.Day02.Problem)),
    new(3, typeof(AdventOfCode2024.Day03.Problem)),
    //// NEW DAYS
};

var rootCommand = new RootCommand("Advent of Code 2024 Solutions");

var createCommand = new Command("create", "Create the file structure for a new day");
var createDayArg = new Argument<int>("Day Number");
createCommand.AddArgument(createDayArg);
createCommand.SetHandler((ctx) =>
{
    CreateDay.Command(ctx.ParseResult.GetValueForArgument(createDayArg));
});
rootCommand.AddCommand(createCommand);

var solveCommand = new Command("solve", "Solve a part for a day, using either passed in text or a file on disk");
solveCommand.AddAlias("s");
var solveDayArg = new Argument<int>("Day Number");
solveCommand.AddArgument(solveDayArg);
var solvePartArg = new Argument<Part>("Part", () => Part.Both);
solveCommand.AddArgument(solvePartArg);
var inputStrOption = new Option<string>(["-i", "--input-str"], "Input string");
solveCommand.AddOption(inputStrOption);
var inputFileOption = new Option<FileInfo>(["-f", "--input-file"], "Input file path");
solveCommand.AddOption(inputFileOption);
solveCommand.SetHandler(async (ctx) =>
{
    var dayNumber = ctx.ParseResult.GetValueForArgument(solveDayArg);
    var part = ctx.ParseResult.GetValueForArgument(solvePartArg);

    var inputString = ctx.ParseResult.GetValueForOption(inputStrOption);
    var inputFile = ctx.ParseResult.GetValueForOption(inputFileOption);

    if (!(string.IsNullOrEmpty(inputString) ^ inputFile is null))
    {
        Console.Error.WriteLine("Exactly one of (input string, input file) must be provided");
        ctx.ExitCode = 1;
        return;
    }

    string input;
    if (!string.IsNullOrEmpty(inputString))
    {
        input = inputString;
    }
    else if (inputFile is not null)
    {
        if (!inputFile.Exists)
        {
            Console.Error.WriteLine("Input file does not exist");
            ctx.ExitCode = 1;
            return;
        }

        input = await File.ReadAllTextAsync(inputFile.FullName);
    }
    else
    {
        throw new UnreachableException();
    }

    var day = days.FirstOrDefault(d => d.DayNumber == dayNumber);
    if (day is null)
    {
        Console.Error.WriteLine("Day not found");
        ctx.ExitCode = 1;
        return;
    }

    var dayProblem = day.GetDayProblem();
    if (part is Part.Part1 or Part.Both)
    {
        await AnsiConsole.Progress().AutoClear(true).StartAsync(async progCtx =>
        {
            var solveTask = progCtx.AddTask("Solving Part 1");
            solveTask.IsIndeterminate = true;
            var progress = new Progress<SolveProgress>();

            progress.ProgressChanged += HandleProgress(solveTask);
            
            var p1Result = await dayProblem.SolvePart1(input, progress);
            AnsiConsole.Write(new Rule("Part 1")
            {
                Style = Style.Parse("green"),
                Justification = Justify.Left
            });
            AnsiConsole.WriteLine(p1Result);
            AnsiConsole.WriteLine();
        });
    }
    
    if (part is Part.Part2 or Part.Both)
    {
        await AnsiConsole.Progress().AutoClear(true).StartAsync(async progCtx =>
        {
            var solveTask = progCtx.AddTask("Solving Part 2");
            solveTask.IsIndeterminate = true;
            var progress = new Progress<SolveProgress>();

            progress.ProgressChanged += HandleProgress(solveTask);
            
            var p2Result = await dayProblem.SolvePart2(input, progress);
            solveTask.StopTask();
            AnsiConsole.Write(new Rule("Part 2")
            {
                Style = Style.Parse("green"),
                Justification = Justify.Left
            });
            AnsiConsole.WriteLine(p2Result);
            AnsiConsole.WriteLine();
        });
    }

    return;

    EventHandler<SolveProgress> HandleProgress(ProgressTask solveTask) =>
        (_, status) =>
        {
            if (!string.IsNullOrEmpty(status.StatusText))
            {
                solveTask.Description = status.StatusText;
            }

            if (status.CurrentProgress is not null && status.TotalProgress is not null)
            {
                if (solveTask.IsIndeterminate) solveTask.IsIndeterminate = false;
                solveTask.Value = status.CurrentProgress.Value;
                solveTask.MaxValue = status.TotalProgress.Value;
            }
        };
});

rootCommand.AddCommand(solveCommand);

await rootCommand.InvokeAsync(args);

internal enum Part
{
    Part1,
    Part2,
    Both
}

internal class Day(
    int dayNumber,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type dayProblem
)
{
    public int DayNumber => dayNumber;
    public DayProblem GetDayProblem()
    {
        return (DayProblem)Activator.CreateInstance(dayProblem)!;
    }
}