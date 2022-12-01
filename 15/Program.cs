using System.Diagnostics;
using _15;

Program2.Main();
return;

string? s;
var len = 100;

var map = new int[len, len];
var mapV2 = new int[500, 500];
var neighbors = new int[10000, 10000];
var nodes = new List<Node>();
var nodesA = new Node[len, len];
//var neighborsV2 = new int[250000, 250000];
var neighborsV2 = new byte[250000][];
for (var index = 0; index < neighborsV2.Length; index++)
{
    neighborsV2[index] = new byte[250000];
}

using (var streamReader = new StreamReader("input.txt"))
{
    for (int i = 0; i < len; i++)
    {
        s = streamReader.ReadLine();
        for (int j = 0; j < len; j++)
        {
            var num = int.Parse(s[j].ToString());
            map[i, j] = num;
            var node = new Node()
            {
                Id = Guid.NewGuid(),
                Name = $"{i},{j}:{num}",
                Point = new Point()
                {
                    X = i,
                    Y = j
                }
            };
            nodes.Add(node);
            nodesA[i, j] = node;
            for (int k = 0; k < 5; k++)
            {
                for (int l = 0; l < 5; l++)
                {
                    mapV2[i + k, j + l] = (num + k + l - 1) % 9 + 1;
                }
            }
        }
    }
}

for (int i = 0; i < len; i++)
{
    for (int j = 0; j < len; j++)
    {
        var index = len * i + j;
        neighbors[index, index] = 0;
        var node = nodesA[i, j];
        if (j < len - 1)
        {
            var cost = map[i, j + 1];
            neighbors[index, index + 1] = cost;

            var nodeB = nodesA[i, j + 1];
            node.Connections.Add(new Edge() { ConnectedNode = nodeB, Cost = cost, Length = 1 });
        }
        if (j > 0)
        {
            var cost = map[i, j - 1];
            neighbors[index, index - 1] = cost;

            var nodeB = nodesA[i, j - 1];
            node.Connections.Add(new Edge() { ConnectedNode = nodeB, Cost = cost, Length = 1 });
        }
        if (i < len - 1)
        {
            var cost = map[i + 1, j];
            neighbors[index, index + len] = cost;

            var nodeB = nodesA[i + 1, j];
            node.Connections.Add(new Edge() { ConnectedNode = nodeB, Cost = cost, Length = 1 });
        }
        if (i > 0)
        {
            var cost = map[i - 1, j];
            neighbors[index, index - len] = cost;

            var nodeB = nodesA[i - 1, j];
            node.Connections.Add(new Edge() { ConnectedNode = nodeB, Cost = cost, Length = 1 });
        }
    }
}



for (int i = 0; i < len; i++)
{
    for (int j = 0; j < len; j++)
    {
        var index = len * i + j;
        neighborsV2[index][index] = 0;
        if (j < len - 1)
            neighborsV2[index][index + 1] = (byte)map[i, j + 1];
        if (j > 0)
            neighborsV2[index][index - 1] = (byte)map[i, j - 1];
        if (i < len - 1)
            neighborsV2[index][index + len] = (byte)map[i + 1, j];
        if (i > 0)
            neighborsV2[index][index - len] = (byte)map[i - 1, j];
    }
}

var astar = new AStar(new Map()
{
    Nodes = nodes,
    StartNode = nodes.Single(x => x.Point.X == 0 && x.Point.Y == 0),
    EndNode = nodes.Single(x => x.Point.X == len - 1 && x.Point.Y == len - 1),
});
var path = astar.GetShortestPathAstar();
Console.WriteLine(astar.ShortestPathCost);


GFG t = new GFG();
var dist = t.dijkstra(neighborsV2, 0);
Console.WriteLine(dist[249999]);
var sum2 = 0;
foreach (var p in path)
{
    var n = map[p.Point.X, p.Point.Y];
    Console.WriteLine($"{p.Name} {sum2}  {dist[p.Point.X * 100 + p.Point.Y]}");
    sum2 += n;
}
return;


