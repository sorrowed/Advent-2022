namespace Day1;

using System.Diagnostics;
using Common;

class Puzzle : PuzzleBase
{
    public Puzzle() : base(1) { }
    public override void Test() { }

    public override void Part1()
    {
        _sw.Restart();
        var first = Sums().First();
        Debug.Assert(first == 71506);
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {first} in {_sw.ElapsedMilliseconds} ms");
    }
    public override void Part2()
    {
        _sw.Restart();
        var firstThree = Sums().Take(3).Sum();
        Debug.Assert(firstThree == 209603);
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {firstThree} in {_sw.ElapsedMilliseconds} ms");
    }
    private static IEnumerable<int> Sums()
    {
        var chunks = new TextFile("Day1/Input.txt").ChunkBy(line => line == "");
        return chunks.Select(chunk => chunk.Select(line => int.Parse(line)).Sum())
            .OrderByDescending(sum => sum);
    }
}
