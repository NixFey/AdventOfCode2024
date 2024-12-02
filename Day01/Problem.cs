using System.Text.RegularExpressions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day01;

public partial class Problem : DayProblem
{
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> _)
    {
        var lines = ParseInput(input);
        var leftList = lines.Left.OrderBy(i => i);
        var rightList = lines.Right.OrderBy(i => i);

        var runningTotal = leftList.Zip(rightList).Sum(it => Math.Abs(it.First - it.Second));

        return Task.FromResult(runningTotal.ToString());
    }

    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> _)
    {
        var lines = ParseInput(input);
        var rightFrequency = lines.Right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        var runningTotal = lines.Left.Sum(l => rightFrequency.GetValueOrDefault(l, 0) * l);

        return Task.FromResult(runningTotal.ToString());
    }

    private (int[] Left, int[] Right) ParseInput(string input)
    {
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var parsed in InputHelpers.SplitLines(input).Select(InputHelpers.GetIntsFromLine))
        {
            leftList.Add(parsed[0]);
            rightList.Add(parsed[1]);
        }

        return (leftList.ToArray(), rightList.ToArray());
    }
}