using System.Collections;

namespace AOC_2022.Helpers
{
    public class Input : IEnumerable<char>
    {
        public string Value { get; set; } = "";

        public string[] Lines => Value.Split('\n');

        public int Length => Value.Length;

        public char[,]? ToGrid()
        {
            if (Lines.Select(l => l.Length).Distinct().Count() != 1)
            {
                return null;
            }

            char[,] grid = new char[Lines[0].Length, Lines.Length];

            for (int x = 0; x < grid.Width(); x++)
            {
                for (int y = 0; y < grid.Height(); y++)
                {
                    grid[x, y] = Lines[y][x];
                }
            }

            return grid;
        }

        public T[,] ToProcessedGrid<T>(Func<int, int, char, T> processFunc)
        {
            if (Lines.Select(l => l.Length).Distinct().Count() != 1)
            {
                throw new InvalidOperationException("all lines are not the same length.");
            }

            T[,] grid = new T[Lines[0].Length, Lines.Length];

            for (int x = 0; x < grid.Width(); x++)
            {
                for (int y = 0; y < grid.Height(); y++)
                {
                    grid[x, y] = processFunc.Invoke(x, y, Lines[y][x]);
                }
            }

            return grid;
        }

        public char this[int key] => Value[key];

        public string[] Split(char v) => Value.Split(v);

        internal string[] Split(string v) => Value.Split(v);

        public IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Value).GetEnumerator();
    }
}
