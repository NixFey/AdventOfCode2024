using System.CommandLine;
using AdventOfCode2024;
using AdventOfCode2024.Helpers;

var days = new Day[]
{
    new(1, typeof(AdventOfCode2024.Day01.Problem)),
    new(2, typeof(AdventOfCode2024.Day02.Problem)),
    new(3, typeof(AdventOfCode2024.Day03.Problem)),
    new(4, typeof(AdventOfCode2024.Day04.Problem)),
    new(5, typeof(AdventOfCode2024.Day05.Problem)),
    new(6, typeof(AdventOfCode2024.Day06.Problem)),
    new(7, typeof(AdventOfCode2024.Day07.Problem)),
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

    await RunSolveCommand.Command(dayNumber, part, inputString, inputFile, ctx, days);
});

rootCommand.AddCommand(solveCommand);

await rootCommand.InvokeAsync(args);