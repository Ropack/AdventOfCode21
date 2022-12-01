string? s;
var board = new int[1000,1000];
using (var streamReader = new StreamReader("input.txt"))
{

    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        var start = s.Split(" -> ")[0];
        var end = s.Split(" -> ")[1];
        var startX = int.Parse(start.Split(",")[0]);
        var startY = int.Parse(start.Split(",")[1]);
        var endX = int.Parse(end.Split(",")[0]);
        var endY = int.Parse(end.Split(",")[1]);

        var directionX = 1;
        var directionY = 1;
        if (startX > endX)
        {
            //(startX, endX) = (endX, startX);
            directionX = -1;
        }

        if (startY > endY)
        {

            //(startY, endY) = (endY, startY);
            directionY = -1;
        }

        if (startX == endX)
        {
            for (int i = startY; i != endY+directionY; i+=directionY)
            {
                board[startX, i]++;
            }
        }
        else if (startY == endY)
        {
            for (int i = startX; i != endX+directionX; i+=directionX)
            {
                board[i, startY]++;
            }
        }
        else
        {
            for (int i = startX, j = startY; i != endX + directionX; i += directionX, j += directionY)
            {
                board[i, j]++;
            }
        }
    }

    var sum = 0;
    foreach (var i in board)
    {
        if (i > 1)
        {
            sum++;
        }
    }

    Console.WriteLine(sum);

}