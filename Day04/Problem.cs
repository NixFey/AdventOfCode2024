using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day04;

public class Problem : DayProblem
{
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> progress)
    {
        var parsed = ParseInput(input);

        var numMatches = 0;
        
        for (var i = 0; i < parsed.GetLength(0); i++)
        {
            for (var j = 0; j < parsed.GetLength(1); j++)
            {
                if (TryGetChar(parsed, i, j) != 'X') continue;
                
                foreach (var direction in Enum.GetValues<Direction>())
                {
                    if (TryGetCharDirection(parsed, i, j, direction)            != 'M') continue;
                    if (TryGetCharDirection(parsed, i, j, direction, 2) != 'A') continue;
                    if (TryGetCharDirection(parsed, i, j, direction, 3) != 'S') continue;
                    numMatches += 1;
                }
            }
        }
        
        return Task.FromResult(numMatches.ToString());
    }

    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> progress)
    {
        var parsed = ParseInput(input);

        var numMatches = 0;
        
        for (var i = 0; i < parsed.GetLength(0); i++)
        {
            for (var j = 0; j < parsed.GetLength(1); j++)
            {
                if (TryGetChar(parsed, i, j) != 'A') continue;
                
                var hasA = (TryGetCharDirection(parsed, i, j, Direction.NW) == 'M' && TryGetCharDirection(parsed, i, j, Direction.SE) == 'S') ||
                           (TryGetCharDirection(parsed, i, j, Direction.NW) == 'S' && TryGetCharDirection(parsed, i, j, Direction.SE) == 'M');
                var hasB = (TryGetCharDirection(parsed, i, j, Direction.NE) == 'M' && TryGetCharDirection(parsed, i, j, Direction.SW) == 'S') ||
                           (TryGetCharDirection(parsed, i, j, Direction.NE) == 'S' && TryGetCharDirection(parsed, i, j, Direction.SW) == 'M');

                if (hasA && hasB) numMatches += 1;
            }
        }
        
        return Task.FromResult(numMatches.ToString());
    }

    private static char? TryGetChar(char[,] arr, int y, int x)
    {
        if (y < arr.GetLowerBound(0) || y > arr.GetUpperBound(0)) return null;
        if (x < arr.GetLowerBound(1) || x > arr.GetUpperBound(1)) return null;

        return arr[y, x];
    }

    private static char? TryGetCharDirection(char[,] arr, int y, int x, Direction dir, int numSteps = 1)
    {
        var transform = DirectionToIndexTransform[dir];
        return TryGetChar(arr, y + transform.y * numSteps, x + transform.x * numSteps);
    }

    private static char[,] ParseInput(string input)
    {
        var lines = InputHelpers.SplitLines(input);
        var arr = new char[lines.Count, lines.First().Length];
        for (var i = 0; i < arr.GetLength(0); i++)
        {
            for (var j = 0; j < arr.GetLength(1); j++)
            {
                arr[i, j] = lines[i][j];
            }
        }

        return arr;
    }

    // ReSharper disable InconsistentNaming
    private enum Direction
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }
    // ReSharper restore InconsistentNaming

    private static readonly Dictionary<Direction, (int y, int x)> DirectionToIndexTransform =
        new Dictionary<Direction, (int x, int y)>
        {
            { Direction.N, (-1, 0) },
            { Direction.NE, (-1, 1) },
            { Direction.E, (0, 1) },
            { Direction.SE, (1, 1) },
            { Direction.S, (1, 0) },
            { Direction.SW, (1, -1) },
            { Direction.W, (0, -1) },
            { Direction.NW, (-1, -1) },
        };
}