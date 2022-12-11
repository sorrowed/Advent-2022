namespace Common;

abstract class PuzzleBase
{
    protected readonly System.Diagnostics.Stopwatch _sw = new();

    public int Number { get; private init; }
    protected PuzzleBase(int number) { Number = number; }

    public string Name { get { return string.Format("Puzzle {0:d}", Number); } }
    public abstract void Test();

    public abstract void Part1();
    public abstract void Part2();
}