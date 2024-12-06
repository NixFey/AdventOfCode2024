using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day06;

/// <summary>
/// Gonna be honest, this solution sucks but I don't really have the bandwidth today to clean it up
/// </summary>
public class Problem : DayProblem
{
    // Uh... this doesn't work lol and idk why
    public override Task<string> SolvePart1(string input, IProgress<SolveProgress> progress)
    {
        //throw new NotImplementedException("This solution doesn't seem to produce the right answer?");
        
        var map = ParseInput(input);

        for (var circuitBreaker = 0; circuitBreaker < 10_000; circuitBreaker++)
        {
            if (!map.Step())
            {
                // We left the board, count up the visited squares
                return Task.FromResult(map.NumVisitedSquares().ToString());
            }
        }
        
        throw new ApplicationException("Circuit breaker tripped, infinite loop?");
    }

    // Naive solution, just see which options time out :D
    public override Task<string> SolvePart2(string input, IProgress<SolveProgress> progress)
    {
        var map = ParseInput(input);
        var numWithLoops = 0;

        for (var i = 0; i <= map.Squares.GetUpperBound(0); i++)
        {
            for (var j = 0; j <= map.Squares.GetUpperBound(1); j++)
            {
                if (map.Squares[i, j] != SquareStatus.Empty) continue;
                
                var newMap = new Map
                {
                    GuardPosition = map.GuardPosition,
                    GuardDirection = map.GuardDirection,
                    Squares = (SquareStatus[,])map.Squares.Clone()
                };

                newMap.Squares[i, j] = SquareStatus.Obstacle;

                var gotOut = false;
                for (var circuitBreaker = 0; circuitBreaker < 10_000; circuitBreaker++)
                {
                    if (!newMap.Step())
                    {
                        // We left the board, count up the visited squares
                        gotOut = true;
                        break;
                    }
                }

                if (!gotOut)
                {
                    //Console.WriteLine($"{i}, {j}");
                    numWithLoops += 1;
                }
            }
        }
        
        return Task.FromResult(numWithLoops.ToString());
    }

    private static Map ParseInput(string input)
    {
        var map = new Map
        {
            GuardDirection = Direction.N
        };
        
        var lines = InputHelpers.SplitLines(input);
        map.Squares = new SquareStatus[lines.Count, lines.First().Length];
        for (var i = 0; i < map.Squares.GetLength(0); i++)
        {
            for (var j = 0; j < map.Squares.GetLength(1); j++)
            {
                if (lines[i][j] == '^')
                {
                    map.GuardPosition = new Coordinate(i, j);
                }
                
                map.Squares[i, j] = lines[i][j] switch
                {
                    '#' => SquareStatus.Obstacle,
                    _ => SquareStatus.Empty
                };
            }
        }

        return map;
    }

    private class Map
    {
        public Coordinate GuardPosition { get; set; } = null!;
        public Direction GuardDirection { get; set; }
        public SquareStatus[,] Squares { get; set; } = null!;

        public bool Step()
        {
            var nextSquare = GuardDirection switch
            {
                Direction.N => GuardPosition with { Y = GuardPosition.Y - 1 },
                Direction.E => GuardPosition with { X = GuardPosition.X + 1 },
                Direction.S => GuardPosition with { Y = GuardPosition.Y + 1 },
                Direction.W => GuardPosition with { X = GuardPosition.X - 1 },
                _ => throw new ArgumentOutOfRangeException()
            };

            if (nextSquare.Y < Squares.GetLowerBound(0) || nextSquare.Y > Squares.GetUpperBound(0) ||
                nextSquare.X < Squares.GetLowerBound(1) || nextSquare.X > Squares.GetUpperBound(1))
            {
                return false;
            }

            var nextSquareStatus = Squares[nextSquare.Y, nextSquare.X];

            if (nextSquareStatus == SquareStatus.Obstacle)
            {
                // Turn clockwise
                GuardDirection = (Direction)((int)(GuardDirection + 1) % Enum.GetValues<Direction>().Length);
            }
            else
            {
                Squares[nextSquare.Y, nextSquare.X] = SquareStatus.Visited;
                GuardPosition = nextSquare;
            }
            
            return true;
        }

        public int NumVisitedSquares()
        {
            var total = 0;

            for (var i = 0; i <= Squares.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= Squares.GetUpperBound(1); j++)
                {
                    if (Squares[i, j] == SquareStatus.Visited)
                    {
                        total += 1;
                    }
                }
            }

            return total;
        }
    }

    private record Coordinate(int Y, int X);

    private enum SquareStatus
    {
        Empty,
        Obstacle,
        Visited
    }

    private enum Direction
    {
        N,
        E,
        S,
        W
    }
}