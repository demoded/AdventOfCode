using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    internal class Solution
    {
        public static void Run()
        {
            var perimeterPositions =  new List<(int x, int y)> {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1), (0, 1),
                (1, -1), (1, 0), (1, 1)
            };
            char[] specialChars = "!@#$%^&*()+-=/:;'<>,?[]{}".ToCharArray();
            char[][] data = null;
            int row = 0;
            int currentNumber = 0;
            bool isPartNumber = false;
            int totalSum = 0;
            var gears = new Dictionary<string, List<int>>();
            var gearPosition = (0, 0);

            // read data from file
            foreach(var line in System.IO.File.ReadLines(@"..\..\..\input.txt"))
            {
                row++;
                if(data == null)
                {
                    //create data array with 2 extra rows and columns to avoid bounds checking
                    data = new char[line.Length + 2][];
                    data[0] = Enumerable.Repeat('.', line.Length + 2).ToArray();
                }

                data[row] = string.Format(".{0}.", line).ToCharArray();
            }
            data[++row] = Enumerable.Repeat('.', data[0].Length).ToArray();

            for (int y = 1; y < data.Length - 1; y++)
            {
                for (int x = 1; x < data.Length - 1; x++)
                {
                    var currentChar = data[y][x];
                    var currentDigit = currentChar - '0';

                    if (currentDigit >= 0 && currentDigit <= 9)
                    {
                        // compose number from digits
                        currentNumber = currentNumber * 10 + currentDigit;

                        //check if sideChars contains special chars
                        foreach (var position in perimeterPositions)
                        {
                            if (specialChars.Contains(data[y + position.y][x + position.x]))
                            { 
                                isPartNumber = true;
                            }

                            if(data[y + position.y][x + position.x] == '*')
                            {
                                gearPosition = (x + position.x, y + position.y);
                            }
                        }
                    }
                    else
                    {
                        AddToGears();
                        AddToTotalSum();
                    }
                }
                AddToGears();
                AddToTotalSum();
            }

            Console.WriteLine($"Total sum of partnumbers:{totalSum}");

            var gearsSum = gears.Where(g => g.Value.Count > 1).Select(x => x.Value[0] * x.Value[1]).ToList().Sum();
            Console.WriteLine($"Gears:{gearsSum}");

            void AddToGears()
            {
                if(gearPosition.Item1 == 0 && gearPosition.Item2 == 0)
                {
                    return;
                }

                var key = string.Concat(gearPosition.Item1, 'x', gearPosition.Item2);
                if(gears.ContainsKey(key))
                {
                    gears[key].Add(currentNumber);
                }
                else
                {
                    gears.Add(key, new List<int> { currentNumber });
                }

                gearPosition = (0, 0);
            }

            void AddToTotalSum()
            {
                //finish processing current number
                if (isPartNumber)
                {
                    totalSum += currentNumber;
                    //Console.WriteLine($"Found partnumber:{currentNumber}");
                }
                currentNumber = 0;
                isPartNumber = false;
            }
        }
    }
}
