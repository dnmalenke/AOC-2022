using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day8)}")]
    public class Day8 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";
            int sum = 0;
            int height = _input.Split('\n').Length;
            int width = _input.Split('\n')[0].Length;

            int[,] forest = new int[width, height];
            int y = 0;
            foreach (var line in _input.Split('\n'))
            {
                int x = 0;
                foreach (var c in line)
                {
                    forest[x, y] = int.Parse(c.ToString());
                    x++;
                }
                y++;
            }

            List<Point> points = new List<Point>();
            Dictionary<Point, int> scores = new Dictionary<Point, int>();
            // top
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (j == 0)
                    {
                        points.Add(new(i, j));
                        scores.TryAdd(new(i, j), 0);
                        scores[new(i, j)] *= 0;
                    }
                    else
                    {
                        bool fail = false;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (forest[i, k] >= forest[i, j])
                            {
                                fail = true;
                                break;
                            }
                        }

                        if (!fail)
                        {
                            points.Add(new(i, j));
                            scores.TryAdd(new(i, j), 1);
                            scores[new(i, j)] *= j;

                        }
                    }
                }
            }

            // bottom
            for (int i = 0; i < width; i++)
            {
                for (int j = height - 1; j >= 0; j--)
                {
                    if (j == height - 1)
                    {
                        points.Add(new(i, j));
                        scores.TryAdd(new(i, j), 0);
                        scores[new(i, j)] *= 0;
                    }
                    else
                    {
                        bool fail = false;
                        for (int k = j + 1; k < height; k++)
                        {
                            if (forest[i, k] >= forest[i, j])
                            {
                                fail = true;
                                break;
                            }
                        }

                        if (!fail)
                        {
                            points.Add(new(i, j));
                            scores.TryAdd(new(i, j), 1);
                            scores[new(i, j)] *= ((height - 1) - j);
                        }
                    }
                }
            }

            // left
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j == 0)
                    {
                        points.Add(new(j, i));
                        scores.TryAdd(new(j, i), 0);
                        scores[new(j, i)] *= 0;
                    }
                    else
                    {
                        bool fail = false;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (forest[k, i] >= forest[j, i])
                            {
                                fail = true;
                                break;
                            }
                        }

                        if (!fail)
                        {
                            points.Add(new(j, i));
                            scores.TryAdd(new(j, i), 1);
                            scores[new(j, i)] *= j;

                        }
                    }
                }
            }

            // right
            for (int i = 0; i < height; i++)
            {
                for (int j = width - 1; j >= 0; j--)
                {
                    if (j == width - 1)
                    {
                        points.Add(new(j, i));
                        scores.TryAdd(new(j, i), 0);
                        scores[new(j, i)] *= 0;
                    }
                    else
                    {
                        bool fail = false;
                        for (int k = j + 1; k < width; k++)
                        {
                            if (forest[k, i] >= forest[j, i])
                            {
                                fail = true;
                                break;
                            }
                        }

                        if (!fail)
                        {
                            points.Add(new(j, i));
                            scores.TryAdd(new(j, i), 1);
                            scores[new(j, i)] *= ((width - 1) - j);

                        }
                    }
                }
            }

            sum = points.Distinct().Count();

            var d = points.Distinct().ToList();

            int max = 0;

            _result += $"\npart 1 sum: {sum}";

            scores = new();

            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int xxx = 1;
                    int s = 0;
                    // look up
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (forest[i, k] >= forest[i, j])
                        {
                            s++;
                            break;
                        }
                        s++;
                    }

                    xxx *= s;
                    s = 0;
                    // look down
                    for (int k = j + 1; k < height; k++)
                    {
                        if (forest[i, k] >= forest[i, j])
                        {
                            s++;
                            break;
                        }
                        s++;
                    }

                    xxx *= s;
                    s = 0;

                    // look left
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (forest[k, j] >= forest[i, j])
                        {
                            s++;
                            break;
                        }
                        s++;
                    }

                    xxx *= s;
                    s = 0;

                    // look right
                    for (int k = i + 1; k < width; k++)
                    {
                        if (forest[k, j] >= forest[i, j])
                        {
                            s++;
                            break;
                        }
                        s++;
                    }

                    xxx *= s;
                    s = 0;

                    scores.Add(new(i, j), xxx);
                }
            }

            _result += $"\npart 2 max: {scores.MaxBy(x => x.Value).Key.X},{scores.MaxBy(x => x.Value).Key.Y} : {scores.MaxBy(x => x.Value).Value}";

        }

        private struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
