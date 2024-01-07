

internal class Solution
{
    internal static void Run()
    {
        var handValueWithBid = new List<(int, int, string)>();

        foreach (var line in File.ReadAllLines("../../../input.txt"))
        {
            var lineParts = line.Split(" ");
            var hand = StringToHand(lineParts[0]);
            var bid = int.Parse(lineParts[1]);
            handValueWithBid.Add((HandValue(hand), bid, lineParts[0]));

            //Console.WriteLine($"{lineParts[0]} => [{string.Join(",", hand)}] => {HandValue(hand)}");
        }

        int result = 0;
        handValueWithBid = handValueWithBid.OrderBy(x => x.Item1).ToList();
        for (int i = 0; i < handValueWithBid.Count; i++)
        {
            //Console.WriteLine($"{handValueWithBid[i].Item1} => {handValueWithBid[i].Item2} bid {handValueWithBid[i].Item3}");
            result += (i + 1) * handValueWithBid[i].Item2;
        }

        Console.WriteLine($"Part 1 result: {result}");
    }

    internal static void RunPart2()
    {
        var handValueWithBid = new List<(int, int, string)>();

        foreach (var line in File.ReadAllLines("../../../input.txt"))
        {
            var lineParts = line.Split(" ");
            var hand = StringToHand(lineParts[0]);
            var bid = int.Parse(lineParts[1]);
            handValueWithBid.Add((HandValue(hand, useJockers: true), bid, lineParts[0]));

            //Console.WriteLine($"{lineParts[0]} => [{string.Join(",", hand)}] => {HandValue(hand, useJockers: true)}");
        }

        int result = 0;
        handValueWithBid = handValueWithBid.OrderBy(x => x.Item1).ToList();
        for (int i = 0; i < handValueWithBid.Count; i++)
        {
            //Console.WriteLine($"{handValueWithBid[i].Item1} => {handValueWithBid[i].Item2} bid {handValueWithBid[i].Item3}");
            result += (i + 1) * handValueWithBid[i].Item2;
        }

        Console.WriteLine($"Part 2 result: {result}");
    }

    private static int HandValue(int[] hand, bool useJockers = false)
    {
        int handValue = 0;
        for (int i = 0; i < hand.Length; i++)
        {
            if (useJockers && hand[i] == 11)
            {
                hand[i] = 1;
            }

            handValue += (int)Math.Pow(14, hand.Length - i - 1) * hand[i];
        }

        int jockersCount = hand.Count(x => x == 1);

        int[] calcHand = new int[hand.Length];
        hand.CopyTo(calcHand, 0);
        Array.Sort(calcHand);

        // 1. Check for five of a kind
        if (calcHand[0] == calcHand[4])
        {
            return 600000000 + handValue;
        }

        // 2. Check for four of a kind
        if (calcHand[0] == calcHand[3] || calcHand[1] == calcHand[4])
        { 
            if (useJockers && jockersCount >= 1)
            {
                return 600000000 + handValue;
            }
            
            return 500000000 + handValue;
        }

        // 3. Check for full house
        if ((calcHand[0] == calcHand[2] && calcHand[3] == calcHand[4])
            || (calcHand[0] == calcHand[1] && calcHand[2] == calcHand[4]))
        { 
            if (useJockers && jockersCount == 1)
            {
                //upgrade to four of a kind 
                return 500000000 + handValue;
            }

            if (useJockers && jockersCount >= 2)
            {
                //upgrade to five of a kind 
                return 600000000 + handValue;
            }

            return 400000000 + handValue;
        }

        // 4. Check for triple
        if (calcHand[0] == calcHand[2] || calcHand[1] == calcHand[3] || calcHand[2] == calcHand[4])
        {
            if ((useJockers && jockersCount == 1) || (useJockers && jockersCount == 3))
            {
                //upgrade to four of a kind 
                return 500000000 + handValue;
            }

            if (useJockers && jockersCount == 2)
            {
                //upgrade to five of a kind 
                return 600000000 + handValue;
            }

            return 300000000 + handValue;
        }

        // 5. Check for two double
        if ((calcHand[0] == calcHand[1] && calcHand[2] == calcHand[3])
            || (calcHand[0] == calcHand[1] && calcHand[3] == calcHand[4])
            || (calcHand[1] == calcHand[2] && calcHand[3] == calcHand[4]))
        { 
            if (useJockers && jockersCount == 1)
            {
                //upgrade to full house 
                return 400000000 + handValue;
            }

            if (useJockers && jockersCount == 2)
            {
                //upgrade to four of a kind 
                return 500000000 + handValue;
            }

            return 200000000 + handValue;
        }
        // 6. Check for double
        if (calcHand[0] == calcHand[1]
            || calcHand[1] == calcHand[2]
            || calcHand[2] == calcHand[3]
            || calcHand[3] == calcHand[4])
        {
            // not jocker pair with jocker
            if(useJockers && jockersCount == 1)
            {
                //upgrade to triple 
                return 300000000 + handValue;
            }

            //jocker pair with other cards different
            if(useJockers && jockersCount == 2)
            {
                //upgrade to triple
                return 300000000 + handValue;
            }

            return 100000000 + handValue;
        }

        return (useJockers && jockersCount == 1 ? 100000000 : 1000000) + handValue ; //Highest card
    }

    private static int[] StringToHand(string v)
    {
        int[] hand = new int[5];

        for (int i = 0; i < 5; i++)
        {
            hand[i] = v[i] switch
            {
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new Exception("Invalid card")
            };
        }

        return hand;
    }
}