var start = new Tile();
start.Y = 0;//map.FindIndex(x => x.Contains("A"));
start.X = 0;//map[start.Y].IndexOf("A");

var finish = new Tile();
finish.Y = len - 1;
finish.X = len - 1;

start.SetDistance(finish.X, finish.Y);

var activeTiles = new List<Tile>();
activeTiles.Add(start);
var visitedTiles = new List<Tile>();

while (activeTiles.Any())
{
    var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

    if (checkTile.X == finish.X && checkTile.Y == finish.Y)
    {
        //Console.Log(We are at the destination!);
        //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
        //return;
        var tile = checkTile;
        var sum = 0;
        Console.WriteLine(tile.Cost);

        while (true)
        {
            Console.WriteLine($"{tile.X} : {tile.Y}");
            if (map[tile.Y, tile.X] == ' ')
            {
                //var newMapRow = map[tile.Y].ToCharArray();
                //newMapRow[tile.X] = '*';
                //map[tile.Y] = new string(newMapRow);
            }

            sum += tile.Cost;
            tile = tile.Parent;
            if (tile == null)
            {
                //Console.WriteLine("Map looks like :");
                //map.ForEach(x => Console.WriteLine(x));
                //Console.WriteLine("Done!");
                Console.WriteLine(sum);
                return;
            }
        }
        break;
    }

    visitedTiles.Add(checkTile);
    activeTiles.Remove(checkTile);

    var walkableTiles = GetWalkableTiles(map, checkTile, finish);

    foreach (var walkableTile in walkableTiles)
    {
        //We have already visited this tile so we don't need to do so again!
        if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
            continue;

        //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
        if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
        {
            var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
            if (existingTile.CostDistance > checkTile.CostDistance)
            {
                activeTiles.Remove(existingTile);
                activeTiles.Add(walkableTile);
            }
        }
        else
        {
            //We've never seen this tile before so add it to the list. 
            activeTiles.Add(walkableTile);
        }
    }
}



//Console.WriteLine("No Path Found!");

//var dist = t.dijkstra(neighbors, 0);
//Console.WriteLine(dist[9999]);
//var dist = t.dijkstra(neighborsV2, 0);
//Console.WriteLine(dist[249999]);


List<Tile> GetWalkableTiles(int[,] map, Tile currentTile, Tile targetTile)
{

    var maxX = map.GetLength(0) - 1;
    var maxY = map.GetLength(1) - 1;
    var possibleTiles = new List<Tile>();
    if (currentTile.X < maxX)
        possibleTiles.Add(
            new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + map[currentTile.X + 1, currentTile.Y] });
    if (currentTile.X > 0)
        possibleTiles.Add(
            new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + map[currentTile.X - 1, currentTile.Y] });
    if (currentTile.Y < maxY)
        possibleTiles.Add(
            new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + map[currentTile.X, currentTile.Y + 1] });
    if (currentTile.Y > 0)
        possibleTiles.Add(
            new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + map[currentTile.X, currentTile.Y - 1] });

    possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));


    return possibleTiles
        .Where(tile => tile.X >= 0 && tile.X <= maxX)
        .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
        //.Where(tile => map[tile.Y,tile.X] == ' ' || map[tile.Y,tile.X] == 'B')
        .ToList();
}

class GFG
{
    // A utility function to find the
    // vertex with minimum distance
    // value, from the set of vertices
    // not yet included in shortest
    // path tree
    //static int V = 10000;
    int minDistance(int[] dist,
                    bool[] sptSet)
    {
        // Initialize min value
        int min = int.MaxValue, min_index = -1;

        for (int v = 0; v < dist.Length; v++)
            if (sptSet[v] == false && dist[v] <= min)
            {
                min = dist[v];
                min_index = v;
            }

        return min_index;
    }

    // A utility function to print
    // the constructed distance array
    void printSolution(int[] dist, int n)
    {
        Console.Write("Vertex	 Distance "
                    + "from Source\n");
        for (int i = 0; i < dist.Length; i++)
            Console.Write(i + " \t\t " + dist[i] + "\n");
    }

