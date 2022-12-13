namespace Day12;

using System.Diagnostics;
using Common;

class Tile
{
    public char Id { get; private init; }
    public int X { get; private init; }
    public int Y { get; private init; }

    public Tile(char id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public int Height { get { return CalculateHeight(Id); } }

    public int Cost { get; set; } = 0;

    public Tile? Parent { get; set; }

    public int TotalCost(Tile to) { return Cost + Distance(this, to); }

    public bool IsTile(Tile other)
    {
        return X == other.X && Y == other.Y;
    }
    private static int CalculateHeight(char c)
    {
        return c switch
        {
            'S' => CalculateHeight('a'),
            'E' => CalculateHeight('z'),
            >= 'a' and <= 'z' => c - 'a',
            _ => throw new ApplicationException()
        };
    }

    public static int Distance(Tile from, Tile to)
    {
        return Math.Abs(to.X - from.X) + Math.Abs(to.Y - from.Y);
    }
}

public class NoPathFoundException : Exception { }

class Map
{
    public List<Tile> Tiles { get; private init; }
    public Map(List<Tile> tiles)
    {
        Tiles = tiles;
    }

    public void Reset()
    {
        foreach (var tile in Tiles)
        {
            tile.Cost = 0;
            tile.Parent = null;
        }
    }
    public List<Tile> Neighbors(Tile tile)
    {
        var result = Tiles.Where(n =>
            ((n.Y == tile.Y && Math.Abs(n.X - tile.X) == 1) ||
            (n.X == tile.X && Math.Abs(n.Y - tile.Y) == 1)) &&
            n.Height - tile.Height <= 1);

        return result.Select(neighbor => new Tile(neighbor.Id, neighbor.X, neighbor.Y) { Cost = tile.Cost + 1, Parent = tile }).ToList();
    }

    public Tile Start { get { return Tiles.First(node => node.Id == 'S'); } }
    public Tile End { get { return Tiles.First(node => node.Id == 'E'); } }

    public List<Tile> Search(Tile from, Tile to)
    {
        List<Tile> unvisited = new() { from };

        List<Tile> visited = new();

        Tile? current = null;
        while (unvisited.Count > 0)
        {
            current = unvisited.MinBy(node => node.TotalCost(to))!;

            unvisited.Remove(current);
            visited.Add(current);

            if (current.IsTile(to))
                break;

            foreach (var neighbor in Neighbors(current))
            {
                if (visited.Any(node => node.IsTile(neighbor)))
                    continue;

                var existing = unvisited.Find(node => node.IsTile(neighbor));
                if (existing != null)
                {
                    if (existing.TotalCost(to) > current!.TotalCost(to))
                    {
                        unvisited.Remove(existing);
                        unvisited.Add(neighbor);
                    }
                }
                else
                {
                    unvisited.Add(neighbor);
                }
            }

        }

        if (!current!.IsTile(to))
            throw new NoPathFoundException();

        List<Tile> result = new();
        while (current != null)
        {
            result.Add(current);
            current = current.Parent;
        }
        result.Reverse();

        return result;
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(12) { }

    private static Map BuildMap(IEnumerable<string> input)
    {
        return new Map(input.SelectMany((line, y) => line.Select((c, x) => new Tile(c, x, y))).ToList());
    }

    public override void Test()
    {
        string[] input =
        {
            "Sabqponm",
            "abcryxxl",
            "accszExk",
            "acctuvwj",
            "abdefghi",
        };

        var map = BuildMap(input);
        var path = map.Search(map.Start, map.End);
        Debug.Assert(path.Count - 1 == 31);
    }

    public override void Part1()
    {
        var map = BuildMap(new TextFile("Day12/Input.txt"));

        _sw.Restart();
        var path = map.Search(map.Start, map.End);
        Debug.Assert(path.Count - 1 == 520);
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {path.Count - 1} in {_sw.ElapsedMilliseconds} milliseconds");
    }

    public override void Part2()
    {
        var map = BuildMap(new TextFile("Day12/Input.txt"));
        _sw.Restart();

        var starts = map.Tiles.Where(tile => tile.Id == 'a' && Tile.Distance(tile, map.End) <= 520).OrderBy(tile => Tile.Distance(tile, map.End)).ToList();

        int distance = 508;
        foreach (var start in starts)
        {
            //     if (Tile.Distance(start, map.End) < distance)
            //     {
            //         try
            //         {
            //             var path = map.Search(start, map.End);
            //             distance = Math.Min(distance, path.Count - 1);
            //         }
            //         catch (NoPathFoundException)
            //         {
            //             // pass
            //         }
            //         map.Reset();
            //     }
        }

        Debug.Assert(distance == 508);
        _sw.Stop();
        Console.WriteLine($"{Name}:2 --> {distance} in {_sw.ElapsedMilliseconds} milliseconds");
    }
}
