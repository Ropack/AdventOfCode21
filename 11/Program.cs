string? s;
var octopuses = new int[12,12];
using (var streamReader = new StreamReader("input.txt"))
{
    for (int i =0; i < 12; i++)
    {
        for (int j = 0; j < 12; j++)
        {
            if (i is 0 or 11 || j is 0 or 11)
            {
                octopuses[i, j] = 11;
            } 
        }
    }

    WriteOctopuses(true);
    for (int i = 1; i <= 10; i++)
    {
        s = streamReader.ReadLine();
        for (int j = 1; j <= 10; j++)
        {
            octopuses[i, j] = int.Parse(s[j-1].ToString());
        }
    }
}

Console.WriteLine("Before any steps:");
WriteOctopuses();
var flashes = 0;
for (int step = 0; step < 3000; step++)
{
    var flashesInStep = 0;
    for (int i = 1; i <= 10; i++)
    {
        for (int j = 1; j <= 10; j++)
        {
            octopuses[i, j]++;
            TriggerNeighbors(i, j);
        }
    }

    for (int i = 1; i <= 10; i++)
    {
        for (int j = 1; j <= 10; j++)
        {
            if (octopuses[i, j] > 9)
            {
                flashes++;
                flashesInStep++;
                octopuses[i, j] = 0;
            }
        }
    }
    
    Console.WriteLine($"After step {step+1}:");
    WriteOctopuses();

    if (flashesInStep == 100)
    {
        Console.WriteLine(step+1);
        break;
    }
}

Console.WriteLine(flashes);

void TriggerNeighbors(int i, int j)
{
    if (octopuses[i, j] == 10)
    {
        for (int k = -1; k < 2; k++)
        {
            for (int l = -1; l < 2; l++)
            {
                octopuses[i + k, j + l]++;
                TriggerNeighbors(i + k, j + l);
            }
        }
    }
}

void WriteOctopuses(bool writeBorder = false)
{
    var c = writeBorder ? 0 : 1;
    for (int i =0+c; i < 12-c; i++)
    {
        for (int j = 0+c; j < 12-c; j++)
        {
            Console.Write($"{octopuses[i, j] ,2}");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}