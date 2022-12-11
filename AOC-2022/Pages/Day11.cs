using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Numerics;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day11)}")]
    public class Day11 : DayTemplate
    {
        public static ulong MagicNumber = 0;

        protected override void Run()
        {
            _result = "";
            int cyc = 0;
            ulong sum = 0;

            var m = ParseIn();


            // var z = 0;
            for (int i = 0; i < 20; i++)
            {
                foreach (var mk in m)
                {
                    mk.Value.Execute(m, true);
                }

                //  Console.WriteLine($"i: {i}");
                // z++;
            }

            var max = m.Values.OrderByDescending(x => x.Inspect).Take(2).ToList();

            sum = max[0].Inspect * max[1].Inspect;

            _result += $"\npart 1 sum: {sum}";

            //foreach (var mk in m.Values)
            //{
            //    _result += $"\n in: {mk.Inspect}";
            //}

            MagicNumber = 1;

            foreach (var item in m.Values.Select(x => x.Test))
            {
                MagicNumber *= item;
            }

            Console.WriteLine($"Magic number: {MagicNumber}");

            m = ParseIn();

            for (int i = 0; i < 10000; i++)
            {
                foreach (var mk in m)
                {
                    mk.Value.Execute(m, false);
                }
            }

            max = m.Values.OrderByDescending(x => x.Inspect).Take(2).ToList();

            sum = max[0].Inspect * max[1].Inspect;
            _result += $"\npart 2: {sum}";


        }

        public Dictionary<int, Monkey> ParseIn()
        {
            Dictionary<int, Monkey> m = new();

            int mNum = 0;

            foreach (var line in _input.Split('\n'))
            {
                if (line.StartsWith("Monkey"))
                {
                    mNum = int.Parse(line.Split(' ')[1].TrimEnd(':'));
                    m.Add(mNum, new());
                }

                if (line.Trim().StartsWith("Starting items"))
                {
                    foreach (var item in line.Trim().Split(' ').Skip(2))
                    {
                        int iNum = int.Parse(item.Trim(','));
                        m[mNum].Items.Add((ulong)iNum);
                    }
                }

                if (line.Trim().StartsWith("Operation"))
                {
                    m[mNum].Operation = line.Trim();
                }

                if (line.Trim().StartsWith("Test"))
                {
                    var spl = line.Split(' ');

                    m[mNum].Test = (ulong)int.Parse(spl[5]);
                }

                if (line.Trim().StartsWith("If true"))
                {
                    var spl = line.Split(' ');

                    m[mNum].TrueDest = int.Parse(spl[9]);
                }

                if (line.Trim().StartsWith("If false"))
                {
                    var spl = line.Split(' ');

                    m[mNum].FalseDest = int.Parse(spl[9]);
                }
            }

            return m;
        }
    }



    public class Monkey
    {
        public string Operation { get; set; }
        public List<ulong> Items { get; set; } = new();
        public ulong Test { get; set; }
        public int TrueDest { get; set; }
        public int FalseDest { get; set; }

        public ulong Inspect { get; set; } = 0;


        public void Execute(Dictionary<int, Monkey> ms, bool p1 = true)
        {
            var spl = Operation.Split(' ');

            long n = -1;
            if (int.TryParse(spl[5], out int x))
            {
                n = x;
            }

            var l = new List<ulong>();

            foreach (var item in Items)
            {
                ulong res = 0;
                if (spl[4] == "*")
                {
                    if (n == -1)
                    {
                        //res = item;
                        //if (item > 1000000)
                        //{
                        //    res = item;
                        //}
                        // else
                        // {
                        res = checked(item * item);
                        //  }
                    }
                    else
                    {


                        res = item * (ulong)n;// checked(item * (BigInteger)n);
                    }
                }
                else
                {
                    if (n == -1)
                    {
                        res = checked(item + item);
                    }
                    else
                    {
                        res = checked(item + (ulong)n);
                    }
                }
                if (p1)
                {
                    res /= 3;
                }
                else
                {
                    res = res % Day11.MagicNumber;
                }
                //if (res % Test == 0)
                //{
                //    res = Test;
                //}
                //else
                //{
                //    res = Test + res % Test;
                //}

                l.Add(res);

                //  Inspect++;
            }

            Inspect += (ulong)Items.Count;

            Items.Clear();
            foreach (var item in l)
            {
                Items.Add(item);
            }

            l.Clear();

            foreach (var item in Items)
            {
                if (item % Test == 0)
                {
                    ms[TrueDest].Items.Add(item);
                    //  Console.WriteLine($"going to: {TrueDest}");
                    l.Add(item);
                }
                else
                {
                    ms[FalseDest].Items.Add(item);
                    //    Console.WriteLine($"going to: {FalseDest}");
                    l.Add(item);
                }
            }

            foreach (var item in l)
            {
                Items.Remove(item);
            }
        }
    }
}
