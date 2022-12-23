namespace Day14;

using System.Diagnostics;
using Common;

record Coordinate(int X, int Y)
{
    public static Coordinate Parse(string chunk)
    {
        var tokens = chunk.Split(',');
        return new Coordinate(int.Parse(tokens[0]), int.Parse(tokens[1]));
    }
}

record Segment(Coordinate Begin, Coordinate End);

class Path
{
    public List<Segment> Segments { get; private init; } = new();

    public static Path FromPoints(IEnumerable<Coordinate> points)
    {
        Path result = new();
        Coordinate? previous = null;
        foreach (var point in points)
        {
            if (previous != null)
            {
                result.Segments.Add(new Segment(previous!, point));
            }
            previous = point;
        }
        return result;
    }
}

class Cave : Dictionary<Coordinate, char>
{
    public Coordinate Sand { get; private init; }
    public Coordinate? TopLeft { get; set; }
    public Coordinate? BottomRight { get; set; }

    public Cave(Coordinate sand)
    {
        Sand = sand;
        Add(Sand, '+');
    }

    public void AddRock(Path path)
    {
        foreach (var segment in path.Segments)
        {
            for (int x = Math.Min(segment.Begin.X, segment.End.X); segment.Begin.Y == segment.End.Y && x <= Math.Max(segment.Begin.X, segment.End.X); ++x)
            {
                TryAdd(new Coordinate(x, segment.Begin.Y), '#');
            }
            for (int y = Math.Min(segment.Begin.Y, segment.End.Y); segment.Begin.X == segment.End.X && y <= Math.Max(segment.Begin.Y, segment.End.Y); ++y)
            {
                TryAdd(new Coordinate(segment.Begin.X, y), '#');
            }
        }
    }

    public bool AddSand(bool limits)
    {
        bool result = false;

        Coordinate sand = Sand with { };
        do
        {
            if (sand.X < TopLeft!.X || sand.X > BottomRight!.X)
            {
                if (!limits)
                {
                    TopLeft = TopLeft! with { X = Math.Min(sand.X, TopLeft.X) };
                    BottomRight = BottomRight! with { X = Math.Max(sand.X, BottomRight.X) };
                }
                else
                {
                    break;
                }
            }
            else if (sand.Y + 1 > BottomRight!.Y)
            {
                if (!limits)
                {
                    Add(sand, 'o');
                    result = true;
                }
                break;
            }
            else if (!Contains(sand.X, sand.Y + 1))
            {
                sand = sand with { X = sand.X, Y = sand.Y + 1 };
            }
            else if (!Contains(sand.X - 1, sand.Y + 1))
            {
                sand = sand with { X = sand.X - 1, Y = sand.Y + 1 };
            }
            else if (!Contains(sand.X + 1, sand.Y + 1))
            {
                sand = sand with { X = sand.X + 1, Y = sand.Y + 1 };
            }
            else
            {
                this[sand] = 'o';

                result = sand != Sand;

                break;
            }
        } while (true);

        return result;
    }

    public void Limits()
    {
        TopLeft = new Coordinate(this.MinBy(point => point.Key.X).Key.X, this.MinBy(point => point.Key.Y).Key.Y);
        BottomRight = new Coordinate(this.MaxBy(point => point.Key.X).Key.X, this.MaxBy(point => point.Key.Y).Key.Y);
    }

    public bool Contains(int x, int y)
    {
        return this.ContainsKey(new Coordinate(x, y));
    }

    public void Print()
    {
        for (int y = TopLeft!.Y; y <= BottomRight!.Y; ++y)
        {
            for (int x = TopLeft!.X; x <= BottomRight!.X; ++x)
            {
                if (this.TryGetValue(new Coordinate(x, y), out char c))
                {
                    Console.Out.Write(c);
                }
                else
                {
                    Console.Out.Write('.');
                }
            }
            Console.Out.WriteLine();
        }
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(14) { }


    public override void Test()
    {
        string[] input =
        {
            "498,4 -> 498,6 -> 496,6",
            "503,4 -> 502,4 -> 502,9 -> 494,9"
        };

        var cave = new Cave(new Coordinate(500, 0));
        cave.AddRock(Path.FromPoints(input[0].Split(" -> ").Select(Coordinate.Parse)));
        cave.AddRock(Path.FromPoints(input[1].Split(" -> ").Select(Coordinate.Parse)));
        cave.Limits();

        //cave.Print();

        while (cave.AddSand(true))
        {
            //cave.Print();
        }

        Debug.Assert(cave.Count(kvp => kvp.Value == 'o') == 24);
    }

    public override void Part1()
    {
        var cave = new Cave(new Coordinate(500, 0));
        foreach (var line in new TextFile("Day14/Input.txt"))
        {
            cave.AddRock(Path.FromPoints(line.Split(" -> ").Select(Coordinate.Parse)));
        }
        cave.Limits();

        _sw.Restart();
        while (cave.AddSand(true)) { };
        int sands = cave.Count(kvp => kvp.Value == 'o');

        _sw.Stop();
        //cave.Print();

        Debug.Assert(sands == 1513);

        Console.WriteLine($"{Name}:1 --> {sands} in {_sw.ElapsedMilliseconds} milliseconds");
    }

    public override void Part2()
    {
        var cave = new Cave(new Coordinate(500, 0));
        foreach (var line in new TextFile("Day14/Input.txt"))
        {
            cave.AddRock(Path.FromPoints(line.Split(" -> ").Select(Coordinate.Parse)));
        }
        cave.Limits();
        cave.BottomRight = cave.BottomRight! with { Y = cave.BottomRight.Y + 1 };

        _sw.Restart();
        while (cave.AddSand(false)) { }
        int sands = cave.Count(kvp => kvp.Value == 'o');
        _sw.Stop();
        //cave.Print();


        Debug.Assert(sands == 22646);

        Console.WriteLine($"{Name}:1 --> {sands} in {_sw.ElapsedMilliseconds} milliseconds");
    }
}
