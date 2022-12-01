string? s;
using (var streamReader = new StreamReader("input.txt"))
{
    var fish = streamReader.ReadLine().Split(',').Select(int.Parse).ToList();
    var counts = new long[9];
    foreach (var i in fish)
    {
        counts[i]++;
    }

    for (int i = 0; i < 256; i++)
    {
        var x = counts[0];
        for (int j = 0; j < 8; j++)
        {
            counts[j] = counts[j + 1];
        }

        counts[6] += x;
        counts[8] = x;
    }

    Console.WriteLine(counts.Sum());
}