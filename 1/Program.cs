var partResults = new List<int>();
string? s;
var sum = 0;
using (var streamReader = new StreamReader("input.txt"))
{
    s = streamReader.ReadLine();
    var current = Convert.ToInt32(s);
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        var next = Convert.ToInt32(s);
        if (next > current)
        {
            sum++;
        }
        //Console.WriteLine($"{current} -> {next}, {sum}");
        current = next;
    }

    Console.WriteLine($"Measurement increased {sum} times.");
}

// part 2
sum = 0;
using (var streamReader = new StreamReader("input.txt"))
{
    var first = new int[3];
    var second = new int[3];
    first[0] = Convert.ToInt32(streamReader.ReadLine());
    first[1] = Convert.ToInt32(streamReader.ReadLine());
    first[2] = Convert.ToInt32(streamReader.ReadLine());
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        second[0] = first[1];
        second[1] = first[2];
        second[2] = Convert.ToInt32(s);
        if (second.Sum() > first.Sum())
        {
            sum++;
        }
        Console.WriteLine($"{first.Sum()} -> {second.Sum()}, {sum}");
        first = second;
        second = new int[3];
    }

    Console.WriteLine($"Measurement increased {sum} times.");
}

