
using System;
using System.Runtime.CompilerServices;

internal class Solution
{
    internal static void Run()
    {
        string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");
        int sum = 0;
        int matches = 0;
        int[] cardMatches = new int[lines.Length];
        var copyScratchQueue = new Queue<int>();
        int totalScratches = 0;

        // Part 1
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            var scratchcard = new Scratchcard(line);
            matches = scratchcard.CountMatches();
            cardMatches[i] = matches;
            sum += (int)Math.Pow(2, (double)matches - 1);
        }

        // Part 2
        for (int i = 0; i < cardMatches.Length; i++)
        {
            copyScratchQueue.Enqueue(i);

            while (copyScratchQueue.Count > 0)
            { 
                totalScratches++;
                var index = copyScratchQueue.Dequeue();
                var bonus = cardMatches[index];
                if (bonus > 0)
                {
                    Enumerable.Range(index+1, bonus).ToList().ForEach(copyScratchQueue.Enqueue);
                }
            }
        }

        Console.WriteLine($"Sum of all points: {sum}");
        Console.WriteLine($"Total scratches: {totalScratches}");
    }
}

internal struct Scratchcard
{
    int[] winningNumbers;
    int[] testNumbers;

    public Scratchcard(string line)
    {
        var semicolonPos = line.IndexOf(':');
        var pipePos = line.IndexOf('|');

        testNumbers = line.AsSpan().Slice(pipePos + 1).ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
        winningNumbers = line.AsSpan().Slice(semicolonPos + 1, pipePos - semicolonPos - 1).ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
    }

    internal int CountMatches()
    {
        int winPos = 0;
        int testPos = 0;
        int matches = 0;

        Array.Sort(winningNumbers);
        Array.Sort(testNumbers);

        while (winPos < winningNumbers.Length && testPos < testNumbers.Length)
        {
            if (winningNumbers[winPos] == testNumbers[testPos])
            {
                matches++;
                winPos++;
                testPos++;
            }
            else if (winningNumbers[winPos] < testNumbers[testPos])
            {
                winPos++;
            }
            else
            {
                testPos++;
            }
        }

        return matches;
    }
}