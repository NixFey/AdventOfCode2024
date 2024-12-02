namespace AdventOfCode2024.Helpers;

public static class InputHelpers
{
    public static List<string> SplitLines(string input)
    {
        return input.Split('\n').Select(l => l.Trim()).ToList();
    }

    public static List<int> GetIntsFromLine(string line)
    {
        return line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()
            .Select(i => int.TryParse(i, out var v) ? v : int.MinValue).ToList();
    }
}