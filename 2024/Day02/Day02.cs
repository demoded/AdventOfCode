using SolutionCore;

public class Day02 : Solution
{
    List<int[]> numbers = new List<int[]>();

    protected override void ReadInput()
    {
        foreach (var line in File.ReadAllLines("..\\..\\..\\Day02\\input.txt"))
        {
            var l = line.AsSpan();
            int count = 0;
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    count++;
                }
            }

            int[] lineNumbers = new int[count + 1];


            int index = 0;
            int pos = 0;
            for (int i = 0; i < l.Length; i++)
            {
                if(l[i] == ' ')
                {
                    lineNumbers[index++] = int.Parse(l[pos..i]);
                    pos = i + 1;
                }
            }
            lineNumbers[index] = int.Parse(l[pos..]);

            numbers.Add(lineNumbers);
        }
    }

    protected override void Solve()
    {
        int partOneResult = 0;
        foreach (var line in numbers)
        {
            partOneResult += IsValidLine(line) ? 1 : 0;
        }

        Console.WriteLine($"Part One result: {partOneResult}");

        int partTwoResult = 0;
        foreach (var line in numbers)
        {
            var isValidLine = IsValidLine(line);

            if (isValidLine is false)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (IsValidLine(line.Where((x, idx) => idx != i).ToArray()) is true)
                    {
                        isValidLine = true;
                        break;
                    }
                }
            }

            partTwoResult += isValidLine ? 1 : 0;
        }

        Console.WriteLine($"Part Two result: {partTwoResult}");
    }

    private bool IsValidLine(int[] line)
    {
        var sign = GetSequienceSign(line);

        for (int i = 1; i < line.Length; i++)
        {
            int diff = line[i] - line[i - 1];
            if (Math.Abs(diff) > 3 || Math.Sign(diff) == 0 || Math.Sign(diff) != sign)
            {
                return false;
            }
        }

        return true;
    }

    private int GetSequienceSign(int[] line)
    {        
        for(int i = 1; i < line.Length; i++)
        {
            if (Math.Sign(line[i] - line[i - 1]) != 0)
            {
                return Math.Sign(line[i] - line[i - 1]);
            }
        }

        return 0;
    }
}