    // Function that implements Dijkstra's
    // single source shortest path algorithm
    // for a graph represented using adjacency
    // matrix representation
    //public int[] dijkstra(int[,] graph, int src)
    public int[] dijkstra(byte[][] graph, int src)
    {
        int[] dist = new int[graph.GetLength(0)]; // The output array. dist[i]
                                                  // will hold the shortest
                                                  // distance from src to i

        // sptSet[i] will true if vertex
        // i is included in shortest path
        // tree or shortest distance from
        // src to i is finalized
        bool[] sptSet = new bool[dist.Length];

        // Initialize all distances as
        // INFINITE and stpSet[] as false
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = int.MaxValue;
            sptSet[i] = false;
        }

        // Distance of source vertex
        // from itself is always 0
        dist[src] = 0;

        // Find shortest path for all vertices
        for (int count = 0; count < dist.Length - 1; count++)
        {
            // Pick the minimum distance vertex
            // from the set of vertices not yet
            // processed. u is always equal to
            // src in first iteration.
            int u = minDistance(dist, sptSet);

            // Mark the picked vertex as processed
            sptSet[u] = true;

            // Update dist value of the adjacent
            // vertices of the picked vertex.
            for (int v = 0; v < dist.Length; v++)

                // Update dist[v] only if is not in
                // sptSet, there is an edge from u
                // to v, and total weight of path
                // from src to v through u is smaller
                // than current value of dist[v]
            {
                if (!sptSet[v] && graph[u][v] != 0 &&
                    dist[u] != int.MaxValue && dist[u] + graph[u][v] < dist[v])
                {
                    dist[v] = dist[u] + graph[u][v];
                }
            }
        }

        // print the constructed distance array
        //printSolution(dist, V);
        return dist;
    }
}

// This code is contributed by ChitraNayal

class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Cost { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;
    public Tile Parent { get; set; }

    //The distance is essentially the estimated distance, ignoring walls to our target. 
    //So how many tiles left and right, up and down, ignoring walls, to get there. 
    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
    }
}

public class Map
{
    public static Map Randomize(int nodeCount, int branching, int seed, bool randomWeights)
    {
        var rnd = new Random(seed);
        var map = new Map();

        for (int i = 0; i < nodeCount; i++)
        {
            //var newNode = Node.GetRandom(rnd, i.ToString());
            //if (!newNode.ToCloseToAny(map.Nodes))
            //    map.Nodes.Add(newNode);
        }

        foreach (var node in map.Nodes)
            node.ConnectClosestNodes(map.Nodes, branching, rnd, randomWeights);
        //map.StartNode = map.Nodes.OrderBy(n => n.Point.X + n.Point.Y).First();
        //map.EndNode = map.Nodes.OrderBy(n => n.Point.X + n.Point.Y).Last();
        map.EndNode = map.Nodes[rnd.Next(map.Nodes.Count - 1)];
        map.StartNode = map.Nodes[rnd.Next(map.Nodes.Count - 1)];

        foreach (var node in map.Nodes)
        {
            Debug.WriteLine($"{node}");
            foreach (var cnn in node.Connections)
            {
                Debug.WriteLine($"{cnn}");
            }
        }
        return map;
    }

    public List<Node> Nodes { get; set; } = new List<Node>();

    public Node StartNode { get; set; }

    public Node EndNode { get; set; }

    public List<Node> ShortestPath { get; set; } = new List<Node>();
}

