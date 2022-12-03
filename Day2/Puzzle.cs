using System.Diagnostics;

namespace Day2;

class Puzzle : IPuzzle
{
    enum RPS { ROCK, PAPER, SCISSOR };
    enum MoveResult { Loss = 0, Draw = 3, Win = 6 }

    RPS ParseFirst(string c) => c switch
    {
        "A" => RPS.ROCK,
        "B" => RPS.PAPER,
        "C" => RPS.SCISSOR,
        _ => throw new InvalidProgramException()
    };
    RPS ParseSecond(string c) => c switch
    {
        "X" => RPS.ROCK,
        "Y" => RPS.PAPER,
        "Z" => RPS.SCISSOR,
        _ => throw new InvalidProgramException()
    };

    MoveResult ParseMove(string c) => c switch
    {
        "X" => MoveResult.Loss,
        "Y" => MoveResult.Draw,
        "Z" => MoveResult.Win,
        _ => throw new InvalidProgramException()
    };

    MoveResult Move(RPS they, RPS we)
    {
        MoveResult r = MoveResult.Draw;
        if (we == WinFrom(they))
            r = MoveResult.Win;
        else if (we == LoseFrom(they))
            r = MoveResult.Loss;
        return r;
    }

    RPS LoseFrom(RPS they) => they switch
    {
        RPS.ROCK => RPS.SCISSOR,
        RPS.PAPER => RPS.ROCK,
        RPS.SCISSOR => RPS.PAPER,
        _ => throw new InvalidProgramException()
    };

    RPS WinFrom(RPS they) => they switch
    {
        RPS.ROCK => RPS.PAPER,
        RPS.PAPER => RPS.SCISSOR,
        RPS.SCISSOR => RPS.ROCK,
        _ => throw new InvalidProgramException()
    };

    RPS DetermineSecond(RPS they, string c)
    {
        return ParseMove(c) switch
        {
            MoveResult.Loss => LoseFrom(they),
            MoveResult.Draw => they,
            MoveResult.Win => WinFrom(they),
            _ => throw new InvalidProgramException()
        };
    }

    int Value(RPS s) => s switch
    {
        RPS.ROCK => 1,
        RPS.PAPER => 2,
        RPS.SCISSOR => 3,
        _ => throw new InvalidProgramException()
    };

    int PlayRound1(string s)
    {
        var r = s.Split(' ');
        var t = new Tuple<RPS, RPS>(ParseFirst(r[0]), ParseSecond(r[1]));

        return (int)Move(t.Item1, t.Item2) + Value(t.Item2);
    }

    int PlayRound2(string s)
    {
        var r = s.Split(' ');
        var t = new Tuple<RPS, RPS>(ParseFirst(r[0]), DetermineSecond(ParseFirst(r[0]), r[1]));

        return (int)Move(t.Item1, t.Item2) + Value(t.Item2);
    }

    public string Name { get { return "Day 2"; } }

    public void Test()
    {
        string[] input = { "A Y", "B X", "C Z" };

        Debug.Assert(ParseFirst(input[0].Split(' ')[0]) == RPS.ROCK);
        Debug.Assert(ParseFirst(input[1].Split(' ')[0]) == RPS.PAPER);
        Debug.Assert(ParseFirst(input[2].Split(' ')[0]) == RPS.SCISSOR);
        Debug.Assert(ParseSecond(input[0].Split(' ')[1]) == RPS.PAPER);
        Debug.Assert(ParseSecond(input[1].Split(' ')[1]) == RPS.ROCK);
        Debug.Assert(ParseSecond(input[2].Split(' ')[1]) == RPS.SCISSOR);

        Debug.Assert(Move(ParseFirst(input[0].Split(' ')[0]), ParseSecond(input[0].Split(' ')[1])) == MoveResult.Win);
        Debug.Assert(Move(ParseFirst(input[1].Split(' ')[0]), ParseSecond(input[1].Split(' ')[1])) == MoveResult.Loss);
        Debug.Assert(Move(ParseFirst(input[2].Split(' ')[0]), ParseSecond(input[2].Split(' ')[1])) == MoveResult.Draw);

        Debug.Assert(Value(ParseSecond(input[0].Split(' ')[1])) + (int)MoveResult.Win == 8);
        Debug.Assert(Value(ParseSecond(input[1].Split(' ')[1])) + (int)MoveResult.Loss == 1);
        Debug.Assert(Value(ParseSecond(input[2].Split(' ')[1])) + (int)MoveResult.Draw == 6);

        Debug.Assert(PlayRound1(input[0]) == 8);
        Debug.Assert(PlayRound1(input[1]) == 1);
        Debug.Assert(PlayRound1(input[2]) == 6);
        Debug.Assert(input.AsEnumerable().Select(x => PlayRound1(x)).Sum() == 15);

        Debug.Assert(PlayRound2(input[0]) == 4);
        Debug.Assert(PlayRound2(input[1]) == 1);
        Debug.Assert(PlayRound2(input[2]) == 7);
        Debug.Assert(input.AsEnumerable().Select(x => PlayRound2(x)).Sum() == 12);
    }

    public void Part1()
    {
        var score = File.ReadAllLines("Day2/Input.txt").AsEnumerable().Select(x => PlayRound1(x)).Sum();

        Console.WriteLine($"{Name}:1 --> {score}");
    }
    public void Part2()
    {
        var score = File.ReadAllLines("Day2/Input.txt").AsEnumerable().Select(x => PlayRound2(x)).Sum();

        Console.WriteLine($"{Name}:2 --> {score}");
    }
}
