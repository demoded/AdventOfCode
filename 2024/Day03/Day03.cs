using SolutionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Day03 : Solution
{
    List<string> lines = new List<string>();
    protected override void ReadInput()
    {
        foreach (var line in File.ReadAllLines("..\\..\\..\\Day03\\input.txt"))
        { 
            lines.Add(line);
        }
    }

    protected override void Solve()
    {
        int partOneResult = 0;

        lines.ForEach(line =>
        {
            partOneResult += Regex.Matches(line, @"mul\(\d+,\d+\)").Aggregate(0, (acc, match) =>
            {
                var numbers = Regex.Matches(match.ToString(), @"\d+").Select(m => int.Parse(m.Value)).ToArray();
                return acc + numbers[0] * numbers[1];
            });
        });

        Console.WriteLine($"Part One: {partOneResult}");

        int partTwoResult = 0;
        var mulEnabled = true;
        lines.ForEach(line =>
        {
            partTwoResult += Regex.Matches(line, @"mul\(\d+,\d+\)|do\(\)|don't\(\)").Aggregate(0, (acc, match) =>
            {
                var numbers = Regex.Matches(match.ToString(), @"\d+").Select(m => int.Parse(m.Value)).ToArray();
                if (match.Value.StartsWith("mul") && mulEnabled)
                {
                    return acc + numbers[0] * numbers[1];
                }
                else if (match.Value.StartsWith("do("))
                {
                    mulEnabled = true;
                    return acc;
                }
                else if (match.Value.StartsWith("don't"))
                {
                    mulEnabled = false;
                    return acc;
                }
                return acc;
            });
        });

        Console.WriteLine($"Part Two: {partTwoResult}");
    }
}