namespace Day7;

using System.Diagnostics;
using Common;

record Dir
{
    public Dir? Parent { get; init; }
    public string? Name { get; init; }
    public List<File> Files { get; } = new List<File>();
    public List<Dir> Dirs { get; } = new List<Dir>();

    public int Size() { return Dirs.Sum(d => d.Size()) + Files.Sum(f => f.Size); }

    public List<Dir> Filter(Func<Dir, bool> selector)
    {
        return Filter(new List<Dir>(), selector);
    }

    private List<Dir> Filter(List<Dir> dirs, Func<Dir, bool> selector)
    {
        if (selector(this))
        {
            dirs.Add(this);
        }

        foreach (var dir in Dirs)
        {
            dir.Filter(dirs, selector);
        }

        return dirs;
    }
}

record File
{
    public string? Name { get; init; }
    public int Size { get; set; }
}



class Puzzle : PuzzleBase
{
    public Puzzle() : base(7) { }

    public override void Test()
    {
        string[] input =
        {
            "$ cd /",
            "$ ls",
            "dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d",
            "$ cd a",
            "$ ls",
            "dir e",
            "29116 f",
            "2557 g",
            "62596 h.lst",
            "$ cd e",
            "$ ls",
            "584 i",
            "$ cd ..",
            "$ cd ..",
            "$ cd d",
            "$ ls",
            "4060174 j",
            "8033020 d.log",
            "5626152 d.ext",
            "7214296 k",
        };


        var smallDirs = BuidFileSystem(input).Filter((d) => d.Size() <= 100000);
        Debug.Assert(smallDirs.Count == 2);
        Debug.Assert(smallDirs[0].Name == "a" && smallDirs[0].Size() == 94853);
        Debug.Assert(smallDirs[1].Name == "e" && smallDirs[1].Size() == 584);
    }


    public override void Part1()
    {
        _sw.Restart();
        var size = BuidFileSystem(new TextFile("Day7/Input.txt"))
            .Filter((d) => d.Size() <= 100000)
            .Sum(d => d.Size());

        Debug.Assert(size == 1583951);
        _sw.Stop();

        Console.WriteLine($"{Name}:1 --> {size} in {_sw.ElapsedMilliseconds} ms");
    }

    public override void Part2()
    {
        _sw.Restart();
        var filesystem = BuidFileSystem(new TextFile("Day7/Input.txt"));
        var spaceWanted = 30000000 - (70000000 - filesystem.Size());

        var dir = filesystem.Filter(d => d.Size() >= spaceWanted).OrderBy(d => d.Size()).First();

        Debug.Assert(dir.Size() == 214171);
        _sw.Stop();

        Console.WriteLine($"{Name}:2 --> {dir.Size()} in {_sw.ElapsedMilliseconds} ms");
    }

    private static Dir BuidFileSystem(IEnumerable<string> input)
    {
        var root = new Dir { Name = "/" };
        Dir? currentDir = null;
        foreach (var line in input)
        {
            var tokens = line.Split(' ');

            if (tokens[0] == "$")
            {
                if (tokens[1] == "cd")
                {
                    if (tokens[2] == "/")
                    {
                        currentDir = root;
                    }
                    else if (tokens[2] == "..")
                    {
                        currentDir = currentDir!.Parent;
                    }
                    else
                    {
                        currentDir = currentDir!.Dirs.Where(d => d.Name == tokens[2]).First();
                    }
                }
            }
            else if (tokens[0] == "dir")
            {
                currentDir!.Dirs.Add(new Dir { Parent = currentDir, Name = tokens[1] });
            }
            else
            {
                currentDir!.Files.Add(new File { Size = int.Parse(tokens[0]), Name = tokens[1] });
            }
        }
        return root;
    }
}
