namespace Day11;

abstract class Monkey
{
    public int Id { get; private init; }
    public List<long> Items { get; private init; }
    public long TestValue { get; private init; }
    public int TrueTarget { get; private init; }
    public int FalseTarget { get; private init; }
    public int Inspected { get; private set; } = 0;

    protected Monkey(int id, List<long> items, long testValue, int trueTarget, int falseTarget)
    {
        Id = id;
        Items = items;
        TestValue = testValue;
        TrueTarget = trueTarget;
        FalseTarget = falseTarget;
    }

    abstract public long Inspect(long item);
    private bool Test(long item) { return item % TestValue == 0; }

    private void Catch(long item) { Items.Add(item); }

    public void Play(List<Monkey> monkeys, long reliefFactor)
    {
        while (Items.Count > 0)
        {
            var item = Items[0];
            Items.RemoveAt(0);

            item = Inspect(item);
            ++Inspected;

            item /= reliefFactor;

            monkeys[Test(item) ? TrueTarget : FalseTarget].Catch(item);
        }
    }

    public void Reduce(long factor)
    {
        for (int j = 0; j < Items.Count; ++j)
        {
            Items[j] %= factor;
        }
    }
}
