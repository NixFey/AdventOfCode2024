using System.CommandLine.Invocation;
using System.Diagnostics;
using Spectre.Console;

namespace AdventOfCode2024.Helpers;

public static class RunSolveCommand
{
    public static async Task Command(int dayNumber, Part part, string? inputString, FileInfo? inputFile,
        InvocationContext ctx, IEnumerable<Day> days)
    {
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

                AnsiConsole.Write(new Rule("Part 1")
                {
                    Style = Style.Parse("green"),
                    Justification = Justify.Left
                });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var p1Result = await dayProblem.SolvePart1(input, progress);
                stopwatch.Stop();
                solveTask.StopTask();
                AnsiConsole.WriteLine(p1Result);
                AnsiConsole.MarkupInterpolated($"  [olive]{stopwatch.ElapsedMilliseconds}ms[/]");
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

                AnsiConsole.Write(new Rule("Part 2")
                {
                    Style = Style.Parse("green"),
                    Justification = Justify.Left
                });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var p2Result = await dayProblem.SolvePart2(input, progress);
                stopwatch.Stop();
                solveTask.StopTask();
                AnsiConsole.WriteLine(p2Result);
                AnsiConsole.MarkupInterpolated($"  [olive]{stopwatch.ElapsedMilliseconds}ms[/]");
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
    }
}