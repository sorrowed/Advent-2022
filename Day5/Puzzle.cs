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

class Crane9000
{
    public void Apply(string[] stacks, Move move)
    {
        var from = stacks[move.From - 1];

        stacks[move.To - 1] += new string(from[^move.HowMany..].Reverse().ToArray()); // HowMany from the end up to the end
        stacks[move.From - 1] = from[0..^move.HowMany]; // From beginning to HowMany from the end
    }
};

class Crane9001
{
    public void Apply(string[] stacks, Move move)
    {
        var from = stacks[move.From - 1];

        stacks[move.To - 1] += from[^move.HowMany..]; // HowMany from the end up to the end
        stacks[move.From - 1] = from[0..^move.HowMany]; // From beginning to HowMany from the end
    }
};

class Puzzle : IPuzzle
{
    public string Name { get { return "Day 5"; } }

    public void Test()
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

        var crane = new Crane9000();
        foreach (var move in moves)
        {
            crane.Apply(stacks, move);
        }

        Debug.Assert(stacks[0].Length == 1 && stacks[0].Last() == 'C');
        Debug.Assert(stacks[1].Length == 1 && stacks[1].Last() == 'M');
        Debug.Assert(stacks[2].Length == 4 && stacks[2].Last() == 'Z');
    }

    public void Part1()
    {
        var stacks = Stacks();

        var crane = new Crane9000();
        foreach (var move in new TextFile("Day5/Input.txt").Select(Move.Parse))
        {
            crane.Apply(stacks, move);
        }

        var topCrates = stacks.Aggregate("", (acc, stack) => acc + stack[^1]);

        Debug.Assert(topCrates == "FWSHSPJWM");

        Console.WriteLine($"{Name}:1 --> {topCrates}");
    }

    public void Part2()
    {
        var stacks = Stacks();

        var crane = new Crane9001();
        foreach (var move in new TextFile("Day5/Input.txt").Select(Move.Parse))
        {
            crane.Apply(stacks, move);
        }
        var topCrates = stacks.Aggregate("", (acc, stack) => acc + stack[^1]);

        Debug.Assert(topCrates == "PWPWHGFZS");

        Console.WriteLine($"{Name}:2 --> {topCrates}");
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
    private string[] Stacks()
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
