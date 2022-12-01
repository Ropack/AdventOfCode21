using System.Linq;

string? s;
using (var streamReader = new StreamReader("input.txt"))
{
    var sum = 0;
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        var segments = Enumerable.Range(0, 10).ToDictionary(x => x, x => Array.Empty<char>());
        var firstPart = s.Split(" | ")[0];
        var secondPart = s.Split(" | ")[1];
        segments[1] = firstPart.Split(' ').Single(x => x.Length == 2).ToCharArray();
        segments[4] = firstPart.Split(' ').Single(x => x.Length == 4).ToCharArray();
        segments[7] = firstPart.Split(' ').Single(x => x.Length == 3).ToCharArray();
        segments[8] = firstPart.Split(' ').Single(x => x.Length == 7).ToCharArray();

        var zeroOrSixOrNine = firstPart.Split(' ').Where(x => x.Length == 6).Select(x => x.ToCharArray()).ToList();
        segments[6] = zeroOrSixOrNine.Single(x => x.Except(segments[1]).Count() == 5);
        var zeroOrNine = zeroOrSixOrNine.Where(x => x.Except(segments[6]).Any()).ToList();
        segments[0] = zeroOrNine.Single(x => x.Except(segments[4]).Count() == 3);
        segments[9] = zeroOrNine.Single(x => x.Except(segments[4]).Count() == 2);
        

        var twoOrThreeOrFive = firstPart.Split(' ').Where(x => x.Length == 5).ToList();
        segments[3] = twoOrThreeOrFive.Single(x => x.Except(segments[1]).Count() == 3).ToCharArray();
        segments[5] = twoOrThreeOrFive.Single(x => !x.Except(segments[6]).Any()).ToCharArray();
        segments[2] = twoOrThreeOrFive.Single(x => x.Except(segments[9]).Count() == 1).ToCharArray();

        //var top = segments[7].Except(segments[1]).Single();
        //var middleOrTopLeft = segments[4].Except(segments[1]).ToList();

        //var count = secondPart.Split(' ').Count(x => x.Length is 2 or 3 or 4 or 7);
        //sum += count;
        var result = int.Parse(string.Join("",
            secondPart
                .Split(' ')
                .Select(x => segments
                    .Single(seg => seg.Value
                        .All(v => x.ToCharArray().Contains(v)) 
                                   && seg.Value.Length == x.Length)
                    .Key.ToString())));
        sum += result;

    }

    Console.WriteLine(sum);
}