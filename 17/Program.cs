

var current = (0d, 0d);
var currentX = 1d;
var targetMinX = 235;
var targetMaxX = 259;
var targetMinY = -62;
var targetMaxY = -118;
var n = 0;
var validYs = new List<(int,int)>();
double x = 0;
while (x <= targetMaxX)
{
    n++;
    x = n / 2d * (currentX + n);
    if (x < targetMinX || x > targetMaxX)
    {
        continue;
    }

    var startY = 0;
    double y = 0;
    while(y >= targetMaxY)
    {
        startY++;
        y = n / 2d * (startY - (n - startY) + 1);
        if (y < targetMinY || y > targetMaxY)
        {
            continue;
        }
        validYs.Add((n, startY));
    }
}