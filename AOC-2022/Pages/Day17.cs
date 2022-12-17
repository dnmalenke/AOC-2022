using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day17)}")]
    public class Day17 : DayTemplate
    {

        private static List<byte> _rows = new();

        protected override void Run()
        {
            _result = "";


            long p1 = Solve(2022);

            _result += $"\nPart 1: {p1}";

            long p2 = Solve(1000000000000);

            _result += $"\nPart 2: {p2}";
        }

        public long Solve(long max)
        {

            long rC = 0;
            int rI = 0;
            int iPos = 0;
            int x = 2;

            _rows.Clear();
            _rows.Add(0b11111110);

            List<Rock> rocks = new()
            {
                new("####"),
                new(".#.\n###\n.#."),
                new("..#\n..#\n###"),
                new("#\n#\n#\n#"),
                new("##\n##"),
            };

            long last2 = -1;
            long lastFound = -1;
            int patternH = 0;
            int startH = 0, curH = 0;
            long highest = 0;
            
            while (rC < max)
            {
                var r = rocks[rI];
                rI = (rI + 1) % rocks.Count;

                var y = Highest() + 3;

                for (int i = _rows.Count; i <= y + 3; i++)
                {
                    _rows.Add(0);
                }

                if (rI == iPos)
                {
                    Console.WriteLine($"\nROCK: {rC} {lastFound} {last2} {patternH}");
                    if (lastFound == -1)
                    {
                        lastFound = rC;
                        startH = Highest();
                    }
                    else if (last2 == -1)
                    {
                        last2 = rC - lastFound;
                        curH = Highest();
                        patternH = curH - startH;


                        var left = max - rC;

                        var rem = left % last2;
                        highest = curH + patternH * (left / last2);
                        rC = (max - rem);
                    }
                }

                while (true)
                {
                    var c = _input[iPos];
                    iPos = (iPos + 1) % _input.Length;

                    if (c == '>')
                    {
                        if (x + r.Width < 7)
                        {
                            if (r.MoveRight(x, y))
                                x++;
                        }
                    }
                    else
                    {
                        if (x > 0)
                        {
                            if (r.MoveLeft(x, y))
                                x--;
                        }
                    }

                    if (r.Check(x, y - 1))
                    {
                        r.Land(x, y);

                        break;
                    }

                    y--;
                }

                rC++;
                x = 2;
            }

            highest += Highest() - curH;

            return highest - 1;
        }

        public static int Highest()
        {
            int i;
            for (i = _rows.Count - 1; i >= 0; i--)
            {
                if (_rows[i] > 0)
                {
                    break;
                }
            }
            return i + 1;
        }

        public class Rock
        {
            public string Def { get; set; }

            public int Width { get; init; }

            public bool Check(int x, int y)
            {
                switch (Def)
                {
                    case "####":
                        return (_rows[y] & (byte)(0b11110000 >> x)) != 0;
                    case ".#.\n###\n.#.":
                        return (_rows[y] & (byte)(0b01000000 >> x)) != 0 || (_rows[y + 1] & (byte)(0b11100000 >> x)) != 0;
                    case "..#\n..#\n###":
                        return (_rows[y] & (byte)(0b11100000 >> x)) != 0;
                    case "#\n#\n#\n#":
                        return (_rows[y] & (byte)(0b10000000 >> x)) != 0;
                    case "##\n##":
                        return (_rows[y] & (byte)(0b11000000 >> x)) != 0;
                    default:
                        break;
                }

                return false;
            }

            public bool MoveRight(int x, int y)
            {
                switch (Def)
                {
                    case "####":
                        return (_rows[y] & (byte)(0b00001000 >> x)) == 0;
                    case ".#.\n###\n.#.":
                        return (_rows[y] & (byte)(0b00100000 >> x)) == 0 && (_rows[y + 2] & (byte)(0b00100000 >> x)) == 0 && (_rows[y + 1] & (byte)(0b00010000 >> x)) == 0;
                    case "..#\n..#\n###":
                        return (_rows[y] & (byte)(0b00010000 >> x)) == 0 && (_rows[y + 1] & (byte)(0b00010000 >> x)) == 0 && (_rows[y + 2] & (byte)(0b00010000 >> x)) == 0;
                    case "#\n#\n#\n#":
                        return (_rows[y] & (byte)(0b01000000 >> x)) == 0 && (_rows[y + 1] & (byte)(0b01000000 >> x)) == 0 && (_rows[y + 2] & (byte)(0b01000000 >> x)) == 0 && (_rows[y + 3] & (byte)(0b01000000 >> x)) == 0;
                    case "##\n##":
                        return (_rows[y] & (byte)(0b00100000 >> x)) == 0 && (_rows[y + 1] & (byte)(0b00100000 >> x)) == 0;
                    default:
                        break;
                }

                return true;
            }

            public bool MoveLeft(int x, int y)
            {
                switch (Def)
                {
                    case "####":
                        return (_rows[y] & (byte)(0b10000000 >> (x - 1))) == 0;
                    case ".#.\n###\n.#.":
                        return (_rows[y] & (byte)(0b01000000 >> (x - 1))) == 0 && (_rows[y + 2] & (byte)(0b01000000 >> (x - 1))) == 0 && (_rows[y + 1] & (byte)(0b10000000 >> (x - 1))) == 0;
                    case "..#\n..#\n###":
                        return (_rows[y] & (byte)(0b10000000 >> (x - 1))) == 0 && (_rows[y + 1] & (byte)(0b00100000 >> (x - 1))) == 0 && (_rows[y + 2] & (byte)(0b00100000 >> (x - 1))) == 0;
                    case "#\n#\n#\n#":
                        return (_rows[y] & (byte)(0b10000000 >> (x - 1))) == 0 && (_rows[y + 1] & (byte)(0b10000000 >> (x - 1))) == 0 && (_rows[y + 2] & (byte)(0b10000000 >> (x - 1))) == 0 && (_rows[y + 3] & (byte)(0b10000000 >> (x - 1))) == 0;
                    case "##\n##":
                        return (_rows[y] & (byte)(0b10000000 >> (x - 1))) == 0 && (_rows[y + 1] & (byte)(0b10000000 >> (x - 1))) == 0;
                    default:
                        break;
                }

                return true;
            }

            public void Land(int x, int y)
            {
                switch (Def)
                {
                    case "####":
                        ErrOr(y, (byte)(0b11110000 >> x));
                        break;
                    case ".#.\n###\n.#.":
                        ErrOr(y, (byte)(0b01000000 >> x));
                        ErrOr(y + 1, (byte)(0b11100000 >> x));
                        ErrOr(y + 2, (byte)(0b01000000 >> x));
                        break;
                    case "..#\n..#\n###":

                        ErrOr(y, (byte)(0b11100000 >> x));
                        ErrOr(y + 1, (byte)(0b00100000 >> x));
                        ErrOr(y + 2, (byte)(0b00100000 >> x));
                        break;
                    case "#\n#\n#\n#":

                        ErrOr(y, (byte)(0b10000000 >> x));
                        ErrOr(y + 1, (byte)(0b10000000 >> x));
                        ErrOr(y + 2, (byte)(0b10000000 >> x));
                        ErrOr(y + 3, (byte)(0b10000000 >> x));
                        break;
                    case "##\n##":
                        ErrOr(y, (byte)(0b11000000 >> x));
                        ErrOr(y + 1, (byte)(0b11000000 >> x));
                        break;
                    default:
                        break;
                }

            }

            public void ErrOr(int y, byte val)
            {
                if ((_rows[y] & val) != 0)
                {
                    Console.WriteLine("ERROR");
                }

                _rows[y] |= val;
            }

            public Rock(string def)
            {
                Def = def;


                switch (Def)
                {
                    case "####":
                        Width = 4;
                        break;
                    case ".#.\n###\n.#.":
                        Width = 3;
                        break;
                    case "..#\n..#\n###":
                        Width = 3;
                        break;
                    case "#\n#\n#\n#":
                        Width = 1;
                        break;
                    case "##\n##":
                        Width = 2;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
