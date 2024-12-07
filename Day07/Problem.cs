using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day07;

public class Problem : DayProblem
{
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> progress)
    {
        var parsed = ParseInput(input);

        var runningTotal = 0UL;
        
        foreach (var (idx, line) in parsed.Index())
        {
            progress.Report(new SolveProgress
            {
                CurrentProgress = idx + 1,
                TotalProgress = parsed.Length
            });
            
            if (RecursivelyFindResult(line.nums[0], line.test, line.nums[1..]))
            {
                runningTotal += line.test;
            }
        }

        return Task.FromResult(runningTotal.ToString());

        bool RecursivelyFindResult(ulong current, ulong goal, ulong[] nums)
        {
            if (nums.Length == 0) return current == goal;
            
            var plus = RecursivelyFindResult(current + nums[0], goal, nums[1..]);
            var mult = RecursivelyFindResult(current * nums[0], goal, nums[1..]);

            return plus || mult;
        }
    }

    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> progress)
    {
        var parsed = ParseInput(input);

        var runningTotal = 0UL;
        
        foreach (var (idx, line) in parsed.Index())
        {
            progress.Report(new SolveProgress
            {
                CurrentProgress = idx + 1,
                TotalProgress = parsed.Length
            });
            
            if (RecursivelyFindResult(line.nums[0], line.test, line.nums[1..]))
            {
                runningTotal += line.test;
            }
        }

        return Task.FromResult(runningTotal.ToString());

        bool RecursivelyFindResult(ulong current, ulong goal, ulong[] nums)
        {
            if (nums.Length == 0) return current == goal;
            
            var plus = RecursivelyFindResult(current + nums[0], goal, nums[1..]);
            var mult = RecursivelyFindResult(current * nums[0], goal, nums[1..]);
            var concat = RecursivelyFindResult(ulong.Parse($"{current}{nums[0]}"), goal, nums[1..]);

            return plus || mult || concat;
        }
    }

    private static (ulong test, ulong[] nums)[] ParseInput(string input)
    {
        return InputHelpers.SplitLines(input).Select(line =>
        {
            var colonSplit = line.Split(':');
            return (ulong.Parse(colonSplit[0]), InputHelpers.GetULongsFromLine(colonSplit[1]).ToArray());
        }).ToArray();
    }
}