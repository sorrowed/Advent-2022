namespace Day11;

class Monkey0 : Monkey
{
    public Monkey0() : base(0, new List<long> { 62, 92, 50, 63, 62, 93, 73, 50 }, 2, 7, 1)
    {
    }
    public override long Inspect(long item)
    {
        return item * 7;
    }
}

class Monkey1 : Monkey
{
    public Monkey1() : base(1, new() { 51, 97, 74, 84, 99 }, 7, 2, 4)
    {
    }
    public override long Inspect(long item)
    {
        return item + 3;
    }
}

class Monkey2 : Monkey
{
    public Monkey2() : base(2, new() { 98, 86, 62, 76, 51, 81, 95 }, 13, 5, 4)
    {
    }

    public override long Inspect(long item)
    {
        return item + 4;
    }
}

class Monkey3 : Monkey
{
    public Monkey3() : base(3, new() { 53, 95, 50, 85, 83, 72 }, 19, 6, 0) { }

    public override long Inspect(long item)
    {
        return item + 5;
    }
}

class Monkey4 : Monkey
{
    public Monkey4() : base(4, new() { 59, 60, 63, 71 }, 11, 5, 3) { }

    public override long Inspect(long item)
    {
        return item * 5;
    }
}

class Monkey5 : Monkey
{
    public Monkey5() : base(5, new() { 92, 65 }, 5, 6, 3) { }

    public override long Inspect(long item)
    {
        return item * item;
    }
}

class Monkey6 : Monkey
{
    public Monkey6() : base(6, new() { 78 }, 3, 0, 7) { }

    public override long Inspect(long item)
    {
        return item + 8;
    }
}

class Monkey7 : Monkey
{
    public Monkey7() : base(7, new() { 84, 93, 54 }, 17, 2, 1) { }

    public override long Inspect(long item)
    {
        return item + 1;
    }
}
