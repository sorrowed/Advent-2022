using Autofac;

var builder = new ContainerBuilder();
builder.RegisterType<Day1.Puzzle>().As<IPuzzle>();
builder.RegisterType<Day2.Puzzle>().As<IPuzzle>();
builder.RegisterType<Day3.Puzzle>().As<IPuzzle>();
builder.RegisterType<Day4.Puzzle>().As<IPuzzle>();
builder.RegisterType<Day5.Puzzle>().As<IPuzzle>();

var puzzles = builder.Build().Resolve<IEnumerable<IPuzzle>>();
foreach (var puzzle in puzzles)
{
    puzzle.Test();
    puzzle.Part1();
    puzzle.Part2();
}
