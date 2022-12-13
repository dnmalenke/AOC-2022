using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day13)}")]
    public class Day13 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";
            int sum = 0;

            var spl = _input.Split('\n');

            string prev = "";
            List<PacketPair> packets = new();
            List<Packet> ps = new();

            foreach (var line in spl)
            {
                if (line != "")
                {
                    ps.Add(new Packet(line));
                }

                if (prev == "")
                {
                    prev = line;
                }
                else
                {
                    packets.Add(new(prev, line));
                    prev = "";
                }
            }

            for (int i = 0; i < packets.Count; i++)
            {
                bool? res = packets[i].Compare();

                if (res == null)
                {
                    _result += $"\n {i + 1}: null";
                }
                else
                {
                    // Console.WriteLine($"{i + 1} returned {((bool)res ? "true" : "false")}");
                    _result += $"\n {i + 1}:  {((bool)res ? "true" : "false")}";

                    if ((bool)res)
                    {
                        sum += 1 + i;
                    }
                }
            }

            _result += $"\npart 1: {sum}\n\n\n";
            ps.Sort();
            foreach (var item in ps)
            {
                _result += $"\n{item.P}";
            }

            int p1i = ps.IndexOf(ps.FirstOrDefault(p => p.P == "[[2]]") ?? new("")) + 1;
            int p2i = ps.IndexOf(ps.FirstOrDefault(p => p.P == "[[6]]") ?? new("")) + 1;

            _result += $"\npart 2: {p1i * p2i}";
        }
    }

    public class Packet : IComparable<Packet>
    {
        public string P { get; set; }
        public Packet(string p)
        {
            P = p;
        }

        public int CompareTo(Packet? other)
        {
            if (other == null)
            {
                return 0;
            }

            bool? res = new PacketPair(P, other.P).Compare();

            if (res == null)
            {
                Console.WriteLine($"Bad: {P}, {other.P}");
                return 0;
            }

            //if (P == "[[[]]]" || other.P == "[[[]]]")
            //    Console.WriteLine($"{P} vs {other.P} returned {((bool)res ? "true" : "false")}");

            if ((bool)res)
            {
                return -1;
            }

            return 1;
        }
    }

    public class PacketPair
    {
        public string P1 { get; set; }
        public string P2 { get; set; }

        public PacketPair(string p1, string p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public bool? Compare()
        {
            return CompareList(P1, P2);
        }

        public static List<string> SplitPacket(string l)
        {
            List<string> lSpl = new();

            string temp = "";
            int end = -1;
            for (int i = 1; i < l.Length; i++)
            {
                if (end != -1)
                {
                    if (i == end)
                    {
                        lSpl.Add(temp);
                        temp = "";
                        end = -1;
                        continue;
                    }
                }
                else
                {
                    if (l[i] == '[')
                    {
                        end = IndexOfEnd(l[i..]) + 1 + i;
                    }

                    if (l[i] == ',' || l[i] == ']')
                    {
                        lSpl.Add($"[{temp}]");
                        temp = "";
                        continue;
                    }
                }

                temp += l[i];
            }

            return lSpl;
        }
        public bool? CompareList(string l, string r)
        {
            Console.WriteLine($"Compare {l} vs {r}");
            bool? res = null;

            var lSpl = SplitPacket(l);
            var rSpl = SplitPacket(r);


            if (lSpl.Count == 1 && rSpl.Count == 1)
            {
                if (r != rSpl[0] && l == lSpl[0] && l.Length == 2)
                {
                    return true;
                }

                if (l != lSpl[0] && r == rSpl[0] && r.Length == 2)
                {
                    return false;
                }

                var lSpl2 = SplitPacket(lSpl[0]);
                var rSpl2 = SplitPacket(rSpl[0]);

                if (lSpl2.Count == 1 && rSpl2.Count == 1 && rSpl2[0] == rSpl[0] && lSpl2[0] == lSpl[0])
                {
                    if (rSpl[0][1..^1].Length == 0 && lSpl[0][1..^1].Length == 0)
                    {
                        return null;
                    }

                    if (rSpl[0][1..^1].Length == 0)
                    {
                        return false;
                    }

                    if (lSpl[0][1..^1].Length == 0)
                    {
                        return true;
                    }

                    int rn = int.Parse(rSpl[0][1..^1]);
                    int ln = int.Parse(lSpl[0][1..^1]);

                    if (rn == ln)
                    {
                        return null;
                    }

                    if (ln < rn)
                    {
                        return true;
                    }

                    if (ln > rn)
                    {
                        return false;
                    }
                }
                else
                {
                    lSpl = lSpl2;
                    rSpl = rSpl2;
                }
            }

            int i;
            for (i = 0; i < lSpl.Count; i++)
            {
                if (i >= rSpl.Count)
                {
                    // r ran out
                    return false;
                }

                res = CompareList(lSpl[i], rSpl[i]);

                if (res != null)
                {
                    return res;
                }
            }

            if (i < rSpl.Count)
            {
                return true;
            }

            return res;
        }

        public static int IndexOfEnd(string l)
        {
            int level = 0;
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] == '[')
                {
                    level++;
                }

                if (l[i] == ']')
                {
                    level--;

                    if (level == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
