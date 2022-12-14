namespace Day5;

using System.Diagnostics;
using Common;

struct Move
{
    public int HowMany { get; init; }
    public int From { get; init; }
    public int To { get; init; }

    public static Move Parse(string input)
    {
        string[] tokens = input.Replace(" ", "")
            .Replace("move", ",")
            .Replace("from", ",")
            .Replace("to", ",")
            .Split(",", StringSplitOptions.RemoveEmptyEntries);

        Debug.Assert(tokens.Length == 3);

        return new Move { HowMany = int.Parse(tokens[0]), From = int.Parse(tokens[1]), To = int.Parse(tokens[2]) };
    }
}

internal static class Crane9000
{
    public static void Apply(string[] stacks, Move move)
    {
        var from = stacks[move.From - 1];

        stacks[move.To - 1] += new string(from[^move.HowMany..].Reverse().ToArray()); // HowMany from the end up to the end
        stacks[move.From - 1] = from[0..^move.HowMany]; // From beginning to HowMany from the end
    }
};

internal static class Crane9001
{
    public static void Apply(string[] stacks, Move move)
    {
        var from = stacks[move.From - 1];

        stacks[move.To - 1] += from[^move.HowMany..]; // HowMany from the end up to the end
        stacks[move.From - 1] = from[0..^move.HowMany]; // From beginning to HowMany from the end
    }
};

class Puzzle : PuzzleBase
{
    public Puzzle() : base(5) { }
    public override void Test()
    {
        var stacks = new string[] { "ZN", "MCD", "P" };
        string[] input =
        {
            "move 1 from 2 to 1",
            "move 3 from 1 to 3",
            "move 2 from 2 to 1",
            "move 1 from 1 to 2",
        };

        var moves = input.Select(Move.Parse).ToArray();
        Debug.Assert(moves.Length == 4);
        Debug.Assert(moves[0].From == 2 && moves[0].To == 1 && moves[0].HowMany == 1);

        foreach (var move in moves)
        {
            Crane9000.Apply(stacks, move);
        }

        Debug.Assert(stacks[0].Length == 1 && stacks[0].Last() == 'C');
        Debug.Assert(stacks[1].Length == 1 && stacks[1].Last() == 'M');
        Debug.Assert(stacks[2].Length == 4 && stacks[2].Last() == 'Z');
    }

    public override void Part1()
    {
        _sw.Restart();
        var stacks = Stacks();

        foreach (var move in new TextFile("Day5/Input.txt").Select(Move.Parse))
        {
            Crane9000.Apply(stacks, move);
        }

        var topCrates = stacks.Aggregate("", (acc, stack) => acc + stack[^1]);

        Debug.Assert(topCrates == "FWSHSPJWM");
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {topCrates} in {_sw.ElapsedMilliseconds} ms");
    }

    public override void Part2()
    {
        _sw.Restart();
        var stacks = Stacks();

        foreach (var move in new TextFile("Day5/Input.txt").Select(Move.Parse))
        {
            Crane9001.Apply(stacks, move);
        }
        var topCrates = stacks.Aggregate("", (acc, stack) => acc + stack[^1]);

        Debug.Assert(topCrates == "PWPWHGFZS");
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {topCrates} in {_sw.ElapsedMilliseconds} ms");
    }


    /*
        [C]         [S] [H]
        [F] [B]     [C] [S]     [W]
        [B] [W]     [W] [M] [S] [B]
        [L] [H] [G] [L] [P] [F] [Q]
        [D] [P] [J] [F] [T] [G] [M] [T]
        [P] [G] [B] [N] [L] [W] [P] [W] [R]
        [Z] [V] [W] [J] [J] [C] [T] [S] [C]
        [S] [N] [F] [G] [W] [B] [H] [F] [N]
        1   2   3   4   5   6   7   8   9
     */
    private static string[] Stacks()
    {
        return new string[]
        {
            "SZPDLBFC",
            "NVGPHWB",
            "FWBJG",
            "GJNFLWCS",
            "WJLTPMSH",
            "BCWGFS",
            "HTPMQBW",
            "FSWT",
            "NCR",
        };
    }
}
