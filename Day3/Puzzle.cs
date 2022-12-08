namespace Day3;

using System.Diagnostics;
using Common;

struct Rucksack
{
    public string Content { get; init; }
    public Rucksack(string content)
    {
        Content = content;
    }

    public string Left
    {
        get { return Content[..(Content.Length / 2)]; }
    }
    public string Right
    {
        get { return Content[(Content.Length / 2)..]; }
    }

    public char SharedItem
    {
        get
        {
            return Left.Intersect(Right).First();
        }
    }

    public Rucksack Intersect(Rucksack r)
    {
        return new Rucksack(Utils.CommonChars(Content, r.Content));
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(3) { }
    static int Priority(char item)
    {
        return Char.IsUpper(item) ? item - 'A' + 27 : item - 'a' + 1;
    }

    public override void Test()
    {
        Debug.Assert(Priority('a') == 1);
        Debug.Assert(Priority('z') == 26);
        Debug.Assert(Priority('A') == 27);
        Debug.Assert(Priority('Z') == 52);

        Rucksack[] rucksacks =
        {
            new Rucksack("vJrwpWtwJgWrhcsFMMfFFhFp"),
            new Rucksack("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"),
            new Rucksack("PmmdzqPrVvPwwTWBwg"),
            new Rucksack("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"),
            new Rucksack("ttgJtRGJQctTZtZT"),
            new Rucksack("CrZsJsPPZsGzwwsLwLmpwMDw"),
        };

        Debug.Assert(rucksacks[0].SharedItem == 'p');
        Debug.Assert(rucksacks[1].SharedItem == 'L');
        Debug.Assert(rucksacks[2].SharedItem == 'P');
        Debug.Assert(rucksacks[3].SharedItem == 'v');
        Debug.Assert(rucksacks[4].SharedItem == 't');
        Debug.Assert(rucksacks[5].SharedItem == 's');
        Debug.Assert(rucksacks.Select(x => Priority(x.SharedItem)).Sum() == 157);

        var priority = rucksacks
            .Chunk(3)
            .Select(chunk => chunk.Aggregate((common, rucksack) => common.Intersect(rucksack)))
            .Select(rucksack => Priority(rucksack.Content.First()))
            .Sum();

        Debug.Assert(priority == 70);
    }

    public override void Part1()
    {
        _sw.Restart();
        var priority = new TextFile("Day3/Input.txt")
            .Select(x => Priority(new Rucksack(x).SharedItem))
            .Sum();
        _sw.Stop();

        Debug.Assert(priority == 8018);
        Console.WriteLine($"{Name}:1 --> {priority} in {_sw.ElapsedMilliseconds} ms");
    }

    public override void Part2()
    {
        // Take the instersection of groups of three rucksacks
        _sw.Restart();
        var priority = new TextFile("Day3/Input.txt")
            .Select(line => new Rucksack(line))
            .Chunk(3)
            .Select(chunk => chunk.Aggregate((common, rucksack) => common.Intersect(rucksack)))
            .Select(rucksack => Priority(rucksack.Content.First()))
            .Sum();

        Debug.Assert(priority == 2518);
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {priority} in {_sw.ElapsedMilliseconds} ms");
    }
}
