using SolutionCore;
using System.Buffers;
using System.Globalization;
using System;
using System.Security;
using System.Text;

public class Day07 : Solution
{
    List<long> results = new();
    List<int[]> numbers = new();
    char[] operators;

    protected override void ReadInput()
    {
        foreach (var line in File.ReadAllLines("..\\..\\..\\Day07\\input.txt"))
        {
            int splitPos = line.IndexOf(":");
            var a = line.AsSpan().Slice(0, splitPos);   // result number 123
            var b = line.AsSpan().Slice(splitPos + 2);  // string of numbers 12 10 3
            
            results.Add(long.Parse(a));

            splitPos = 0;
            int[] ints = new int[b.Count(' ') + 1];
            int index = 0;
            for (var i = 0; i < b.Length; i++)
            {
                if (b[i] == ' ')
                {
                    ints[index++] = int.Parse(b.Slice(splitPos, i - splitPos));
                    splitPos = i + 1;
                }
            }
            ints[index] = int.Parse(b.Slice(splitPos));
            numbers.Add(ints);
        }
    }

    protected override void Solve()
    {
        long partOneResult = 0;

        for (int i = 0; i < results.Count; i++)
        {
            operators = Enumerable.Repeat('+', numbers[i].Length - 1).ToArray();
            if (FindResult(numbers[i], results[i], operators))
            {
                partOneResult += results[i];
            }
        }

        Console.WriteLine($"Part One result: {partOneResult}");

        long partTwoResult = 0;

        for (int i = 0; i < results.Count; i++)
        {
            var combinations = GenerateCombinations(new char[] { '+', '*', '|' }, numbers[i].Length - 1);

            long result = 0;
            foreach (var ops in combinations)
            {
                int[] numClone = (int[])numbers[i].Clone();

                result = CalcSequential(numClone, ops.ToCharArray());                

                if (result == results[i])
                {
                    partTwoResult += results[i];
                    break;
                }
            }

            Console.WriteLine($"{string.Join(" ", numbers[i])} = {result} expected:{results[i]}:{result == results[i]} ");
        }

        Console.WriteLine($"Part Two result: {partTwoResult}");
    }

    private bool FindResult(int[] ints, long expectedResult, char[] ops)
    {
        do 
        {
            if (CalcSequential(ints, ops) == expectedResult)
            {
                return true;
            }
        } 
        while (MutateOperators(ops));

        return false;
    }

    private bool MutateOperators(char[] ops)
    {
        if (ops.Length == 1 && ops[0] == '|')
        {
            return false;
        }

        for (int i = ops.Length - 1; i >= 0; i--)
        {
            if (ops[i] == '+')
            {
                ops[i] = '*';
                break;
            }
            else if (ops[i] == '*')
            {
                ops[i] = '+';
                if (i == 0 || i == 1 && ops[0] == '|')
                {
                    return false;
                }
            }
        }

        return true;
    }

    private long CalcSequential(int[] ints, char[] operators)
    {
        long result;

        result = ints[0];
        for (int i = 0; i < operators.Length; i++)
        {
            if (operators[i] == '+')
            {
                result += ints[i + 1];
            }
            else if (operators[i] == '|')
            {
                result = long.Parse($"{result}{ints[i + 1]}");
            }
            else 
            {
                result *= ints[i + 1];
            }
        }
        return result;
    }

    public static List<string> GenerateCombinations(char[] characters, int length)
    {
        List<string> result = new List<string>();
        GenerateCombinationsRecursive(characters, length, "", result);
        return result;//.Where(s => s.Contains('|')).ToList(); // Filter to include only combinations with '|'
    }

    private static void GenerateCombinationsRecursive(char[] characters, int length, string current, List<string> result)
    {
        if (length == 0)
        {
            result.Add(current);
            return;
        }

        foreach (var c in characters)
        {
            GenerateCombinationsRecursive(characters, length - 1, current + c, result);
        }
    }
}