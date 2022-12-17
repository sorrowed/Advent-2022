namespace Day13;

using System.Diagnostics;
using System.Text.Json;
using Common;

class Puzzle : PuzzleBase
{
    public Puzzle() : base(13) { }

    private static int Compare(JsonElement.ArrayEnumerator left, JsonElement.ArrayEnumerator right)
    {
        while (left.MoveNext() && right.MoveNext())
        {
            //Console.Out.WriteLine($"Comparing {left.Current} and {right.Current}");

            if (left.Current.ValueKind == JsonValueKind.Number && right.Current.ValueKind == JsonValueKind.Number)
            {
                int r = left.Current.GetInt32() - right.Current.GetInt32();
                if (r != 0)
                {
                    return r;
                }
            }
            else if (left.Current.ValueKind == JsonValueKind.Number && right.Current.ValueKind == JsonValueKind.Array)
            {
                return Compare(JsonDocument.Parse($"[{left.Current.GetInt32()}]").RootElement.EnumerateArray(), right.Current.EnumerateArray());
            }
            else if (left.Current.ValueKind == JsonValueKind.Array && right.Current.ValueKind == JsonValueKind.Number)
            {
                return Compare(left.Current.EnumerateArray(), JsonDocument.Parse($"[{right.Current.GetInt32()}]").RootElement.EnumerateArray());
            }
            else
            {
                Debug.Assert(left.Current.ValueKind == JsonValueKind.Array && right.Current.ValueKind == JsonValueKind.Array);

                int r = Compare(left.Current.EnumerateArray(), right.Current.EnumerateArray());
                if (r != 0)
                {
                    return r;
                }
            }
        }
        return left.Count() - right.Count();
    }
    private static int Compare(string a, string b)
    {
        var left = JsonDocument.Parse(a).RootElement;
        var right = JsonDocument.Parse(b).RootElement;

        Console.Out.WriteLine($"Comparing {left} and {right}");

        int result = Compare(left.EnumerateArray(), right.EnumerateArray());

        if (result < 0)
            Console.Out.WriteLine($"Left side is smaller");
        else if (result > 0)
            Console.Out.WriteLine($"Right side is smaller");
        else
            throw new ApplicationException();
        return result;
    }
    private static int Compare(Tuple<string, string> pair)
    {
        return Compare(pair.Item1, pair.Item2);
    }

    private static IEnumerable<Tuple<string, string>> Pairs(IEnumerable<string> input)
    {
        return input.ChunkBy((s) => s == "")
            .Select(chunk => new Tuple<string, string>(chunk.ElementAt(0), chunk.ElementAt(1)));
    }

    public override void Test()
    {
        string[] input =
        {
            "[1,1,3,1,1]",
            "[1,1,5,1,1]",
            "",
            "[[1],[2,3,4]]",
            "[[1],4]",
            "",
            "[9]",
            "[[8,7,6]]",
            "",
            "[[4,4],4,4]",
            "[[4,4],4,4,4]",
            "",
            "[7,7,7,7]",
            "[7,7,7]",
            "",
            "[]",
            "[3]",
            "",
            "[[[]]]",
            "[[]]",
            "",
            "[1,[2,[3,[4,[5,6,7]]]],8,9]",
            "[1,[2,[3,[4,[5,6,0]]]],8,9]"
        };

        var pairs = Pairs(input).ToList();

        Debug.Assert(Compare(pairs[0]) < 0);
        Debug.Assert(Compare(pairs[1]) < 0);
        Debug.Assert(Compare(pairs[2]) > 0);
        Debug.Assert(Compare(pairs[3]) < 0);
        Debug.Assert(Compare(pairs[4]) > 0);
        Debug.Assert(Compare(pairs[5]) < 0);
        Debug.Assert(Compare(pairs[6]) > 0);
        Debug.Assert(Compare(pairs[7]) > 0);

        var sumOfIndices = Pairs(input)
            .Select((pair, index) => Compare(pair) < 0 ? index + 1 : 0)
            .Sum();
        Debug.Assert(sumOfIndices == 13);
    }

    public override void Part1()
    {
        _sw.Restart();
        var sumOfIndices = Pairs(new TextFile("Day13/Input.txt"))
            .Select((pair, index) => Compare(pair) < 0 ? index + 1 : 0)
            .Sum();
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {sumOfIndices} in {_sw.ElapsedMilliseconds} milliseconds");
    }

    public override void Part2()
    {
        _sw.Restart();

        //Debug.Assert(distance == 508);
        _sw.Stop();
        //Console.WriteLine($"{Name}:2 --> {distance} in {_sw.ElapsedMilliseconds} milliseconds");
    }
}