public class Node
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Point Point { get; set; }
    public List<Edge> Connections { get; set; } = new List<Edge>();

    public double? MinCostToStart { get; set; }
    public Node NearestToStart { get; set; }
    public bool Visited { get; set; }
    public double StraightLineDistanceToEnd { get; set; }

    //internal static Node GetRandom(Random rnd, string name)
    //{
    //    return new Node
    //    {
    //        Point = new Point
    //        {
    //            X = rnd.NextDouble(),
    //            Y = rnd.NextDouble()
    //        },
    //        Id = Guid.NewGuid(),
    //        Name = name
    //    };
    //}

    internal void ConnectClosestNodes(List<Node> nodes, int branching, Random rnd, bool randomWeight)
    {
        var connections = new List<Edge>();
        foreach (var node in nodes)
        {
            if (node.Id == this.Id)
                continue;

            var dist = Math.Sqrt(Math.Pow(Point.X - node.Point.X, 2) + Math.Pow(Point.Y - node.Point.Y, 2));
            connections.Add(new Edge
            {
                ConnectedNode = node,
                Length = dist,
                Cost = randomWeight ? rnd.NextDouble() : dist,
            });
        }
        connections = connections.OrderBy(x => x.Length).ToList();
        var count = 0;
        foreach (var cnn in connections)
        {
            //Connect three closes nodes that are not connected.
            if (!Connections.Any(c => c.ConnectedNode == cnn.ConnectedNode))
                Connections.Add(cnn);
            count++;

            //Make it a two way connection if not already connected
            if (!cnn.ConnectedNode.Connections.Any(cc => cc.ConnectedNode == this))
            {
                var backConnection = new Edge { ConnectedNode = this, Length = cnn.Length };
                cnn.ConnectedNode.Connections.Add(backConnection);
            }
            if (count == branching)
                return;
        }
    }

    public double StraightLineDistanceTo(Node end)
    {
        return Math.Sqrt(Math.Pow(Point.X - end.Point.X, 2) + Math.Pow(Point.Y - end.Point.Y, 2));
    }

    internal bool ToCloseToAny(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            var d = Math.Sqrt(Math.Pow(Point.X - node.Point.X, 2) + Math.Pow(Point.Y - node.Point.Y, 2));
            if (d < 0.01)
                return true;
        }
        return false;
    }
    public override string ToString()
    {
        return Name;
    }
}

public class Edge
{
    public double Length { get; set; }
    public double Cost { get; set; }
    public Node ConnectedNode { get; set; }

    public override string ToString()
    {
        return "-> " + ConnectedNode.ToString();
    }
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
public class AStar
{
    public Map Map { get; set; }
    public Node Start { get; set; }
    public Node End { get; set; }
    public int NodeVisits { get; private set; }
    public double ShortestPathLength { get; set; }
    public double ShortestPathCost { get; private set; }

    public AStar(Map map)
    {
        Map = map;
        End = map.EndNode;
        Start = map.StartNode;
    }


    private void BuildShortestPath(List<Node> list, Node node)
    {
        if (node.NearestToStart == null)
            return;
        list.Add(node.NearestToStart);
        ShortestPathLength += node.Connections.Single(x => x.ConnectedNode == node.NearestToStart).Length;
        ShortestPathCost += node.Connections.Single(x => x.ConnectedNode == node.NearestToStart).Cost;
        BuildShortestPath(list, node.NearestToStart);
    }

    public List<Node> GetShortestPathAstar()
    {
        foreach (var node in Map.Nodes)
            node.StraightLineDistanceToEnd = node.StraightLineDistanceTo(End);
        AstarSearch();
        var shortestPath = new List<Node>();
        shortestPath.Add(End);
        BuildShortestPath(shortestPath, End);
        shortestPath.Reverse();
        return shortestPath;
    }

    private void AstarSearch()
    {
        Start.MinCostToStart = 0;
        var prioQueue = new List<Node>();
        prioQueue.Add(Start);
        do
        {
            prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + x.StraightLineDistanceToEnd).ToList();
            var node = prioQueue.First();
            prioQueue.Remove(node);
            NodeVisits++;
            foreach (var cnn in node.Connections.OrderBy(x => x.Cost))
            {
                var childNode = cnn.ConnectedNode;
                if (childNode.Visited)
                    continue;
                if (childNode.MinCostToStart == null ||
                    node.MinCostToStart + cnn.Cost < childNode.MinCostToStart)
                {
                    childNode.MinCostToStart = node.MinCostToStart + cnn.Cost;
                    childNode.NearestToStart = node;
                    if (!prioQueue.Contains(childNode))
                        prioQueue.Add(childNode);
                }
            }
            node.Visited = true;
            if (node == End)
                return;
        } while (prioQueue.Any());
    }
}