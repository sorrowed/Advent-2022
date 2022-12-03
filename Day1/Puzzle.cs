namespace Day1;

using Common;

class Puzzle : IPuzzle
{
    public string Name { get { return "Day 1"; } }

    public void Test() { }

    public void Part1()
    {
        Console.WriteLine($"{Name}:1 --> {Sums().First()}");
    }
    public void Part2()
    {
        Console.WriteLine($"{Name}:2 --> {Sums().Take(3).Sum()}");
    }
    private static IEnumerable<int> Sums()
    {
        var chunks = File.ReadAllLines("Day1/Input.txt").ChunkBy(line => line == "");
        return chunks.Select(chunk => chunk.Select(line => int.Parse(line)).Sum()).OrderByDescending(sum => sum);
    }
}
