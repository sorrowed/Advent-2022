namespace Day8;

using System.Diagnostics;
using Common;

class Forest
{

    private readonly long[][] _heightMap;

    public Forest(long[][] heightMap)
    {
        _heightMap = heightMap;
    }

    private int Height { get { return _heightMap.GetLength(0); } }
    private int Width(int y) { return _heightMap[y].GetLength(0); }

    public int VisibleTrees()
    {
        int visible = 0;
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width(y); ++x)
            {
                if (IsVisibleFromOutside(x, y))
                {
                    ++visible;
                }
            }
        }
        return visible;
    }

    public long MaxScenicScore()
    {
        long result = 0;
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width(y); ++x)
            {
                result = Math.Max(result, ScenicScore(x, y));
            }
        }
        return result;
    }

    private bool IsVisibleFromOutside(int x, int y)
    {
        return
            IsVisibleFromLeft(x, y) ||
            IsVisibleFromRight(x, y) ||
            IsVisibleFromTop(x, y) ||
            IsVisibleFromBottom(x, y);
    }

    private bool IsVisibleFromLeft(int x, int y)
    {
        bool visible = true;
        for (int toTheLeft = x - 1; visible && toTheLeft >= 0; --toTheLeft)
        {
            visible &= (_heightMap[y][toTheLeft] < _heightMap[y][x]);
        }
        return visible;
    }


    private bool IsVisibleFromRight(int x, int y)
    {
        bool visible = true;
        for (int toTheRight = x + 1; visible && toTheRight < Width(y); ++toTheRight)
        {
            visible &= (_heightMap[y][toTheRight] < _heightMap[y][x]);
        }
        return visible;
    }

    private bool IsVisibleFromTop(int x, int y)
    {
        bool visible = true;

        for (int toTheTop = y - 1; visible && toTheTop >= 0; --toTheTop)
        {
            visible &= (_heightMap[toTheTop][x] < _heightMap[y][x]);
        }
        return visible;
    }

    private bool IsVisibleFromBottom(int x, int y)
    {
        bool visible = true;

        for (int toTheBottom = y + 1; visible && toTheBottom < Height; ++toTheBottom)
        {
            visible &= (_heightMap[toTheBottom][x] < _heightMap[y][x]);
        }

        return visible;
    }

    private long ScenicScore(int x, int y)
    {
        return IsOnEdge(x, y) ? 0 :
            ViewingDistanceToLeft(x, y) *
            ViewingDistanceToRight(x, y) *
            ViewingDistanceToTop(x, y) *
            ViewingDistanceToBottom(x, y);
    }


    private long ViewingDistanceToLeft(int x, int y)
    {
        long distance = 1;
        for (int toLeft = x - 1; toLeft > 0; --toLeft)
        {
            if (_heightMap[y][toLeft] < _heightMap[y][x])
            {
                ++distance;
            }
            else
            {
                break;
            }
        }
        return distance;
    }
    private long ViewingDistanceToRight(int x, int y)
    {
        long distance = 1;
        for (int toRight = x + 1; toRight < Width(y) - 1; ++toRight)
        {
            if (_heightMap[y][toRight] < _heightMap[y][x])
            {
                ++distance;
            }
            else
            {
                break;
            }
        }
        return distance;
    }

    private long ViewingDistanceToTop(int x, int y)
    {
        long distance = 1;
        for (int toTop = y - 1; toTop > 0; --toTop)
        {
            if (_heightMap[toTop][x] < _heightMap[y][x])
            {
                ++distance;
            }
            else
            {
                break;
            }
        }
        return distance;
    }

    private long ViewingDistanceToBottom(int x, int y)
    {
        long distance = 1;
        for (int toBottom = y + 1; toBottom < Height - 1; ++toBottom)
        {
            if (_heightMap[toBottom][x] < _heightMap[y][x])
            {
                ++distance;
            }
            else
            {
                break;
            }
        }
        return distance;
    }
    private bool IsOnEdge(int x, int y)
    {
        return x == 0 || y == 0 || x >= Width(y) - 1 || y >= Height - 1;
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(8) { }

    public override void Test()
    {
        string[] input =
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };

        var forest = new Forest(HeightMap(input));

        Debug.Assert(forest.VisibleTrees() == 21);

        Debug.Assert(forest.MaxScenicScore() == 8);
    }

    public override void Part1()
    {
        var heightmap = HeightMap(new TextFile("Day8/Input.txt"));

        _sw.Restart();
        var visible = new Forest(heightmap).VisibleTrees();
        Debug.Assert(visible == 1825);
        _sw.Stop();

        // Input2: Debug.Assert(visible == 3319);
        // Input3: Debug.Assert(visible == 27476);


        Console.WriteLine($"{Name}:1 --> {visible} in {_sw.ElapsedMilliseconds} ms");
    }

    public override void Part2()
    {
        var heightmap = HeightMap(new TextFile("Day8/Input.txt"));

        _sw.Restart();
        var score = new Forest(heightmap).MaxScenicScore();
        Debug.Assert(score == 235200);
        _sw.Stop();


        // Input2: Debug.Assert(visible == 672888);
        // Input3: Debug.Assert(score == 32065007130);

        Console.WriteLine($"{Name}:2 --> {score} in {_sw.ElapsedMilliseconds} ms");
    }

    private static long[][] HeightMap(IEnumerable<string> input)
    {
        return input.Select(line => line.ToCharArray().Select(c => (long)(c - '0')).ToArray()).ToArray();
    }
}
