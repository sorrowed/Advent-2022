using Autofac;
using Common;

var builder = new ContainerBuilder();
builder.RegisterType<Day1.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day2.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day3.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day4.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day5.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day6.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day7.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day8.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day9.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day10.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day11.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day12.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day13.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day14.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day15.Puzzle>().As<PuzzleBase>();

var puzzles = builder.Build().Resolve<IEnumerable<PuzzleBase>>();
foreach (var puzzle in puzzles)
{
    puzzle.Test();
    puzzle.Part1();
    puzzle.Part2();
}
