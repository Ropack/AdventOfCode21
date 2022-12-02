
// puzzle data
var targetMinX = 235;
var targetMaxX = 259;
var targetMinY = -62;
var targetMaxY = -118;

// test data result count should be 112
//var targetMinX = 20;
//var targetMaxX = 30;
//var targetMinY = -5;
//var targetMaxY = -10;


// test data (from reddit, result count should be 2223)
//var targetMinX = 257;
//var targetMaxX = 286;
//var targetMinY = -57;
//var targetMaxY = -101;

var validCoordinates = new List<(int X, long Y)>();


// 22 is the minimum of initial velocity to reach 235 x-coordinate
for (int initialX = 1; initialX < targetMaxX + 1; initialX++)
{
    var numberOfSteps = 0;
    while (true)
    {
        numberOfSteps++;

        var finalVelocityX = initialX >= numberOfSteps ? initialX - numberOfSteps : 0;
        var numberOfMovedStepsInX = initialX >= numberOfSteps ? numberOfSteps : initialX;
        // x-coordinate after specified number of steps
        var finalX = (int)((double)numberOfMovedStepsInX / 2 * (finalVelocityX + initialX + 1));

        if (numberOfSteps > 30000) // just some reasonable amount of steps, no reason why exactly this number
        {
            break;
        }

        if (finalX < targetMinX)
        {
            continue;
        }

        if (finalX > targetMaxX)
        {
            break;
        }

        //Console.WriteLine($"Found initial velocity {initialX} that will be in target X after {numberOfSteps} steps. The finalX is {finalX}.");

        var initialY = -1000L;//-119L;
        while (true)
        {
            initialY++;

            var finalVelocityY = initialY - numberOfSteps;
            var finalY = (long)((double)numberOfSteps / 2 * (finalVelocityY + initialY + 1));

            //Console.WriteLine($"\tTrying to find initial Y. For initial {initialY} the final Y position after {numberOfSteps} steps will be {finalY}.");

            if (finalY < targetMaxY)
            {
                continue;
            }

            if (finalY > targetMinY)
            {
                break;
            }

            validCoordinates.Add((initialX, initialY));
            //Console.WriteLine($"Initial velocity {(initialX, initialY)}, steps {numberOfSteps}");
        }

    }
}

var maxBy = validCoordinates.MaxBy(val => val.Y);
Console.WriteLine(maxBy);

var maxReachedYOnTrajectory = (double)maxBy.Y / 2 * (maxBy.Y + 1);
Console.WriteLine(maxReachedYOnTrajectory);

var distinctStartingVelocities = validCoordinates.Distinct();
Console.WriteLine(distinctStartingVelocities.Count());
//Console.WriteLine(string.Join(",",validCoordinates.GroupBy(x=>x).Where(x=>x.Count() > 1).Select(x=>x.Key).ToList()));

bool IsInTargetX(int value)
{
    if (value < targetMinX || value > targetMaxX)
    {
        return false;
    }

    return true;
}

bool IsInTargetY(int value)
{
    if (value < targetMinY || value > targetMaxY)
    {
        return false;
    }

    return true;
}