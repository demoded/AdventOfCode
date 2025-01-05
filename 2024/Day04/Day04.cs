using SolutionCore;
using System.Text;
using System.Text.RegularExpressions;

public class Day04 : Solution
{
    char[][] matrix;
    int width;
    int height;
    protected override void ReadInput()
    {
        var text = File.ReadAllText("..\\..\\..\\Day04\\input.txt");
        height = text.Count(c => c == Environment.NewLine[0]) + 1;
        width = text.IndexOf(Environment.NewLine);

        matrix = new char[height][];
        int i = 0;
        foreach (var line in text.Split(Environment.NewLine))
        {
            matrix[i++] = line.ToCharArray();
        }
    }

    protected override void Solve()
    {
        var horizontalText = string.Join(" ", matrix.Select(row => new string(row)));
        var verticalText = string.Join(" ", Enumerable.Range(0, width).Select(col => new string(matrix.Select(row => row[col]).ToArray())));
        var forwardDiagonalText = ForwardDiagonals(matrix);
        var reverseDiagonalText = ForwardDiagonals(matrix.Select(row => row.Reverse().ToArray()).ToArray());

        var res = CountXMAS(horizontalText) + CountXMAS(verticalText) + CountXMAS(forwardDiagonalText) + CountXMAS(reverseDiagonalText);
        Console.WriteLine($"Part One result: {res}");

        res = 0;
        for (int i = 1; i < height-1; i++)
        {
            for (int j = 1; j < width-1; j++)
            {
                if (matrix[i][j] == 'A')
                { 
                    var neigbours = new char[] { matrix[i - 1][j - 1], matrix[i - 1][j + 1], matrix[i + 1][j - 1], matrix[i + 1][j + 1] };
                    if (!neigbours.Contains('X') && !neigbours.Contains('A')
                        && neigbours[0] != neigbours[3] && neigbours[1] != neigbours[2])
                    { 
                        Console.WriteLine($"{neigbours[0]}A{neigbours[3]}, {neigbours[1]}A{neigbours[2]}");
                        res++;
                    }
                }
            }
        }
        Console.WriteLine($"Part Two result: {res}");
    }

    private int CountXMAS(string text)
    {
        return Regex.Matches(text, "XMAS").Count
            + Regex.Matches(text, "SAMX").Count;
    }

    public static string ForwardDiagonals(char[][] matrix)
    {
        if (matrix == null) return string.Empty;

        int rows = matrix.Length;
        int cols = rows > 0 ? matrix[0].Length : 0;
        StringBuilder result = new StringBuilder();

        for (int r = 0; r < rows; r++)
        {
            result.Append(GetDiagonal(matrix, r, 0, rows, cols)).Append(' ');
        }

        for (int c = 1; c < cols; c++)
        {
            result.Append(GetDiagonal(matrix, 0, c, rows, cols)).Append(' ');
        }

        return result.ToString().Trim();
    }

    private static string GetDiagonal(char[][] matrix, int row, int col, int rows, int cols)
    {
        StringBuilder diagonal = new StringBuilder();

        while (row < rows && col < cols)
        {
            diagonal.Append(matrix[row][col]);
            row++;
            col++;
        }

        return diagonal.ToString();
    }
}