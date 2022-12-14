using System.Text;

namespace AOC_2022.Helpers
{
    public record class Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Make a point from comma separted string X,Y. Gets rid of garbage around string
        /// </summary>
        /// <param name="csv"></param>
        public Point(string csv)
        {
            var spl = csv.Split(',');
            X = spl[0].ParseNumber();
            Y = spl[1].ParseNumber();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static int Distance(Point s, Point d)
        {
            return (int)Math.Round(Math.Sqrt(Math.Pow(s.X - d.X, 2) + Math.Pow(s.Y - d.Y, 2)));
        }

        /// <summary>
        /// turns list of points into visualized string
        /// </summary>
        /// <param name="points"></param>
        /// <param name="charFunc"></param>
        /// <param name="emptyChar"></param>
        /// <param name="reverse">false if higher y is down, true if higher y is up</param>
        /// <returns></returns>
        public static string StringifyList(List<Point> points, Func<Point, char> charFunc, char emptyChar = '.', bool reverse = false)
        {
            int offX = -points.MinVal(p => p.X);
            int offY = -points.MinVal(p => p.Y);

            char[,] output = new char[points.MaxVal(p => p.X) + offX + 1, points.MaxVal(p => p.Y) + offY + 1];

            output.Fill(emptyChar);

            foreach (var p in points)
            {
                output[p.X + offX, p.Y + offY] = charFunc.Invoke(p);
            }

            return Util.StringifyGrid(output, reverse);
        }
    }
}
