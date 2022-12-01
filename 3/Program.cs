string? s;
var gamma = new int[12];
var ss = new List<string>();
string gammaString;
using (var streamReader = new StreamReader("input.txt"))
{
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        ss.Add(s);
    }

    for (int i = 0; i < ss.First().Length; i++)
    {
        gamma[i] = GetMostCommonValueOfBit(ss, i);
    }

    gammaString = string.Join("", gamma);
    var gammaInt = Convert.ToUInt32(gammaString, 2);
    var epsilonInt = ~gammaInt & 0x00000FFF;

    var epsilonString = Convert.ToString(epsilonInt, 2);
    Console.WriteLine($"Gamma {gammaString} {gammaInt}, epsilon {epsilonString} {epsilonInt}, power {gammaInt * epsilonInt}");
}

// part2
int index = 0;
var current = ss.ToList();
while (current.Count > 1)
{
    current = current.Where(x => x[index].ToString() == GetMostCommonValueOfBit(current, index).ToString()).ToList();
    index++;
    if (index == 12)
    {
        index = 0;
    }
}

var oxygenRating = current.Single();
var oxygenRatingInt = Convert.ToUInt32(oxygenRating, 2);
Console.WriteLine($"{oxygenRating} {oxygenRatingInt}");


index = 0;
current = ss.ToList();
while (current.Count > 1)
{
    current = current.Where(x => x[index].ToString() == (~GetMostCommonValueOfBit(current, index) & 0x00000001).ToString()).ToList();
    index++;
    if (index == 12)
    {
        index = 0;
    }
}

var o2ScrubberRating = current.Single();
var o2ScrubberRatingInt = Convert.ToUInt32(o2ScrubberRating, 2);
Console.WriteLine($"{o2ScrubberRating} {o2ScrubberRatingInt}");

var lifeSupportRating = oxygenRatingInt * o2ScrubberRatingInt;
Console.WriteLine($"{lifeSupportRating}");


int GetMostCommonValueOfBit(List<string> list, int bitIndex)
{
    return (int)(list.Select(x => int.Parse(x[bitIndex].ToString())).Sum() / (double)list.Count * 2);
}