using SolutionCore;

public class Program
{
    private static void Main(string[] args)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        Solution.Run<Day07>();

        watch.Stop();
        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
    }
}