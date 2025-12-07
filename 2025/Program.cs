internal class Program
{
    private static void Main(string[] args)
    {
        var result = AOCRunner<Day01>.Run();
        Console.WriteLine($"{nameof(Day01)} result: {result}");
    }
}