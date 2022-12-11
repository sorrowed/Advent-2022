namespace Day11;
using TestMonkeys;

using System.Diagnostics;
using Common;

class Puzzle : PuzzleBase
{
    public Puzzle() : base(11) { }

    public override void Test()
    {

        Test1();
        Test2();
    }

    private static void Test1()
    {
        List<Monkey> monkeys = new() { new TestMonkey0(), new TestMonkey1(), new TestMonkey2(), new TestMonkey3() };

        PlayRelaxed(monkeys, 1);

        Debug.Assert(monkeys[0].Items.SequenceEqual(new List<long> { 20, 23, 27, 26 }));
        Debug.Assert(monkeys[1].Items.SequenceEqual(new List<long> { 2080, 25, 167, 207, 401, 1046 }));
        Debug.Assert(monkeys[2].Items.SequenceEqual(new List<long> { }));
        Debug.Assert(monkeys[3].Items.SequenceEqual(new List<long> { }));

        PlayRelaxed(monkeys, 19);

        Debug.Assert(monkeys[0].Items.SequenceEqual(new List<long> { 10, 12, 14, 26, 34 }));
        Debug.Assert(monkeys[1].Items.SequenceEqual(new List<long> { 245, 93, 53, 199, 115 }));
        Debug.Assert(monkeys[2].Items.SequenceEqual(new List<long> { }));
        Debug.Assert(monkeys[3].Items.SequenceEqual(new List<long> { }));

        Debug.Assert(monkeys[0].Inspected == 101);
        Debug.Assert(monkeys[1].Inspected == 95);
        Debug.Assert(monkeys[2].Inspected == 7);
        Debug.Assert(monkeys[3].Inspected == 105);
    }

    private static void Test2()
    {
        List<Monkey> monkeys = new() { new TestMonkey0(), new TestMonkey1(), new TestMonkey2(), new TestMonkey3() };

        PlayWorried(monkeys, 10000);

        Debug.Assert(monkeys[0].Inspected == 52166);
        Debug.Assert(monkeys[1].Inspected == 47830);
        Debug.Assert(monkeys[2].Inspected == 1938);
        Debug.Assert(monkeys[3].Inspected == 52013);

        Debug.Assert(Busines(monkeys) == 2713310158);
    }

    private static void PlayRelaxed(List<Monkey> monkeys, int rounds)
    {
        for (int i = 0; i < rounds; ++i)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Play(monkeys, 3);
            }
        }
    }

    private static void PlayWorried(List<Monkey> monkeys, int rounds)
    {
        long reduce = monkeys.Aggregate(1L, (acc, monkey) => acc * monkey.TestValue);

        for (int i = 0; i < rounds; ++i)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Play(monkeys, 1);
            }

            foreach (var monkey in monkeys)
            {
                monkey.Reduce(reduce);
            }
        }
    }

    private static long Busines(List<Monkey> monkeys)
    {
        return monkeys.OrderByDescending(monkey => monkey.Inspected)
            .Take(2)
            .Aggregate(1L, (acc, monkey) => acc * monkey.Inspected);
    }

    public override void Part1()
    {
        List<Monkey> monkeys = new()
        {
            new Monkey0(), new Monkey1(), new Monkey2(), new Monkey3(),
            new Monkey4(), new Monkey5(), new Monkey6(), new Monkey7()
        };

        _sw.Restart();
        PlayRelaxed(monkeys, 20);

        var monkeyBuisines = Busines(monkeys);
        Debug.Assert(monkeyBuisines == 90882);
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {monkeyBuisines} in {_sw.ElapsedMilliseconds} milliseconds");
    }

    public override void Part2()
    {
        List<Monkey> monkeys = new()
        {
            new Monkey0(), new Monkey1(), new Monkey2(), new Monkey3(),
            new Monkey4(), new Monkey5(), new Monkey6(), new Monkey7()
        };

        _sw.Restart();
        PlayWorried(monkeys, 10000);

        var monkeyBuisines = Busines(monkeys);
        Debug.Assert(monkeyBuisines == 30893109657);
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {monkeyBuisines} in {_sw.ElapsedMilliseconds} milliseconds");
    }
}
