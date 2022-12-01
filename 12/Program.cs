string? s;
var ss = new List<string>();
using (var streamReader = new StreamReader("input.txt"))
{
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        ss.Add(s);
    }

}

var neighbors = new Dictionary<string, List<string>>();
foreach (var caveLink in ss)
{
    var cave1 = caveLink.Split('-')[0];
    var cave2 = caveLink.Split('-')[1];
    if (neighbors.ContainsKey(cave1))
    {
        neighbors[cave1].Add(cave2);
    }
    else
    {
        neighbors.Add(cave1, new List<string>() { cave2 });
    }
    if (neighbors.ContainsKey(cave2))
    {
        neighbors[cave2].Add(cave1);
    }
    else
    {
        neighbors.Add(cave2, new List<string>() { cave1 });
    }
}

var paths = new List<string>();
foreach (var cave in neighbors["start"])
{
    var currentPath = new List<string>()
    {
        "start"
    };
    Next(cave, currentPath.ToList(), true);
}

foreach (var path in paths)
{
    Console.WriteLine(path);
}
Console.WriteLine(paths.Count);

void Next(string currentCave, List<string> currentPath, bool canVisitSmallCaveTwice)
{
    currentPath.Add(currentCave);
    if (currentCave == "end")
    {
        paths.Add(string.Join(',', currentPath));
        return;
    }

    foreach (var cave in neighbors[currentCave])
    {
        var currentFlag = canVisitSmallCaveTwice;
        if(cave.ToLower() == cave && currentPath.Contains(cave))
        {
            if (cave == "start")
            {
                continue;
            }

            if (!currentFlag)
            {
                continue;
            }
            else
            {
                currentFlag = false;
            }
        }
        Next(cave, currentPath.ToList(), currentFlag);
    }
}