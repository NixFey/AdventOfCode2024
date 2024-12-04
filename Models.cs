using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024;

public class Day(
    int dayNumber,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type dayProblem
)
{
    public int DayNumber => dayNumber;
    public DayProblem GetDayProblem()
    {
        return (DayProblem)Activator.CreateInstance(dayProblem)!;
    }
}

public enum Part
{
    Part1,
    Part2,
    Both
}