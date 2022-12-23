namespace Day15;

using System.Diagnostics;
using System.Text.RegularExpressions;
using Common;

record Coordinate(int X, int Y)
{
    public static int Manhattan(Coordinate from, Coordinate to)
    {
        return Math.Abs(to.X - from.X) + Math.Abs(to.Y - from.Y);
    }
}

record SensorReport(Coordinate Sensor, Coordinate Beacon)
{
    public int Distance
    {
        get
        {
            return Coordinate.Manhattan(Sensor, Beacon);
        }
    }
    public static SensorReport Parse(string chunk)
    {
        Regex r = new(@"x=(-?\d*),\sy=(-?\d*):.*x=(-?\d*),\sy=(-?\d*)", RegexOptions.None, TimeSpan.FromMilliseconds(150));
        Match m = r.Match(chunk);
        if (m.Success)
        {
            var sensor = new Coordinate(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            var beacon = new Coordinate(int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
            return new SensorReport(sensor, beacon);
        }
        throw new InvalidOperationException();
    }
}

class Deployments
{
    public List<SensorReport> Reports { get; private init; }

    public Deployments(List<SensorReport> reports) { Reports = reports; }

    /// <summary>
    /// Determine extends of sensor detection limits (limits detection are sensor coords +/- detection distance)
    /// </summary>
    public static (Coordinate, Coordinate) Limits(IEnumerable<SensorReport> reports)
    {
        var left = reports.MinBy(c => c.Sensor.X - c.Distance);
        var right = reports.MaxBy(c => c.Sensor.X + c.Distance);
        var top = reports.MinBy(c => c.Sensor.Y - c.Distance);
        var bottom = reports.MaxBy(c => c.Sensor.Y + c.Distance);

        return new(
            new Coordinate(left!.Sensor.X - left!.Distance, top!.Sensor.Y - top!.Distance),
            new Coordinate(right!.Sensor.X + right!.Distance, bottom!.Sensor.Y + bottom!.Distance)
        );
    }


    /// <summary>
    /// For each point on the horizontal line y which is not a beacon or sensor, see if there is any sensor that has a detection range which covers that point
    /// </summary>
    public int CountNotBeaconOnLine(int y)
    {
        var (tl, br) = Limits(Reports);

        return Enumerable.Range(tl!.X, br!.X - tl!.X + 1)
                         .Count(x => Reports.Any(report => { var point = new Coordinate(x, y); return point != report.Sensor && point != report.Beacon && Coordinate.Manhattan(report.Sensor, point) <= report.Distance; }));
    }

    public static void Print(IEnumerable<SensorReport> reports)
    {
        var (tl, br) = Limits(reports);
        for (int y = tl.Y; y <= br!.Y; ++y)
        {
            Console.Out.Write($"{y:d2} : ");

            for (int x = tl!.X; x <= br!.X; ++x)
            {
                if (reports.Any(r => r.Sensor == new Coordinate(x, y)))
                {
                    Console.Out.Write('S');
                }
                else if (reports.Any(r => r.Beacon == new Coordinate(x, y)))
                {
                    Console.Out.Write('B');
                }
                else
                {
                    Console.Out.Write('.');
                }
            }
            Console.Out.WriteLine();
        }
    }
}

class Puzzle : PuzzleBase
{
    public Puzzle() : base(15) { }


    public override void Test()
    {
        string[] input =
        {
            "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
            "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
            "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
            "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
            "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
            "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
            "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
            "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
            "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
            "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
            "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
            "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
            "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
            "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
        };

        var deployments = new Deployments(input.Select(SensorReport.Parse).ToList());

        Debug.Assert(deployments.Reports.Count == 14);

        var count = deployments.CountNotBeaconOnLine(9);
        Debug.Assert(count == 25);
        count = deployments.CountNotBeaconOnLine(10);
        Debug.Assert(count == 26);
        count = deployments.CountNotBeaconOnLine(11);
        Debug.Assert(count == 27);

        var (tl, br) = Deployments.Limits(deployments.Reports);
        for (int y = tl.Y; y <= br!.Y; ++y)
        {
            for (int x = tl!.X; x <= br!.X; ++x)
            {
                var beacon = new Coordinate(x,y);
                Debug.Assert(false);
            }
        }
    }

    public override void Part1()
    {
        var deployments = new Deployments(new TextFile("Day15/Input.txt").Select(SensorReport.Parse).ToList());
        _sw.Restart();
        var count = deployments.CountNotBeaconOnLine(2000000);
        _sw.Stop();

        Debug.Assert(count == 5127797);

        Console.WriteLine($"{Name}:1 --> {count} in {_sw.ElapsedMilliseconds} milliseconds");
    }

    public override void Part2()
    {
        // Which coordinate on the map is not covered by any sensor?
        // _sw.Restart();
        // _sw.Stop();

        // Debug.Assert(sands == 22646);

        // Console.WriteLine($"{Name}:1 --> {sands} in {_sw.ElapsedMilliseconds} milliseconds");
    }
}
