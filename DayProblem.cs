namespace AdventOfCode2024;

/// <summary>
/// A generic day's problem
/// </summary>
public abstract class DayProblem
{
    public abstract Task<string> SolvePart1(string input, IProgress<SolveProgress> progress);
    public abstract Task<string> SolvePart2(string input, IProgress<SolveProgress> progress);
}