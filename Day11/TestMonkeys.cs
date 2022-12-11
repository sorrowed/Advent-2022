namespace TestMonkeys;
using Day11;

class TestMonkey0 : Monkey
{
    public TestMonkey0() : base(0, new List<long> { 79, 98 }, 23, 2, 3)
    {
    }
    public override long Inspect(long item)
    {
        return item * 19;
    }
}

class TestMonkey1 : Monkey
{
    public TestMonkey1() : base(1, new() { 54, 65, 75, 74 }, 19, 2, 0)
    {
    }
    public override long Inspect(long item)
    {
        return item + 6;
    }
}

class TestMonkey2 : Monkey
{
    public TestMonkey2() : base(2, new() { 79, 60, 97 }, 13, 1, 3)
    {
    }

    public override long Inspect(long item)
    {
        return item * item;
    }
}

class TestMonkey3 : Monkey
{
    public TestMonkey3() : base(3, new() { 74 }, 17, 0, 1) { }

    public override long Inspect(long item)
    {
        return item + 3;
    }
}