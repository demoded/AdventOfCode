using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    internal class Task1
    {
        public static void Run()
        {
            string[] numberNames = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var numberDict = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };
            string startChars = "otfsen";
            string endChars = "eorxnt";

            int firstPos, lastPos;
            long totalSum1 = 0, totalSum2 = 0;
            int lineSum = 0;

            foreach (var line in System.IO.File.ReadLines(@"..\..\..\input_task1.txt"))
            {
                //Task one finding digits in a string
                var currentLine = line.AsSpan();
                lastPos = currentLine.LastIndexOfAny("01234567890".ToCharArray());
                firstPos = currentLine.IndexOfAny("01234567890".ToCharArray());
                lineSum = 10 * (currentLine[firstPos] - 48) + (currentLine[lastPos] - 48);
                totalSum1 += lineSum;

                //Task two finding number names in a string
                int i = 0;
                int firstNumber = 0, lastNumber = 0;

                // loop through the string from the beginning and the end simultaneously                
                do
                {
                    // check string characters from the beginning
                    // if the digit found in the current position then take it as a first number for sum
                    if (firstNumber == 0 && char.IsNumber(currentLine[i]))
                    {
                        firstNumber = currentLine[i] - 48;
                    }
                    
                    // if the digit not found in the current position then try to find if any number names fits in the current position
                    if (firstNumber == 0 && startChars.IndexOf(currentLine[i]) >= 0)
                    { 
                        foreach(var number in numberDict)
                        {
                            if (i + number.Key.Length > currentLine.Length)
                            {
                                continue;
                            }

                            if (currentLine.Slice(i, number.Key.Length).ToString().Equals(number.Key))
                            {
                                firstNumber = number.Value;
                                break;
                            }
                        }
                    }

                    // check string characters from the end
                    // if the digit found in the current position then take it as a last number for sum
                    if (lastNumber == 0 && char.IsNumber(currentLine[currentLine.Length - i - 1]))
                    {
                        lastNumber = currentLine[currentLine.Length - i - 1] - 48;
                    }

                    // if the digit not found in the current position then try to find if any number names fits in the current position
                    if (lastNumber == 0 && endChars.LastIndexOf(currentLine[currentLine.Length - i - 1]) >= 0)
                    {
                        foreach (var number in numberDict)
                        {
                            if (   currentLine.Length <= number.Key.Length
                                || currentLine.Length - i - number.Key.Length < 0)
                            {
                                continue;
                            }

                            if (currentLine.Slice(currentLine.Length - i - number.Key.Length, number.Key.Length).ToString().Equals(number.Key))
                            {
                                lastNumber = number.Value;
                                break;
                            }
                        }
                    }
                    i++;                    
                }
                while (i < currentLine.Length && (firstNumber == 0 || lastNumber == 0));

                totalSum2 += 10*firstNumber + lastNumber;
            }
            Console.WriteLine($"Total sum Task1: {totalSum1}");
            Console.WriteLine($"Total sum Task2: {totalSum2}");
        }
    }
}
