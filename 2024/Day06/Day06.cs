using SolutionCore;

public class Day06 : Solution
{
    List<char[]> startingMap = new();
    List<char[]> map = new();
    Player guard = new();
    HashSet<Vector2D> visited = new();
    Vector2D startPosition;

    protected override void ReadInput()
    {
        int guardPosition = -1;
        int rowCounter = 0;

        foreach (var line in File.ReadAllLines("..\\..\\..\\Day06\\input.txt"))
        {
            startingMap.Add(line.ToCharArray());

            guardPosition = line.IndexOf('^');
            if (guardPosition >= 0)
            {
                startPosition = new Vector2D(guardPosition, rowCounter);
            }

            rowCounter++;
        }
    }

    protected override void Solve()
    {
        guard.Position = startPosition;
        // copy all values from starting map to map with creating a new instance of char[]
        map = startingMap.Select(row => (char[])row.Clone()).ToList();

        while (GuardOnMap())
        {
            if (CanGoForward())
            {
                map[guard.Position.Y][guard.Position.X] = 'X';
                guard.Position += guard.Direction;
                if (ValueAtPostition(guard.Position) != 'X')
                {
                    visited.Add(guard.Position);
                    guard.Steps++;
                }
            }
            else
            {
                TurnRight();
                //PrintMap();
            }
        }

        Console.WriteLine($"Part One result: {guard.Steps}");

        int partTwoResult = 0;
        visited.Remove(visited.Last()); //removed the last visited position because it is out of bounds

        foreach (var position in visited)
        {
            var loopDetected = 0;
            Dictionary<Vector2D,int> visitedBlocks = new();

            map = startingMap.Select(row => (char[])row.Clone()).ToList();
            map[position.Y][position.X] = '#';

            guard.Position = startPosition;
            guard.Direction = Directions.Clockwise[0];

            while (GuardOnMap())
            {
                if (CanGoForward())
                {
                    map[guard.Position.Y][guard.Position.X] = 'X';
                    guard.Position += guard.Direction;
                    if (ValueAtPostition(guard.Position) != 'X')
                    {
                        guard.Steps++;
                    }
                }
                else
                {
                    var blockPosition = guard.Position + guard.Direction;
                    if (visitedBlocks.TryGetValue(blockPosition, out int value))
                    {
                        visitedBlocks[blockPosition] = ++value;
                        loopDetected = value;

                        if (value > 4) // theoretically we can hit the same block not more than 4 times (from 4 different directions)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // remember the block to detect loops
                        visitedBlocks.Add(blockPosition, 1);
                    }

                    TurnRight();
                }
            }

            if (loopDetected > 4)
            {
                //PrintMap();
                partTwoResult++;
            }
        }

        Console.WriteLine($"Part Two result: {partTwoResult}");
    }

    private void PrintMap()
    {
        Console.SetCursorPosition(0, 0);
        foreach (var row in map)
        {
            Console.WriteLine(row);
        }
    }

    private void TurnRight()
    {
        guard.Direction = Directions.Clockwise[(Array.IndexOf(Directions.Clockwise, guard.Direction) + 1) % 4];
    }

    private bool CanGoForward()
    {
        return ValueAtPostition(guard.Position + guard.Direction) != '#';
    }

    private bool GuardOnMap()
    {
        if (guard.Position.Y < 0 || guard.Position.Y >= map.Count || guard.Position.X < 0 || guard.Position.X >= map[0].Length)
        {
            return false;
        }

        return true;
    }

    private char ValueAtPostition(Vector2D position)
    {
        if (position.X < 0 || position.X >= map[0].Length || position.Y < 0 || position.Y >= map.Count)
        {
            return Char.MinValue;
        }
        return map[position.Y][position.X];
    }
}