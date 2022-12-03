using Autofac;

var builder = new ContainerBuilder();
builder.RegisterType<Day1.Puzzle>().As<IPuzzle>();
builder.RegisterType<Day2.Puzzle>().As<IPuzzle>();
builder.RegisterType<Day3.Puzzle>().As<IPuzzle>();

builder.Build().Resolve<IEnumerable<IPuzzle>>().ToList().ForEach(day => { day.Test(); day.Part1(); day.Part2(); });