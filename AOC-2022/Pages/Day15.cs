using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day15)}")]
    public class Day15 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";
            int sum = 0;

            List<Sensor> sensors = new();

            foreach (var line in _input.Lines)
            {
                var spl = line.Split(' ');

                sensors.Add(new Sensor(spl[2].ParseNumber(), spl[3].ParseNumber(), spl[8].ParseNumber(), spl[9].ParseNumber()));
            }

            int range = _input.Lines.Length == 14 ? 20 : 4000000;
            int row = _input.Lines.Length == 14 ? 10 : 2000000;

            // if distance from beacon is bigger than distance to row 10 then something

            // distance = max(point.X, point.Y)

            HashSet<Point> points = new HashSet<Point>();

            //for (int x = 0; x <= 4000000; x++)
            //{
            //    for (int yy = 0; yy <= 4000000; yy++)
            //    {
            //        points.Add(new(x, yy));
            //    }
            //}

            Console.WriteLine("created hashset");

            foreach (var s in sensors)
            {
                //  _result += $"\n {s.X}, {s.Y} d: {s.Distance}, n: {s.NumAtY(y)}";

                foreach (var p in s.PointsAtY(row))
                {
                    points.Add(p);
                }

                Console.WriteLine("sensor done");
            }

            foreach (var b in sensors.Select(s => s.Beacon))
            {
                points.Remove(b);
            }

            _result += $"\npart 1: {points.Count}";



            for (int y = 0; y <= range; y++)
            {
                if(y % 1000 == 0)
                    Console.WriteLine($"y: {y}");
                //  var rr = Enumerable.Range(0, 4000000 + 1);
                List<(int min, int max)> vals = new();
                foreach (var s in sensors)
                {
                    int num = s.NumAtY(y);
                    int x = (int)Math.Ceiling(num / 2.0) - num + s.X;
                    int maxX = x + num > range ? range : x + num - 1;

                    int min = x < 0 ? 0 : x;
                    if (maxX < min)
                    {

                    }
                    else
                    {
                        vals.Add((min, maxX));
                    }
                }

                int xxx = 0;

                while (vals.Count > 1)
                {
                    List<(int min, int max)> nVals = new();

                    for (int i = 0; i < vals.Count; i++)
                    {
                        for (int j = i + 1; j < vals.Count; j++)
                        {
                            if (vals[i].max >= vals[j].min - 1 && vals[j].max >= vals[i].min - 1)
                            {
                                if (vals[i].max > vals[j].max)
                                {
                                    if (vals[i].min < vals[j].min)
                                    {
                                        nVals.Add((vals[i].min, vals[i].max));
                                    }
                                    else
                                    {
                                        nVals.Add((vals[j].min, vals[i].max));
                                    }
                                }
                                else
                                {
                                    if (vals[i].min < vals[j].min)
                                    {
                                        nVals.Add((vals[i].min, vals[j].max));
                                    }
                                    else
                                    {
                                        nVals.Add((vals[j].min, vals[j].max));
                                    }
                                }

                                var l = vals[i];
                                var ll = vals[j];

                                vals.Remove(l);
                                vals.Remove(ll);

                                nVals.AddRange(vals);
                                goto end;
                            }

                            if (vals[i].max >= vals[j].max && vals[i].min <= vals[j].min)
                            {
                                vals.RemoveAt(j);
                                goto end;
                            }
                        }
                    }
                end:
                    if (nVals.Count > 0)
                    {
                        vals.Clear();
                        vals.AddRange(nVals);
                    }

                    if (vals.Count == 2)
                    {
                        xxx++;

                        if (xxx == 3)
                        {
                            var v1 = vals[0];
                            var v2 = vals[1];
                            ulong x;

                            if (v1.max < v2.max)
                            {
                                x = (ulong)v1.max + 1;
                            }
                            else
                            {
                                x = (ulong)v2.max + 1;
                            }

                            _result += $"\npart 2:{x} {y} {x * (ulong)4000000 + (ulong)y}";

                            Console.WriteLine($"FINISHED {y} {vals.First().min} {vals.First().max} {vals.Last().min} {vals.Last().max}");
                            range = 0;
                            break;
                        }
                    }
                }
            }

          //  _result += $"\npart 2: {points.First()}";
        }

        public record class Sensor : Point
        {
            public Point Beacon { get; set; }

            public new int Distance
            {
                get
                {
                    return Math.Abs(X - Beacon.X) + Math.Abs(Y - Beacon.Y);
                }
            }

            public int NumAtY(int y)
            {
                if (y >= Y)
                {
                    if (Y + Distance >= y)
                        return ((Math.Abs(Y + Distance - y) + 1) * 2 - 1);

                    return 0;
                }


                if (Y - Distance <= y)
                    return ((Math.Abs(Y - Distance - y) + 1) * 2 - 1);
                return 0;
            }

            public List<Point> PointsAtY(int y)
            {
                int num = NumAtY(y);
                if (num <= 0) return new List<Point>();

                var l = new List<Point>();

                int x = (int)Math.Ceiling(num / 2.0) - num + X;

                for (int i = 0; i < num; i++)
                {
                    l.Add(new Point(x, y));
                    x++;
                }

                return l;
            }
            public Sensor(int x, int y, int bx, int by) : base(x, y)
            {
                Beacon = new Point(bx, by);
            }
        }
    }
}
