namespace _15;

public class Program2
{
    public static void Main()
    {
        string? s;
        var inputLen = 100;
        var len = 500;

        var map = new int[inputLen, inputLen];
        var mapV2 = new int[500, 500];
        var neighbors = new int[len*len, 5];

        using (var streamReader = new StreamReader("input.txt"))
        {
            for (int i = 0; i < inputLen; i++)
            {
                s = streamReader.ReadLine();
                for (int j = 0; j < inputLen; j++)
                {
                    var num = int.Parse(s[j].ToString());
                    map[i, j] = num;
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            mapV2[k * inputLen + i, l * inputLen +j] = (num + k + l - 1) % 9 + 1;
                        }
                    }
                }
            }
        }

        FillNeighbors(len, mapV2, neighbors);
        Console.WriteLine("Prepared. Go");

        var distances = Dijkstra(neighbors, 0, len);
        Console.WriteLine(distances[len*len-1]);
    }

    private static void FillNeighbors(int len, int[,] map, int[,] neighbors)
    {
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                var index = len * i + j;
                if (j < len - 1)
                {
                    var cost = map[i, j + 1];
                    neighbors[index, 0] = cost;
                }

                if (j > 0)
                {
                    var cost = map[i, j - 1];
                    neighbors[index, 1] = cost;
                }

                if (i < len - 1)
                {
                    var cost = map[i + 1, j];
                    neighbors[index, 2] = cost;
                }

                if (i > 0)
                {
                    var cost = map[i - 1, j];
                    neighbors[index, 3] = cost;
                }
            }
        }
    }

    public static int[] Dijkstra(int[,] graph, int src, int len)
    {
        var length = graph.GetLength(0);
        int[] dist = new int[length]; // The output array. dist[i]
                                                  // will hold the shortest
                                                  // distance from src to i

        // sptSet[i] will true if vertex
        // i is included in shortest path
        // tree or shortest distance from
        // src to i is finalized
        bool[] processed = new bool[dist.Length];

        // Initialize all distances as
        // INFINITE and stpSet[] as false
        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = int.MaxValue;
            processed[i] = false;
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
            int pickedVertexIndex = GetMinDistance(dist, processed);

            // Mark the picked vertex as processed
            processed[pickedVertexIndex] = true;

            // Update dist value of the adjacent
            // vertices of the picked vertex.
            //for (int v = 0; v < dist.Length; v++)
            for (int v = 0; v < 4; v++)

                // Update dist[v] only if is not in
                // sptSet, there is an edge from u
                // to v, and total weight of path
                // from src to v through u is smaller
                // than current value of dist[v]
            {
                int neighborIndex;
                if (v == 0)
                {
                    neighborIndex = pickedVertexIndex + 1;
                }
                else if (v == 1)
                {
                    neighborIndex = pickedVertexIndex - 1;
                }
                else if (v == 2)
                {
                    neighborIndex = pickedVertexIndex + len;
                }
                else if (v == 3)
                {
                    neighborIndex = pickedVertexIndex - len;
                }
                else
                {
                    throw new Exception();
                }

                if (neighborIndex < 0 || neighborIndex >= length)
                {
                    continue;
                }

                if (!processed[neighborIndex] 
                    && graph[pickedVertexIndex,v] != 0 
                    && dist[pickedVertexIndex] != int.MaxValue 
                    && dist[pickedVertexIndex] + graph[pickedVertexIndex, v] < dist[neighborIndex])
                {
                    dist[neighborIndex] = dist[pickedVertexIndex] + graph[pickedVertexIndex, v];
                }
            }

            //if (pickedVertexIndex == length - 1 && dist[pickedVertexIndex] != int.MaxValue)
            //{
            //    return dist;
            //}
            if (count % 100 == 0)
            {
                
                Console.WriteLine(count);
            }
        }

        // print the constructed distance array
        //printSolution(dist, V);
        return dist;
    }

    static int GetMinDistance(int[] distances, bool[] processed)
    {
        // Initialize min value
        int min = int.MaxValue, minIndex = -1;

        for (var v = 0; v < distances.Length; v++)
        {
            if (processed[v] == false && distances[v] <= min)
            {
                min = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }
}