using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

string? s;
var rules = new List<string>();
var rulesD = new Dictionary<string, char>();
string template;
using (var streamReader = new StreamReader("input.txt"))
{
    template = streamReader.ReadLine();
    streamReader.ReadLine();

    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        var key = s.Split(" -> ")[0];
        var val = s.Split(" -> ")[1];
        rules.Add(s);
        rulesD.Add(key, val[0]);

    }
}

var ll = new LinkedList<char>(template.ToCharArray());

//for (int step = 0; step < 40; step++)
//{
//    var first = ll.First;
//    var llCount = ll.Count-1;
//    for (var i = 0; i < llCount; i++)
//    {
//        var next = first.Next;
//        var key = $"{first.Value}{next.Value}";
//        var newValue = rulesD[key];
//        ll.AddAfter(first, newValue);
//        first = next;
//    }

//    Console.WriteLine($"After step {step}");
//}

//var result = ll.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Count()).ToList();
//Console.WriteLine($"{result.First()} {result.Last()} {result.First() - result.Last()}");

//var d = new Dictionary<string, int>();
//var subPolymers = new Dictionary<string, LinkedList<char>>();
var polymers = new Dictionary<string, Polymer>();
var polymerList = new List<Polymer>();

var first = ll.First;
var llCount = ll.Count - 1;
for (var i = 0; i < llCount; i++)
{
    var next = first.Next;
    var key = $"{first.Value}{next.Value}";
    first = next;
    if (polymers.ContainsKey(key))
    {
        var oldPolymer = polymers[key];
        polymerList.Add(oldPolymer);
    }
    else
    {
        var polymer = new Polymer(key);
        polymerList.Add(polymer);
        polymers.Add(key, polymer);
    }

}

var maxSteps = 41;
for (int step = 0; step < maxSteps; step++)
{
    foreach (var polymer in polymerList)
    {
        polymer.Expand(polymers, rulesD, step + 1);
    }

    Console.WriteLine($"After step {step}");
}

var mergedDictionary = new Dictionary<char, long>();
foreach (var polymer in polymerList)
{
    mergedDictionary = Helper.MergeDictionaries(mergedDictionary, polymer.GetCharCount(maxSteps-1));
}

mergedDictionary[ll.Last.Value]++;
Console.WriteLine(mergedDictionary.Sum(x=>x.Value));
var result = mergedDictionary.ToList().OrderByDescending(x => x.Value).Select(x => x.Value).ToList();
Console.WriteLine($"{result.First()} {result.Last()} {result.First() - result.Last()}");

[DebuggerDisplay("{Key}")]
public class Polymer
{
    public Polymer(string key, int expandCount = 0)
    {
        Key = key;
        ExpandCount = expandCount;
    }

    public string Key { get; }
    public int ExpandCount { get; set; }

    [MemberNotNullWhen(true, nameof(Polymer1), nameof(Polymer2))]
    public bool IsExpanded { get; set; }
    public Polymer? Polymer1 { get; private set; }
    public Polymer? Polymer2 { get; private set; }
    private Dictionary<int, Dictionary<char, long>> CharCount { get; } = new();
    public Dictionary<char, long> GetCharCount(int depth)
    {
        if (depth == 0)
        {
            return new Dictionary<char, long>()
            {
                {Key[0], 1},
                //{Key[1], 1}
            };
        }
        if (CharCount.ContainsKey(depth))
        {
            return CharCount[depth];
        }

        var d1 = Polymer1.GetCharCount(depth - 1);
        var d2 = Polymer2.GetCharCount(depth - 1);
        var result = Helper.MergeDictionaries(d1, d2);
        CharCount.Add(depth, result);
        return result;
    }

    public void Expand(Dictionary<string, Polymer> polymers, Dictionary<string, char> rules, int expandCount)
    {
        if (expandCount == ExpandCount)
        {
            return;
        }
        ExpandCount++;
        if (IsExpanded)
        {
            Polymer1.Expand(polymers, rules, expandCount);
            Polymer2.Expand(polymers, rules, expandCount);
        }
        else
        {
            IsExpanded = true;
            var newValue = rules[Key];
            var key1 = $"{Key[0]}{newValue}";
            if (polymers.ContainsKey(key1))
            {
                Polymer1 = polymers[key1];
            }
            else
            {
                Polymer1 = new Polymer(key1, expandCount);
                polymers.Add(key1, Polymer1);
            }

            var key2 = $"{newValue}{Key[1]}";
            Polymer2 = new Polymer(key2, expandCount);
            if (polymers.ContainsKey(key2))
            {
                Polymer2 = polymers[key2];
            }
            else
            {
                Polymer2 = new Polymer(key2, expandCount);
                polymers.Add(key2, Polymer2);
            }
        }
    }
}

public static class Helper
{
    public static Dictionary<char, long> MergeDictionaries(Dictionary<char, long> d1, Dictionary<char, long> d2)
    {
        var result = d1.ToDictionary(x => x.Key, x => x.Value);
        foreach (var (key, value) in d2)
        {
            if (result.ContainsKey(key))
            {
                result[key] += value;
            }
            else
            {
                result.Add(key, value);
            }
        }
        return result;
    }
}
