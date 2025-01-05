
using SolutionCore;

public class Day05 : Solution
{
    LinkedList<Pair> rules = new LinkedList<Pair>();
    LinkedList<int[]> prints = new LinkedList<int[]>();
    protected override void ReadInput()
    {
        bool rulesSection = true;
        foreach (var line in File.ReadAllLines("..\\..\\..\\Day05\\input.txt"))
        {
            if (rulesSection)
            {
                if (line == string.Empty)
                {
                    rulesSection = false;
                    continue;
                }

                var pair = line.Split("|").Select(int.Parse).ToArray();
                rules.AddLast(new Pair { First = pair[0], Second = pair[1] });
            }
            else
            { 
                prints.AddLast(line.Split(",").Select(int.Parse).ToArray());
            }
        }
    }

    protected override void Solve()
    {
        var resPartOne = 0;
        var resPartTwo = 0;
        foreach (var print in prints)
        {
            Console.Write($"{string.Join(",", print)} ");
         
            if (ValidatePrint(print))
            {
                Console.WriteLine($"VALID, added {print[print.Length / 2]} to the result");
                resPartOne += print[print.Length / 2];
            }
            else
            {
                Console.Write($"INVALID, Trying to reorder ");
                TryReorderPrint(print);
                if (ValidatePrint(print))
                {
                    var middleValue = print[print.Length / 2];
                    Console.WriteLine($"added {middleValue}");
                    resPartTwo += middleValue;
                }
                else
                {
                    Console.WriteLine($"failed");
                }
            }
        }

        Console.WriteLine($"Task One result: {resPartOne}");
        Console.WriteLine($"Task Two result: {resPartTwo}");
    }

    private int[] TryReorderPrint(int[] print)
    {
        var changed = true;
        while (changed)
        {
            changed = false;
            for (int i = 0; i < print.Length - 1; i++)
            {
                if (!rules.Contains(new Pair { First = print[i], Second = print[i + 1] }))
                {
                    print[i] = print[i] + print[i + 1];
                    print[i + 1] = print[i] - print[i + 1];
                    print[i] = print[i] - print[i + 1];
                    changed = true;
                }
            }
        }

        return print;
    }

    private bool ValidatePrint(int[] print)
    {
        var valid = true;

        for (int i = 0; i < print.Length - 1; i++)
        {
            var pair = new Pair { First = print[i], Second = print[i + 1] };
            if (rules.Contains(pair))
            {
                continue;
            }

            valid = false;
            break;
        }

        return valid;
    }
}

public struct Pair
{
    public int First { get; set; }
    public int Second { get; set; }
}