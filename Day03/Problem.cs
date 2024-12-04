using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day03;

public partial class Problem : DayProblem
{
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> progress)
    {
        var matches = Part1Regex().Matches(input);
        var sum = matches.Select(m => int.Parse(m.Groups["n1"].Value) * int.Parse(m.Groups["n2"].Value)).Sum();
        return Task.FromResult(sum.ToString());
    }

    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> progress)
    {
        var matches = Part2Regex().Matches(input);

        var shouldAdd = true;
        var sum = 0;
        foreach (var match in matches.AsEnumerable())
        {
            var instr = match.Groups["instr"].Value;
            switch (instr)
            {
                case "do":
                    shouldAdd = true;
                    break;
                case "don't":
                    shouldAdd = false;
                    break;
                case "mul":
                {
                    if (shouldAdd)
                        sum += int.Parse(match.Groups["n1"].Value) * int.Parse(match.Groups["n2"].Value);
                    break;
                }
            }
        }

        return Task.FromResult(sum.ToString());
    }

    [GeneratedRegex(@"mul\((?<n1>\d{1,3}),(?<n2>\d{1,3})\)")]
    private partial Regex Part1Regex();
    
    /// <summary>
    /// Capture groups:
    /// <list type="bullet">
    ///     <item>
    ///         <description><c>instr</c>: The instruction, <c>do</c>, <c>don't</c>, or <c>mul</c></description>
    ///     </item>
    ///     <item>
    ///         <description><c>n1</c>: If <c>instr</c> is <c>mul</c>, the first number to multiply</description>
    ///     </item>
    ///     <item>
    ///         <description><c>n2</c>: If <c>instr</c> is <c>mul</c>, the second number to multiply</description>
    ///     </item>
    /// </list>
    /// </summary>
    [GeneratedRegex(@"(?:(?<instr>do|don't)\(\))|(?<instr>mul)\((?<n1>\d{1,3}),(?<n2>\d{1,3})\)")]
    private partial Regex Part2Regex();
}