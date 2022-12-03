namespace Day1;

using System.Diagnostics;
using Common;

class Puzzle : IPuzzle
{
    public string Name { get { return "Day 1"; } }

    public void Test() { }

    public void Part1()
    {
        var first = Sums().First();
        Debug.Assert(first == 71506);

        Console.WriteLine($"{Name}:1 --> {first}");
    }
    public void Part2()
    {
        var firstThree = Sums().Take(3).Sum();
        Debug.Assert(firstThree == 209603);

        Console.WriteLine($"{Name}:2 --> {firstThree}");
    }
    private static IEnumerable<int> Sums()
    {
        var chunks = File.ReadAllLines("Day1/Input.txt").ChunkBy(line => line == "");
        return chunks.Select(chunk => chunk.Select(line => int.Parse(line)).Sum()).OrderByDescending(sum => sum);
    }
}
