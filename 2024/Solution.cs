
namespace SolutionCore
{
    public abstract class Solution
    {
        public static void Run<T>() where T : Solution, new()
        {
            var solution = new T();
            solution.ReadInput();
            solution.Solve();
        }

        protected abstract void ReadInput();

        protected abstract void Solve();
    }

    public class Vector2D
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X + b.X, a.Y + b.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2D other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public static class Directions
    {
        public static int Up = 0;
        public static int Right = 1;
        public static int Down = 2;
        public static int Left = 3;

        public static Vector2D[] Clockwise =
        [
            new Vector2D(0, -1),
            new Vector2D(1, 0),
            new Vector2D(0, 1),
            new Vector2D(-1, 0)
        ];
    };

    public class Player
    {
        public Vector2D Position { get; set; }
        public Vector2D Direction { get; set; }
        public int Steps { get; set; }

        public Player()
        {
            Position = new Vector2D(0, 0);
            Direction = Directions.Clockwise[0];
            Steps = 0;
        }
    }
}