
internal class Solution
{
    internal static void Run()
    {
        List<int> raceTime = new List<int>();
        List<int> recordDistance = new List<int>();
        List<int> winCount = new List<int>();
        int victories = 0;

        // Read input
        foreach (var line in File.ReadLines(@"..\..\..\input.txt"))
        {
            if (line.StartsWith("Time"))
            { 
                raceTime = line.Substring(line.IndexOf(":") + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList();
            }

            if (line.StartsWith("Distance"))
            {
                recordDistance = line.Substring(line.IndexOf(":") + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            }
        }

        // Part 1
        for (int i = 0; i < raceTime.Count; i++)
        {
            victories = 0;
            for (int t = 1; t <= raceTime[i]; t++)
            {
                var speed = t; 
                var raceDistance = speed * (raceTime[i] - t);

                if (raceDistance > recordDistance[i])
                {
                    victories++;
                }
            }

            winCount.Add(victories);
        }

        // multiply all winCount entries
        var result = winCount.Aggregate((a, b) => a * b);
        Console.WriteLine($"Part 1 result: {result}");

        // Part 2
        var totalTime = long.Parse(string.Join("", raceTime));
        var tatalDistance = long.Parse(string.Join("", recordDistance));

        victories = 0;
        for (int t = 1; t <= totalTime; t++)
        {
            var speed = t;
            var raceDistance = speed * (totalTime - t);

            if (raceDistance > tatalDistance)
            {
                victories++;
            }
            else if (victories > 0)
            {
                break;
            }
        }

        Console.WriteLine($"Part 2 result: {victories}");
    }
}