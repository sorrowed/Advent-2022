namespace Day9;

using System.Diagnostics;
using Common;


record Vector(int X, int Y)
{
    public bool IsTouching
    {
        get { return Math.Abs(X) <= 1 && Math.Abs(Y) <= 1; }
    }

    public Vector Distance(Vector other)
    {
        return new Vector(other.X - X, other.Y - Y);
    }
}

record Move(char Direction, int Count);

class Rope
{
    readonly Vector _bl;
    readonly Vector _tr;
    readonly bool _verbose;
    public List<Vector> Knots { get; private set; }

    public Vector Head
    {
        get { return Knots[0]; }
        private set { Knots[0] = value; }
    }

    public Vector Tail
    {
        get { return Knots[^1]; }
        private set { Knots[^1] = value; }
    }

    public HashSet<Vector> TailPositions { get; private init; } = new();

    public Rope(int knots, Vector bl, Vector tr, bool verbose = false)
    {
        Debug.Assert(knots >= 2);

        _bl = bl;
        _tr = tr;
        _verbose = verbose;

        Knots = Enumerable.Repeat(new Vector(0, 0), knots).ToList();

        TailPositions.Add(Tail);

        Print(Knots);
    }

    public void Move(Move move)
    {
        for (int count = 0; count < move.Count; ++count)
        {
            Move(move.Direction);

            Follow();

            Validate();

            TailPositions.Add(Tail);

            Print(Knots);
        }
    }

    private void Move(char direction)
    {
        Head = direction switch
        {
            'L' => Head with { X = Head.X - 1 },
            'D' => Head with { Y = Head.Y - 1 },
            'R' => Head with { X = Head.X + 1 },
            'U' => Head with { Y = Head.Y + 1 },
            _ => throw new InvalidProgramException(),
        };
    }

    public void Follow()
    {
        for (int i = 1; i < Knots.Count; ++i)
        {
            var leader = Knots[i - 1];
            var follower = Knots[i];

            var distance = follower.Distance(leader);
            if (distance.IsTouching)
                continue;

            if (follower.X == leader.X)
            {
                follower = new Vector(follower.X, follower.Y + Math.Sign(distance.Y)); // Cannot move more than 1
            }
            else if (follower.Y == leader.Y)
            {
                follower = new Vector(follower.X + Math.Sign(distance.X), follower.Y); // Cannot move more than 1
            }
            else // Move diagonally
            {
                follower = new Vector(follower.X + Math.Sign(distance.X), follower.Y + Math.Sign(distance.Y));
            }
            Knots[i] = follower;
        }
    }

    private void Validate()
    {
        for (int i = 1; i < Knots.Count; ++i)
        {
            Debug.Assert(Knots[i].Distance(Knots[i - 1]).IsTouching);
        }
    }

    private void Print(List<Vector> positions)
    {
        if (!_verbose)
            return;

        Console.Out.WriteLine(new string('-', 80));
        for (int y = _tr.Y - 1; y >= _bl.Y; --y)
        {
            for (int x = _bl.X; x <= _tr.X; ++x)
            {
                int knotIndex = positions.IndexOf(new Vector(x, y));
                if (knotIndex != -1)
                {
                    Console.Out.Write((char)('0' + knotIndex));
                }
                else
                {
                    Console.Out.Write('.');
                }
            }
            Console.Out.WriteLine();
        }
        Console.Out.WriteLine(new string('-', 80));
        Console.Out.Write($"\x1b[{2 + _tr.Y - _bl.Y}A");
        Thread.Sleep(250);
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(9) { }

    public override void Test()
    {
        string[] input =
        {
            "R 4",
            "U 4",
            "L 3",
            "D 1",
            "R 4",
            "D 1",
            "L 5",
            "R 2",
        };

        {
            var rope = new Rope(2, new Vector(0, 0), new Vector(6, 6));

            foreach (var move in Moves(input))
            {
                rope.Move(move);
            }

            Debug.Assert(rope.Head == new Vector(2, 2));
            Debug.Assert(rope.Tail == new Vector(1, 2));
            Debug.Assert(rope.TailPositions.Count == 13);
        }
        {
            var rope = new Rope(10, new Vector(0, 0), new Vector(6, 6));
            foreach (var move in Moves(input))
            {
                rope.Move(move);
            }

            Debug.Assert(rope.Head == new Vector(2, 2));
            Debug.Assert(rope.Tail == new Vector(0, 0));
            Debug.Assert(rope.TailPositions.Count == 1);
        }

        string[] input2 =
        {
            "R 5",
            "U 8",
            "L 8",
            "D 3",
            "R 17",
            "D 10",
            "L 25",
            "U 20",
        };
        {
            var rope = new Rope(10, new Vector(-11, -5), new Vector(14, 15));
            foreach (var move in Moves(input2))
            {
                rope.Move(move);
            }

            Debug.Assert(rope.Head == new Vector(-11, 15));
            Debug.Assert(rope.Tail == new Vector(-11, 6));
            Debug.Assert(rope.TailPositions.Count == 36);
        }
    }

    public override void Part1()
    {
        _sw.Restart();
        var rope = new Rope(2, new Vector(-20, -20), new Vector(20, 20));
        foreach (var move in Moves(new TextFile("Day9/Input.txt")))
        {
            rope.Move(move);
        }

        Debug.Assert(rope.TailPositions.Count == 5683);
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {rope.TailPositions.Count} in {_sw.ElapsedMilliseconds} ms");
    }

    public override void Part2()
    {
        _sw.Restart();
        var rope = new Rope(10, new Vector(-20, -20), new Vector(20, 20));
        foreach (var move in Moves(new TextFile("Day9/Input.txt")))
        {
            rope.Move(move);
        }

        Debug.Assert(rope.TailPositions.Count == 2372);
        _sw.Stop();
        Console.WriteLine($"{Name}:2 --> {rope.TailPositions.Count} in {_sw.ElapsedMilliseconds} ms");
    }

    private static IEnumerable<Move> Moves(IEnumerable<string> input)
    {
        return input.Select(line => line.Split(' ')).Select(tokens => new Move(tokens[0].First(), int.Parse(tokens[1])));
    }
}
