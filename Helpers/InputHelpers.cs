namespace AdventOfCode2024.Helpers;

public static class InputHelpers
{
    public static List<string> SplitLines(string input)
    {
        return input.Split('\n').Select(l => l.Trim()).ToList();
    }

    public static List<int> GetIntsFromLine(string line, char delimiter = ' ')
    {
        return line.Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()
            .Select(i => int.TryParse(i, out var v) ? v : int.MinValue).ToList();
    }
    
    public static ulong[] GetULongsFromLine(string line, char delimiter = ' ')
    {
        return line.Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()
            .Select(i => ulong.TryParse(i, out var v) ? v : ulong.MaxValue).ToArray();
    }
}