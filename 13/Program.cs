string? s;
var ss = new List<string>();
var folds = new List<string>();
using (var streamReader = new StreamReader("input.txt"))
{
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        ss.Add(s);
    }

    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        folds.Add(s);
    }
}

var map = new bool[1400, 1000];
foreach (var line in ss)
{
    var x = int.Parse(line.Split(',')[0]);
    var y = int.Parse(line.Split(',')[1]);
    map[x, y] = true;
}

foreach (var fold in folds)
{
    var instruction = fold.Substring("fold along ".Length);
    var direction = instruction.Split('=')[0];
    var position = int.Parse(instruction.Split('=')[1]);

    if (direction == "y")
    {
        var newMap = new bool[map.GetLength(0), position];
        for (int i = 0; i < newMap.GetLength(0); i++)
        {
            for (int j = 0; j < newMap.GetLength(1); j++)
            {
                newMap[i, j] = map[i, j];
            }
        }

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = position + 1; j < map.GetLength(1); j++)
            {
                var newY = j - 2 * (j - position);
                if (newY >= 0)
                {
                    newMap[i, newY] = map[i, newY] | map[i, j];
                }
            }
        }

        Console.WriteLine(newMap.Cast<bool>().Sum(x => x ? 1 : 0));
        //PrintMaps(map, newMap);
        map = newMap;
    }

    if (direction == "x")
    {
        var newMap = new bool[position, map.GetLength(1)];
        for (int i = 0; i < newMap.GetLength(0); i++)
        {
            for (int j = 0; j < newMap.GetLength(1); j++)
            {
                newMap[i, j] = map[i, j];
            }
        }

        for (int i = position + 1; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                var newX = i - 2 * (i - position);
                if (newX >= 0)
                {
                    newMap[newX, j] = map[newX, j] | map[i, j];
                }
            }
        }

        Console.WriteLine(newMap.Cast<bool>().Sum(x => x ? 1 : 0));
        //PrintMaps(map, newMap);
        map = newMap;
    }
}
PrintMaps(map);

void PrintMaps(bool[,] newMap1)
{
    for (int j = 0; j < newMap1.GetLength(1); j++)
    {
        for (int i = 0; i < newMap1.GetLength(0); i++)
        {
            Console.Write(newMap1[i, j] ? "#" : ".");
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}