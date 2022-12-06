namespace Day6;

using System.Diagnostics;
using Common;

class Puzzle : PuzzleBase
{
    public Puzzle() : base(6) { }

    private Tuple<int, string> Unique(string input, int count)
    {
        for (int i = 0; i < input.Length - count; ++i)
        {
            string token = input[(i)..(i + count)];
            if (token.Distinct().Count() == count)
            {
                return new Tuple<int, string>(i + count, token);
            }
        }
        throw new InvalidOperationException();
    }

    public override void Test()
    {
        Debug.Assert(Unique("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4).Item1 == 7);
        Debug.Assert(Unique("bvwbjplbgvbhsrlpgdmjqwftvncz", 4).Item1 == 5);
        Debug.Assert(Unique("nppdvjthqldpwncqszvftbrmjlhg", 4).Item1 == 6);
        Debug.Assert(Unique("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4).Item1 == 10);
        Debug.Assert(Unique("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4).Item1 == 11);
    }


    public override void Part1()
    {
        var startOfPacket = Unique(File.OpenText("Day6/Input.txt").ReadToEnd(), 4);

        Debug.Assert(startOfPacket.Item1 == 1300);

        Console.WriteLine($"{Name}:1 --> {startOfPacket.Item1}");
    }

    public override void Part2()
    {
        var startOfMessage = Unique(File.OpenText("Day6/Input.txt").ReadToEnd(), 14);

        Debug.Assert(startOfMessage.Item1 == 3986);

        Console.WriteLine($"{Name}:2 --> {startOfMessage.Item1}");
    }
}
