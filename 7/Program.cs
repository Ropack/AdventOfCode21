string? s;
using (var streamReader = new StreamReader("input.txt"))
{
    var positions = streamReader.ReadLine().Split(',').Select(int.Parse).ToList();

    int? min = null;
    for (int i = positions.Min(); i < positions.Max(); i++)
    {
        var score = 0;
        foreach (var position in positions)
        {
            var distance = Math.Abs(i - position);
            score += (int)(distance / (double)2 * (distance + 1));

        }

        if (min > score || min is null)
        {
            min = score;
        }
    }

    Console.WriteLine(min);
}