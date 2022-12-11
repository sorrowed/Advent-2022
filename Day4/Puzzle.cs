namespace Day4;

using System.Diagnostics;
using Common;

struct Range
{
    public int Begin { get; set; }
    public int End { get; set; }

    public static bool Contains(Range first, Range second)
    {
        return first.Begin <= second.Begin && first.End >= second.End;
    }

    public static bool Overlaps(Range first, Range second)
    {
        return first.Begin <= second.End && first.End >= second.Begin;
    }
}

struct Pair
{
    public Range First { get; set; }
    public Range Second { get; set; }

    public bool IsContaining
    {
        get { return Range.Contains(First, Second) || Range.Contains(Second, First); }
    }
    public bool IsOverlapping
    {
        get { return Range.Overlaps(First, Second); }
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(4) { }
    public override void Test()
    {
        string[] input =
        {
            "2-4,6-8",
            "2-3,4-5",
            "5-7,7-9",
            "2-8,3-7",
            "6-6,4-6",
            "2-6,4-8",
        };

        var p1 = Parse(input[0]);
        Debug.Assert(p1.First.Begin == 2);
        Debug.Assert(p1.First.End == 4);
        Debug.Assert(p1.Second.Begin == 6);
        Debug.Assert(p1.Second.End == 8);

        var pairs1 = input.Select(line => Parse(line)).Where(pair => pair.IsContaining).ToArray();
        Debug.Assert(pairs1.Length == 2);
        Debug.Assert(pairs1[0].First.Begin == 2 && pairs1[0].First.End == 8);
        Debug.Assert(pairs1[0].Second.Begin == 3 && pairs1[0].Second.End == 7);
        Debug.Assert(pairs1[1].First.Begin == 6 && pairs1[1].First.End == 6);
        Debug.Assert(pairs1[1].Second.Begin == 4 && pairs1[1].Second.End == 6);

        var pairs2 = input.Select(line => Parse(line)).Where(pair => pair.IsOverlapping);
        Debug.Assert(pairs2.Count() == 4);
    }

    public override void Part1()
    {
        _sw.Restart();
        var count = new TextFile("Day4/Input.txt")
            .Select(Parse)
            .Count(pair => pair.IsContaining);

        Debug.Assert(count == 550);
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {count} in {_sw.ElapsedMilliseconds} ms");
    }

    public override void Part2()
    {
        _sw.Restart();
        var count = new TextFile("Day4/Input.txt")
            .Select(Parse)
            .Count(pair => pair.IsOverlapping);

        Debug.Assert(count == 931);
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {count} in {_sw.ElapsedMilliseconds} ms");
    }

    private static Pair Parse(string input)
    {
        int[] s = input.Split(new char[] { '-', ',' }).Select(c => int.Parse(c)).ToArray();
        Debug.Assert(s.Length == 4);

        return new Pair
        {
            First = new Range { Begin = s[0], End = s[1] },
            Second = new Range { Begin = s[2], End = s[3] }
        };
    }
}
