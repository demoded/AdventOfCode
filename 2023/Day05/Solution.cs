
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

internal class Solution
{
    internal struct MapEntry
    {
        internal long dst;
        internal long src;
        internal long len;
    }

    internal static void Run()
    {
        int mapCounter = 0;
        List<long> seeds = new List<long>();
        var maps = new Dictionary<int, List<MapEntry>>();
        var flatMap = new List<MapEntry>();

        // Read input
        foreach (var line in File.ReadLines(@"..\..\..\input.txt"))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var colonPosition = line.IndexOf(':');
            if (colonPosition > 0)
            {
                var type = line.AsSpan().Slice(0, line.IndexOf(':')).ToString();
                if (type == "seeds")
                {
                    seeds = line.AsSpan().Slice(line.IndexOf(':') + 1).ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToList();
                }
                else if (type.EndsWith("map"))
                {
                    mapCounter++;
                    maps[mapCounter] = new List<MapEntry>();
                }
            }
            else
            {
                var mapline = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
                maps[mapCounter].Add(new MapEntry { dst = mapline[0], src = mapline[1], len = mapline[2] });
            }
        }

        // Part 1
        var result = long.MaxValue;
        foreach (var seed in seeds)
        {
            var mapResult = seed;

            foreach (var map in maps)
            {
                foreach (var entry in map.Value)
                {
                    if (mapResult >= entry.src && mapResult <= entry.src + entry.len - 1)
                    {
                        mapResult = entry.dst + (mapResult - entry.src);
                        break;
                    }
                }
            }

            result = Math.Min(result, mapResult);
        }
        Console.WriteLine($"Part 1 Min location: {result}");

        // Part 2
        result = long.MaxValue;

        foreach (var map in maps)
        {
            string m = string.Empty;
            foreach (var entry in map.Value)
            {
                m = m + $"[{entry.src}:{entry.src + entry.len - 1} to {entry.dst}] ";
            }
            Debug.Print($"Map {map.Key}: {m}");

            string t = string.Empty;
            for (int i = 0; i < seeds.Count; i += 2)
            {
                t = t + $"[{seeds[i]}:{seeds[i] + seeds[i + 1] - 1}] ";
            }
            Debug.Print($"Starting Seeds: {t}");

            var matchedSeeds = new List<long>();
            var missedSeeds = new List<long>();
            foreach (var entry in map.Value)
            {
                for (int i = 0; i < seeds.Count; i += 2)
                {
                    var seedRangeStart = seeds[i];
                    var seedRangeEnd = seedRangeStart + seeds[i + 1] - 1;
                    var newRangeStart = seedRangeStart;
                    var newRangeEnd = seedRangeEnd;

                    // so, either start or end will fall into the range
                    if (seedRangeStart < entry.src + entry.len && entry.src < seedRangeEnd)
                    {
                        Debug.Print($"Intersection found Map [{entry.src}:{entry.src + entry.len - 1}] to {entry.dst} Seed [{seedRangeStart}:{seedRangeEnd}]");                        
                        // both start and end will fall into the range
                        if (seedRangeStart >= entry.src && seedRangeEnd < entry.src + entry.len)
                        {
                            // move start, leave length the same
                            newRangeStart = seedRangeStart + entry.dst - entry.src;
                            matchedSeeds.Add(newRangeStart);
                            matchedSeeds.Add(seeds[i + 1]);
                            Debug.Print($"    Full inner intersection. Move to [{newRangeStart}:{newRangeStart + seeds[i+1] - 1}]");
                        }
                        // start is outside the range, end is inside
                        else if (seedRangeStart < entry.src)
                        {
                            // move start to the entry start, cut length to comensate start change
                            newRangeStart = entry.dst;
                            var newLength = seedRangeEnd - entry.src + 1;
                            matchedSeeds.Add(newRangeStart);
                            matchedSeeds.Add(newLength);
                            missedSeeds.Add(seedRangeStart);
                            missedSeeds.Add(entry.src - seedRangeStart);
                            Debug.Print($"    Start is outside. Moved Seed to [{matchedSeeds[0]}:{matchedSeeds[0] + matchedSeeds[1] - 1}]");
                        }
                        else if (seedRangeStart >= entry.src)
                        {
                            // cut length to fit entry range end
                            var newLength = entry.src + entry.len - seedRangeStart;
                            matchedSeeds.Add(seedRangeStart + entry.dst - entry.src);
                            matchedSeeds.Add(newLength);
                            missedSeeds.Add(seedRangeStart + newLength);
                            missedSeeds.Add(seedRangeEnd - seedRangeStart - newLength + 1);
                            Debug.Print($"    End is outside. Moved Seed to [{seedRangeStart + entry.dst - entry.src}:{seedRangeStart + entry.dst - entry.src + newLength - 1}]. ");
                            Debug.Print($"                    Added new Seed [{seedRangeStart + newLength}:{seedRangeStart + newLength + seedRangeEnd - seedRangeStart - newLength}]");
                        }
                    }
                    else
                    {
                        missedSeeds.Add(seeds[i]);
                        missedSeeds.Add(seeds[i + 1]);
                    }
                }

                seeds.Clear();
                seeds.AddRange(missedSeeds);
                missedSeeds.Clear();
            }

            seeds.AddRange(matchedSeeds);


            result = long.MaxValue;
            t = string.Empty;
            for (int i = 0; i < seeds.Count; i += 2)
            {
                t = t + $"[{seeds[i]}:{seeds[i] + seeds[i + 1] - 1}] ";
                result = Math.Min(result, seeds[i]);
            }
            Debug.Print($"Processed Seeds: {t}");
            Debug.Print("");

        }

        Console.WriteLine($"Part 2 Min location: {result}");
    }

    internal static void TestRangeIntersection()
    {
        TestIntersection((0, 10), (5, 7));   // expected output [0:4][5:7][8:10]
        TestIntersection((0, 10), (5, 15));  // expected output [0:4][5:10][11:15]
        TestIntersection((0, 10), (15, 20)); // expected output [0:10][15:20]
        TestIntersection((5, 10), (0, 7));   // expected output [0:4][5:7][8:10]

        // [A.start                                               A.end]
        //                       [B.start    B.end]
        // [A.start    B.start-1][B.start    B.end][B.end + 1     A.end]
        // [BEFORE              ][INTERSECTION    ][AFTER              ]
        void TestIntersection((long start, long end) a, (long start, long end) b)
        {
            (long start, long end) before = (a.start, Math.Min(a.end, b.start));
            (long start, long end) intersection = (Math.Max(a.start, b.start), Math.Min(a.end, b.end));
            (long start, long end) after = (Math.Max(a.start, b.end), a.end);

            if (before.end >= before.start)
            {
                Console.Write($"[{before.start}:{before.end}]");
            }
            if (intersection.end >= intersection.start)
            {
                Console.Write($"[{intersection.start}:{intersection.end}]");
            }
            if (after.end >= after.start)
            {
                Console.Write($"[{after.start}:{after.end}]");
            }
            Console.WriteLine();
        }
    }
}