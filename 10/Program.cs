string? s;
var ss = new List<string>();
using (var streamReader = new StreamReader("input.txt"))
{
    var errorScore = 0;
    var completionScores = new List<long>();
    while (!string.IsNullOrEmpty(s = streamReader.ReadLine()))
    {
        var isError = false;
        long completionScore = 0;
        ss.Add(s);
        var stack = new Stack<char>();
        foreach (var ch in s)
        {
            if (ch is '(' or '[' or '{' or '<')
            {
                stack.Push(ch);
            }
            else
            {
                var top = stack.Pop();
                if (ch is ')')
                {
                    if (top != '(')
                    {
                        errorScore += 3;
                        isError = true;
                        break;
                    }
                }

                if (ch is ']')
                {
                    if (top != '[')
                    {
                        errorScore += 57;
                        isError = true;
                        break;
                    }
                }

                if (ch is '}')
                {
                    if (top != '{')
                    {
                        errorScore += 1197;
                        isError = true;
                        break;
                    }
                }

                if (ch is '>')
                {
                    if (top != '<')
                    {
                        errorScore += 25137;
                        isError = true;
                        break;
                    }
                }
            }
        }

        if (!isError)
        {
            foreach (var ch in stack)
            {
                if (ch is '(')
                {
                    completionScore *= 5;
                    completionScore += 1;
                }

                else if (ch is '[')
                {
                    completionScore *= 5;
                    completionScore += 2;
                }

                else if (ch is '{')
                {
                    completionScore *= 5;
                    completionScore += 3;
                }

                else if (ch is '<')
                {
                    completionScore *= 5;
                    completionScore += 4;
                }
            }

            completionScores.Add(completionScore);
        }
    }

    Console.WriteLine(errorScore);
    completionScores = completionScores.OrderBy(x=>x).ToList();
    Console.WriteLine(completionScores[completionScores.Count / 2]);
}