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

            List<Sensor> sensors = new();

            foreach (var line in _input.Lines)
            {
                var spl = line.Split(' ');

                sensors.Add(new Sensor(spl[2].ParseNumber(), spl[3].ParseNumber(), spl[8].ParseNumber(), spl[9].ParseNumber()));
            }

            int range = _input.Lines.Length == 14 ? 20 : 4000000;
            int row = _input.Lines.Length == 14 ? 10 : 2000000;

            HashSet<Point> points = new();

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

            LL(sensors, range); // fastest implementation I could come up with

            //  _result += $"\npart 2: {points.First()}";
        }

        public void LL(List<Sensor> sensors, int range)
        {
            for (int y = 0; y <= range; y++)// Parallel.For(0, range + 1, (y) => 
            {
                if (y % 10000 == 0)
                    Console.WriteLine($"y: {y}");
                //  var rr = Enumerable.Range(0, 4000000 + 1);
                LinkedList<(int min, int max)> vals = new(); // adding is O(1) all the time instead of O(n) in List<T>
                foreach (var s in sensors)
                {
                    int num = s.NumAtY(y);
                    int x = ((num + 2 - 1) / 2) - num + s.X; //(int)Math.Ceiling(num / 2.0) ; uses faster ceiling formula
                    int maxX = x + num > range ? range : x + num - 1;

                    int min = x < 0 ? 0 : x;
                    if (maxX < min)
                    {

                    }
                    else
                    {
                        vals.AddLast((min, maxX));
                    }
                }

                int xxx = 0;


#pragma warning disable CS8602 // Dereference of a possibly null reference.
                while (vals.Count > 1)
                {
                    //  List<(int min, int max)> nVals = new();
                    (int min, int max) newVal;
                    var curNode = vals.First;
                    for (int i = 0; i < vals.Count; i++)
                    {
                        var nNode = curNode.Next;
                        for (int j = i + 1; j < vals.Count; j++)
                        {

                            var cv = curNode.Value;
                            var nv = nNode.Value;
                            if (cv.max >= nv.min - 1 && nv.max >= cv.min - 1)
                            {
                                if (cv.max > nv.max)
                                {
                                    if (cv.min < nv.min)
                                    {
                                        vals.Remove(nNode);
                                        goto end;
                                    }
                                    else
                                    {
                                        newVal = (nv.min, cv.max);
                                        vals.Remove(nNode);
                                        vals.Remove(curNode);
                                        vals.AddLast(newVal);
                                        goto end;
                                    }
                                }
                                else
                                {
                                    if (cv.min < nv.min)
                                    {
                                        newVal = (cv.min, nv.max);
                                        vals.Remove(nNode);
                                        vals.Remove(curNode);
                                        vals.AddLast(newVal);
                                        goto end;
                                    }
                                    else
                                    {
                                        vals.Remove(curNode);
                                        goto end;
                                    }
                                }
                            }

                            if (cv.max >= nv.max && cv.min <= nv.min)
                            {
                                vals.Remove(nNode);
                                goto end;
                            }
                            nNode = nNode.Next;
                        }
                        curNode = curNode.Next;
                    }
                end:
                    if (vals.Count == 2)
                    {
                        xxx++;

                        if (xxx == 3)
                        {
                            var v1 = vals.First.Value;
                            var v2 = vals.First.Next.Value;
                            ulong x;

                            if (v1.max < v2.max)
                            {
                                x = (ulong)v1.max + 1;
                            }
                            else
                            {
                                x = (ulong)v2.max + 1;
                            }

                            _result += $"\npart 2:{x} {y} {x * 4000000 + (ulong)y}";

                            Console.WriteLine($"FINISHED {y} {vals.First().min} {vals.First().max} {vals.Last().min} {vals.Last().max}");
                            range = 0;
                            break;
                        }
                    }
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }


        public record class Sensor : Point
        {
            public Point Beacon { get; set; }

            private readonly int _distance;

            public int NumAtY(int y)
            {
                if (y >= Y)
                {
                    if (Y + _distance >= y)
                        return ((Math.Abs(Y + _distance - y) + 1) * 2 - 1);

                    return 0;
                }


                if (Y - _distance <= y)
                    return ((Math.Abs(Y - _distance - y) + 1) * 2 - 1);
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
                _distance = Math.Abs(X - Beacon.X) + Math.Abs(Y - Beacon.Y);
            }
        }
    }
}
