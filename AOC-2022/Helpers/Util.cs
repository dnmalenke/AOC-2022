using System.Text;
using System.Text.RegularExpressions;

namespace AOC_2022.Helpers
{
    public static class Util
    {
        /// <summary>
        /// Turns grid of chars into string
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="reverse">false if higher y is down, true if higher y is up</param>
        /// <returns></returns>
        public static string StringifyGrid(char[,] grid, bool reverse = false)
        {
            StringBuilder sb = new();

            for (int y = reverse ? grid.Height() - 1 : 0; reverse ? y >= 0 : y < grid.Height(); y += (reverse ? -1 : 1))
            {
                for (int x = 0; x < grid.Width(); x++)
                {
                    sb.Append(grid[x, y]);
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }

        public static TResult? Case<TInput, TResult>(TInput input, params (TInput, TResult)[] cases) where TInput : IEquatable<TInput>
        {
            return cases.FirstOrDefault(c => input.Equals(c.Item1)).Item2 ?? default;
        }

        public static TResult Case<TInput, TResult>(TInput input, TResult defaultVal, params (TInput, TResult)[] cases) where TInput : IEquatable<TInput>
        {
            return cases.FirstOrDefault(c => input.Equals(c.Item1)).Item2 ?? defaultVal;
        }

        public static void Case(Func<string, bool> comparison, params (string, Action)[] cases)
        {
            cases.FirstOrDefault(c => comparison.Invoke(c.Item1)).Item2?.Invoke();
        }

        public static bool InBounds<T>(int x, int y, T[,] grid)
        {
            return x >= 0 && y >= 0 && x < grid.Width() && y < grid.Height();
        }
    }
}
