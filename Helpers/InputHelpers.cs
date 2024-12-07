using System.Numerics;

namespace AdventOfCode2024.Helpers;

public static class InputHelpers
{
    public static IList<string> SplitLines(string input)
    {
        return input.Split('\n').Select(l => l.Trim()).ToArray();
    }

    public static IList<T> GetNumbersFromLine<T>(string line, char delimiter = ' ') where T : IParsable<T>
    {
        return line.Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(i => T.TryParse(i, null, out var v) ? v : throw new Exception($"Unable to parse {i}")).ToArray();
    }
    
    public static IList<int> GetNumbersFromLine(string line, char delimiter = ' ')
    {
        return GetNumbersFromLine<int>(line, delimiter);
    }
}