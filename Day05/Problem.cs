using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day05;

public class Problem : DayProblem
{
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> progress)
    {
        var (rules, updates) = ParseInput(input);

        var runningSum = 0;

        foreach (var update in updates)
        {
            var isValid = CheckUpdateValidity(update, rules);

            if (isValid)
            {
                runningSum += update[update.Length/2];
            }
        }
        
        return Task.FromResult(runningSum.ToString());
    }

    /// <summary>
    /// This isn't exactly the best way to go about this problem, but it does work so.....
    /// </summary>
    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> progress)
    {
        var (rules, updates) = ParseInput(input);

        var runningSum = 0;

        foreach (var update in updates)
        {
            // It's fine, don't care
            if (CheckUpdateValidity(update, rules)) continue;
            
            for (var circuitBreak = 0; circuitBreak < 1000; circuitBreak++)
            {
                var stable = true;
                for (var i = 1; i < update.Length; i++)
                {
                    var stop = false;
                    foreach (var rule in rules.Where(r => r.A == update[i]))
                    {
                        var idxB = Array.IndexOf(update, rule.B);
                        if (idxB > i || idxB == -1) continue;
                        // Swap
                        (update[i - 1], update[i]) = (update[i], update[i - 1]);
                        stable = false;
                        stop = true;
                        break;
                    }

                    if (stop) break;
                }

                if (stable) break;
            }

            runningSum += update[update.Length / 2];
        }
        
        return Task.FromResult(runningSum.ToString());
    }

    private static bool CheckUpdateValidity(int[] update, OrderingRule[] rules)
    {
        var isValid = true;
        var relevantRules = rules.Where(r => update.Contains(r.A) && update.Contains(r.B));
        foreach (var rule in relevantRules)
        {
            var idxA = Array.IndexOf(update, rule.A);
            var idxB = Array.IndexOf(update, rule.B);

            if (idxA > idxB)
            {
                isValid = false;
                break;
            }
        }

        return isValid;
    }

    private static (OrderingRule[] rules, int[][] updates) ParseInput(string input)
    {
        var split = input.Split("\n\n");
        var rules = InputHelpers.SplitLines(split[0]).Select(l =>
        {
            var ints = InputHelpers.GetNumbersFromLine(l, '|');
            return new OrderingRule(ints[0], ints[1]);
        }).ToArray();
        var updates = InputHelpers.SplitLines(split[1]).Select(l => InputHelpers.GetNumbersFromLine(l, ',').ToArray())
            .ToArray();

        return (rules, updates);
    }

    private record OrderingRule(int A, int B);
}