using System.Collections;

string? s;
var boards = new List<int[,]>();
using (var streamReader = new StreamReader("input.txt"))
{
    var numbers = streamReader.ReadLine().Split(',').Select(int.Parse).ToList();
    streamReader.ReadLine();

    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        var board = new int[5, 5];

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                board[i, j] = int.Parse(s.Substring(j * 3, 2));
            }

            s = streamReader.ReadLine();
        }
        boards.Add(board);
    }

    var nonWinners = boards.ToList();
    foreach (var number in numbers)
    {
        MarkNumber(number);
        var winners = GetWinners(nonWinners).ToList();

        foreach (var winner in winners)
        {
            nonWinners.Remove(winner);
        }

        if (!nonWinners.Any())
        {
            var winner = winners.Last();
            var sum = winner.Cast<int>().Where(i => i != -1).Sum();
            Console.WriteLine($"Sum {sum}, number {number}, score {sum * number}");
            return;
        }
    }

}

IEnumerable<int[,]> GetWinners(List<int[,]> boards)
{
    foreach (var board in boards)
    {
        var columnWin = Enumerable.Range(0, 5).Select(c => Enumerable.Range(0, 5).Select(r => board[r, c]).Sum()).Any(x => x == -5);
        var rowWin = Enumerable.Range(0, 5).Select(r => Enumerable.Range(0, 5).Select(c => board[r, c]).Sum()).Any(x => x == -5);

        if (columnWin || rowWin)
        {
            yield return board;
        }
    }
}

void MarkNumber(int number)
{
    foreach (var board in boards)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j] == number)
                {
                    board[i, j] = -1;
                }
            }
        }
    }
}