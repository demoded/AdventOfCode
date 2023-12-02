using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    internal class Task1
    {
        public static void Run()
        {
            int totalNumbersSum = 0;
            int powersSum = 0;

            foreach(var line in System.IO.File.ReadLines(@"..\..\..\input_data.txt"))
            {
                var currentLine = line.AsSpan();
                var gameName = currentLine.Slice(0, currentLine.IndexOf(':')); // Game 1
                var gameData = currentLine.Slice(currentLine.IndexOf(':') + 1); // 1 red, 2 blue, 3 green; 4 red, 5 blue, etc

                //select only digits from gameName
                var gameNumber = gameName.Trim("Game ".ToCharArray()).ToString();

                bool isGameValid = true;
                int minRed = 0, minGreen = 0, minBlue = 0;

                foreach (var hand in gameData.ToString().Split(';'))
                {
                    foreach(var colorCounts in hand.Trim().Split(','))
                    {
                        int redCount = 0, greenCount = 0, blueCount = 0;

                        var colorPair = colorCounts.Trim().Split(' ');
                        var count = int.Parse(colorPair[0]);
                        var color = colorPair[1];
                        switch(color)
                        {
                            case "red":
                                redCount += count;
                                minRed = Math.Max(minRed == 0 ? redCount : minRed, redCount);
                                break;
                            case "green":
                                greenCount += count;
                                minGreen = Math.Max(minGreen == 0 ? greenCount : minGreen, greenCount);
                                break;
                            case "blue":
                                blueCount += count;
                                minBlue = Math.Max(minBlue == 0 ? blueCount : minBlue, blueCount);
                                break;
                        }
                        isGameValid = isGameValid && redCount <= 12 && greenCount <= 13 && blueCount <= 14;
                    }
                }

                if (isGameValid)
                {
                    totalNumbersSum += int.Parse(gameNumber);
                }

                powersSum += minRed * minGreen * minBlue;

                //Console.WriteLine(gameData.ToString());
                //Console.WriteLine($"Game {gameNumber:3} is {(isGameValid ? " VAL " : "INVAL")} MinRGB {minRed}, {minGreen}, {minBlue}. Power {minRed * minGreen * minBlue}. PowerSum {powersSum}");
            }

            Console.WriteLine($"Total sum of valid games is {totalNumbersSum}");
            Console.WriteLine($"Sum of powers is {powersSum}");
        }
    }
}
