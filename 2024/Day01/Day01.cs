using SolutionCore;

public class Day01 : Solution
{
    private List<int> left = new List<int>();
    private List<int> right = new List<int>();

    protected override void ReadInput()
    {
        var lines = File.ReadAllLines("..\\..\\..\\Day01\\input.txt");
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var spacePos = line.AsSpan().IndexOf(' ');
            var leftNum = int.Parse(line.AsSpan(0, spacePos));
            var rightNum = int.Parse(line.AsSpan(spacePos + 1));

            left.Add(leftNum);
            right.Add(rightNum);
        }
    }

    protected override void Solve()
    {
        left.Sort();
        right.Sort();

        var res = left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();

        Console.WriteLine($"Day01 Task 01 result: {res}");

        var rightNumberCount = right.GroupBy(num => num)
                                    .ToDictionary(g => g.Key, g => g.Count());

        res = left.Sum(l => l * (rightNumberCount.ContainsKey(l) ? rightNumberCount[l] : 0));
        Console.WriteLine($"Day01 Task 02 result: {res}");
    }
}