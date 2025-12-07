internal interface ISolvable
{
    internal string Solve();
}

internal abstract class AOCRunner<T> where T : ISolvable, new()
{
    internal string[] _example;
    internal string[] _input;

    public AOCRunner() : base() 
    {
        _example = File.ReadAllLines($"..\\..\\..\\{typeof(T).Name}\\example.txt");
        _input = File.ReadAllLines($"..\\..\\..\\{typeof(T).Name}\\input.txt");
    }

    internal static string Run()
    {
        return new T().Solve();
    }
}

