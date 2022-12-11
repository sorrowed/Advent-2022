namespace Day10;

using System.Diagnostics;
using System.Text;
using Common;

class Crt
{
    private const int DISPLAY_COLUMNS = 40;

    public string Output { get { return _output.ToString(); } }

    private readonly StringBuilder _output = new("\n");

    public Crt(Cpu cpu)
    {
        cpu.OnCycleBegin += (sender, args) =>
        {
            Cycle(args.CPU!);
        };
    }

    private void Cycle(Cpu cpu)
    {
        if (IsSpriteInView(cpu))
        {
            _output.Append('#');
        }
        else
        {
            _output.Append(' ');
        }

        if (cpu.Cycle % DISPLAY_COLUMNS == 0)
        {
            _output.Append('\n');
        }
    }

    private static bool IsSpriteInView(Cpu cpu)
    {
        long middle = cpu.Registers["x"];
        int column = (cpu.Cycle - 1) % DISPLAY_COLUMNS;

        return column >= (middle - 1) && column <= (middle + 1);
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(10) { }

    public override void Test()
    {
        Test1();
        Test2();
    }

    private static void Test1()
    {
        string[] input =
        {
                "noop",
                "addx 3",
                "addx -5",
            };

        var instructions = input.Select(InstructionFactory.Parse);

        var cpu = new Cpu();
        cpu.Run(instructions);

        Debug.Assert(cpu.Registers["x"] == -1);
    }

    private static void Test2()
    {
        var instructions = new TextFile("Day10/TestInput.txt").Select(InstructionFactory.Parse);
        long strength = 0;

        var cpu = new Cpu();
        cpu.OnCycleBegin += (_, args) =>
        {
            var cpu = args.CPU!;

            Debug.Assert(cpu.Cycle != 20 || cpu.Registers["x"] == 21);
            Debug.Assert(cpu.Cycle != 60 || cpu.Registers["x"] == 19);
            Debug.Assert(cpu.Cycle != 100 || cpu.Registers["x"] == 18);
            Debug.Assert(cpu.Cycle != 140 || cpu.Registers["x"] == 21);
            Debug.Assert(cpu.Cycle != 180 || cpu.Registers["x"] == 16);
            Debug.Assert(cpu.Cycle != 220 || cpu.Registers["x"] == 18);

            if (UnevenlyDivisibleBy20(cpu.Cycle))
            {
                strength += cpu.Cycle * cpu.Registers["x"];
            }
        };

        cpu.Run(instructions);

        Debug.Assert(cpu.Registers["x"] == 17);
        Debug.Assert(strength == 13140);
    }

    private static bool UnevenlyDivisibleBy20(int value)
    {
        return (value % 20 == 0) && int.IsOddInteger(value / 20);
    }

    public override void Part1()
    {
        var instructions = new TextFile("Day10/Input.txt").Select(InstructionFactory.Parse);

        var cpu = new Cpu();

        long strength = 0;

        cpu.OnCycleBegin += (sender, args) =>
        {
            var cpu = args.CPU!;

            if (UnevenlyDivisibleBy20(cpu.Cycle))
            {
                strength += cpu.Cycle * cpu.Registers["x"];
            }
        };

        _sw.Restart();
        cpu.Run(instructions);
        _sw.Stop();

        Debug.Assert(strength == 11720);
        Console.WriteLine($"{Name}:1 --> {strength} in {_sw.ElapsedMilliseconds} milliseconds");
    }

    public override void Part2()
    {
        var instructions = new TextFile("Day10/Input.txt").Select(InstructionFactory.Parse);

        var cpu = new Cpu();
        var crt = new Crt(cpu);

        _sw.Restart();
        cpu.Run(instructions);
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {crt.Output} in {_sw.ElapsedMilliseconds} milliseconds");
    }
}
