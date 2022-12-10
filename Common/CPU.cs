namespace Common;
using System.Collections.Generic;

using Registers = Dictionary<string, long>;

class InstructionFactory
{
    public static Instruction Parse(string input)
    {
        var tokens = input.Split(' ');
        switch (tokens[0])
        {
            case "noop": return new Noop();
            case "addx": return new Addx() { Operand = long.Parse(tokens[1]) };
            default: throw new NotImplementedException();
        }
    }
}

abstract class Instruction
{
    public int Cycles { get; protected set; } = 1;

    public bool IsLastCycle { get { return Cycles == 1; } }
    public bool IsCommplete { get { return Cycles <= 0; } }
    public bool Cycle(Registers registers)
    {
        CycleImpl(registers);
        --Cycles;
        return IsCommplete;
    }

    protected abstract void CycleImpl(Registers registers);
}

class Noop : Instruction
{
    protected override void CycleImpl(Registers registers)
    {
    }
}

class Addx : Instruction
{
    public long Operand { get; init; }
    public Addx() { Cycles = 2; }
    protected override void CycleImpl(Registers registers)
    {
        if (IsLastCycle)
        {
            registers["x"] += Operand;
        }
    }
}

class CPUEventArgs : EventArgs
{
    public CPU? CPU { get; init; }
}

class CPU
{
    public event EventHandler<CPUEventArgs>? OnCycleBegin;

    public int Cycle { get; private set; } = 1;
    public Registers Registers { get; private init; } = new();

    public CPU()
    {
        Registers["x"] = 1;
    }

    public void Run(IEnumerable<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            do
            {
                OnCycleBegin?.Invoke(this, new CPUEventArgs() { CPU = this });
                instruction.Cycle(Registers);
                ++Cycle;
            }
            while (!instruction.IsCommplete);
        }
    }
}
