string? s;
var ss = new List<string>();
using (var streamReader = new StreamReader("input.txt"))
{
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        ss.Add(s);
    }

    var lengthX = ss.First().Length;
    var lengthY = ss.Count;
    var map = new int[lengthX, lengthY];
    for (int x = 0; x < lengthX; x++)
    {
        for (int y = 0; y < lengthY; y++)
        {
            map[x, y] = int.Parse(ss[x][y].ToString());
        }
    }

    var maxX = lengthX - 1;
    var maxY = lengthY - 1;

    var lowPoints = new List<(int, int)>();
    var sum = 0;
    for (int x = 0; x < lengthX; x++)
    {
        for (int y = 0; y < lengthY; y++)
        {
            var current = map[x, y];
            var left = x == 0 ? 10 : map[x - 1, y];
            var top = y == 0 ? 10 : map[x, y - 1];
            var right = x == maxX ? 10 : map[x + 1, y];
            var bottom = y == maxY ? 10 : map[x, y + 1];

            if (current < left && current < top && current < right && current < bottom)
            {
                sum += current + 1;
                lowPoints.Add((x, y));
            }
        }
    }

    Console.WriteLine(sum);

    // part2
    var basinCounter = 0;
    var basins = new int[lengthX, lengthY];
    var basinsCount = new Dictionary<int, int>();

    foreach (var lowPoint in lowPoints)
    {
        var x = lowPoint.Item1;
        var y = lowPoint.Item2;
        basinCounter++;
        basins[x, y] = basinCounter;
        basinsCount.Add(basinCounter, 1);
        Search(x, y, -1, 0); //left
        Search(x, y, 1, 0); //right
        Search(x, y, 0, -1); //top
        Search(x, y, 0, 1); //bottom
    }

    var tops = basinsCount.OrderByDescending(x => x.Value).Take(3);
    var product = 1;
    foreach (var t in tops)
    {
        product *= t.Value;
    }
    Console.WriteLine(product);

    void Search(int x, int y, int dirX, int dirY)
    {
        var previous = map[x, y];
        var currX = x + dirX;
        var currY = y + dirY;
        if (currX == -1 || currX == lengthX || currY == -1 || currY == lengthY)
        {
            return;
        }

        if (basins[currX, currY] != 0)
        {
            return;
        }

        var current = map[currX, currY];

        if (current == 9)
        {
            return;
        }

        if (current <= previous)
        {
            return;
        }

        basins[currX, currY] = basins[x, y];
        basinsCount[basins[x, y]]++;
            
        Search(currX, currY, -1, 0);
        Search(currX, currY, 1, 0);
        Search(currX, currY, 0, -1);
        Search(currX, currY, 0, 1);
    }
}

