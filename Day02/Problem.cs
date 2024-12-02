using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day02;

public class Problem : DayProblem
{
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> _)
    {
        var reports = ParseInput(input);

        var numSafe = reports.Count(IsReportSafe);

        return Task.FromResult(numSafe.ToString());
    }

    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> _)
    {
        var reports = ParseInput(input);

        var numSafe = 0;

        foreach (var report in reports)
        {
            var isSafe = IsReportSafe(report);
            if (isSafe)
            {
                numSafe += 1;
                continue;
            }
            
            var newReport = new int[report.Length - 1];
            for (var i = 0; i < report.Length; i++)
            {
                // Create a copy of the report into `newReport` except for the item we want to test without
                Array.Copy(report, 0, newReport, 0, i);
                Array.Copy(report, i + 1, newReport, i, report.Length - i - 1);

                if (IsReportSafe(newReport))
                {
                    numSafe += 1;
                    break;
                }
            }
        }

        return Task.FromResult(numSafe.ToString());
    }

    private static List<int[]> ParseInput(string input)
    {
        return InputHelpers.SplitLines(input).Select(l => InputHelpers.GetIntsFromLine(l).ToArray()).ToList();
    }

    private static bool IsReportSafe(int[] report)
    {
        bool? isIncreasing = null;
        var isSafe = true;

        for (var i = 0; i < report.Length - 1; i++)
        {
            var current = report[i];
            var next = report[i + 1];
            var diff = Math.Abs(current - next);

            if (diff is < 1 or > 3)
            {
                isSafe = false;
                break;
            }

            if (isIncreasing is null)
            {
                isIncreasing = current < next;
            }
            else
            {
                // Bail if trend doesn't match what we expect
                if ((isIncreasing.Value && current > next) || (!isIncreasing.Value && current < next))
                {
                    isSafe = false;
                    break;
                }
            }
        }

        return isSafe;
    }
}