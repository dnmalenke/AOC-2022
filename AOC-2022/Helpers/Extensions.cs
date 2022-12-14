using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AOC_2022.Helpers
{
    public static partial class Extensions
    {
        [GeneratedRegex("-?\\d+")]
        private static partial Regex NumRegex();

        public static string GetNumber(this string input) => NumRegex().Match(input).Value;

        public static int ParseNumber(this string input) => int.Parse(input.GetNumber());
        public static bool TryParseNumber(this string input, out int result) => int.TryParse(input.GetNumber(), out result);



#pragma warning disable CS8603 // Possible null reference return.
        public static TResult MinVal<T, TResult>(this IEnumerable<T> values, Func<T, TResult> selector) => values.Select(selector).Min();
        public static TResult MaxVal<T, TResult>(this IEnumerable<T> values, Func<T, TResult> selector) => values.Select(selector).Max();
#pragma warning restore CS8603 // Possible null reference return.

        public static int Width<T>(this T[,] arr) => arr.GetUpperBound(0) + 1;
        public static int Height<T>(this T[,] arr) => arr.GetUpperBound(1) + 1;

        public static void Fill<T>(this T[,] arr, T value)
        {
            for (int x = 0; x < arr.Width(); x++)
            {
                for (int y = 0; y < arr.Height(); y++)
                {
                    arr[x, y] = value;
                }
            }
        }

        #region GridEnumerators
        public static IEnumerator<ValueTuple<int, int, object>> AllEnumerator(this Array grid)
        {
            if (grid.Rank != 2)
            {
                throw new ArgumentException("Only works on 2d arrays");
            }

            int x = 0;
            int y = 0;

            foreach (object item in grid)
            {
                yield return ValueTuple.Create(x, y, item);

                y++;

                if (y == grid.GetUpperBound(1) + 1)
                {
                    x++;
                    y = 0;
                }
            }
        }

        public static IEnumerator GetEnumerator(this IEnumerator<(int, int, object)> e) => e;

        public static void Deconstruct(this object tuple, out int x, out int y, out object value)
        {
            ValueTuple<int, int, object> t = (ValueTuple<int, int, object>)tuple;
            x = t.Item1;
            y = t.Item2;
            value = t.Item3;
        }
        #endregion
    }
}
