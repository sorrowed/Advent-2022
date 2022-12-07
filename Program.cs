using Autofac;

var builder = new ContainerBuilder();
builder.RegisterType<Day1.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day2.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day3.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day4.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day5.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day6.Puzzle>().As<PuzzleBase>();
builder.RegisterType<Day7.Puzzle>().As<PuzzleBase>();

var puzzles = builder.Build().Resolve<IEnumerable<PuzzleBase>>();
foreach (var puzzle in puzzles)
{
    puzzle.Test();
    puzzle.Part1();
    puzzle.Part2();
}
