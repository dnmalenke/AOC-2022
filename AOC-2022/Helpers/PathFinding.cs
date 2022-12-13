using AOC_2022.Pages;
using System.Runtime.ConstrainedExecution;

namespace AOC_2022.Helpers
{
    public class PathFinding
    {
        public interface IPathable<T>
        {
            T? Parent { get; set; }
        }

        public enum Method
        {
            BreadthFirst,
            AStar
        }

        /// <summary>
        /// Returns a list of T starting at start containing the steps to goal or an empty list if a path cannot be found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        /// <param name="neighborsFunc">Function that returns neighbors for a given T</param>
        /// <returns></returns>
        public static List<T> BreadthFirstSearch<T>(T start, T goal, Func<T, List<T>> neighborsFunc) where T : IEquatable<T>, IPathable<T>
        {
            Queue<T> pQ = new();
            pQ.Enqueue(start);

            HashSet<T> visited = new();

            T? item = default;
            while (pQ.Count > 0)
            {
                item = pQ.Dequeue();

                if (item.Equals(goal))
                {
                    break;
                }

                foreach (var dest in neighborsFunc(item))
                {
                    if (visited.Contains(dest))
                    {
                        continue;
                    }

                    visited.Add(dest);
                    dest.Parent = item;
                    pQ.Enqueue(dest);
                }
            }

            if (!goal.Equals(item))
            {
                return new();
            }

            start.Parent = default;
            var cur = item;

            List<T> result = new();
            while (cur != null)
            {
                result.Insert(0, cur);
                cur = cur.Parent;
            }

            return result;
        }

        public static List<T> AStarSearch<T>(T start, T goal, Func<T, List<T>> neighborsFunc, Func<T, T, int> distanceFunc, Func<T, T, int> costFunc) where T : IEquatable<T>, IPathable<T>
        {
            PriorityQueue<T, int> pQ = new(); // list of open nodes
            Dictionary<T, int> g = new();
            Dictionary<T, int> f = new();
            pQ.Enqueue(start, 0); // add starting node
            HashSet<T> closed = new();
            bool done = false;


            while (pQ.Count > 0 && !done)
            {
                var q = pQ.Dequeue();

                closed.Add(q);

                if (goal.Equals(q))
                {
                    done = true;
                    break;
                }

                var d = neighborsFunc(q);

                foreach (var item in d)
                {
                    T successor = item;

                    var tCost = g[q] + costFunc.Invoke(q, successor);

                    if (pQ.UnorderedItems.Any(n => n.Element.Equals(successor)))
                    {
                        if (g[successor] <= tCost)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        var existingNode = closed.FirstOrDefault(n => n.Equals(successor));
                        if (existingNode != null)
                        {
                            if (g[existingNode] <= tCost)
                            {
                                continue;
                            }
                            else
                            {
                                closed.Remove(existingNode);
                                pQ.Enqueue(existingNode, f[existingNode]);
                            }
                        }
                        else
                        {
                            f[successor] = tCost + distanceFunc.Invoke(successor, goal);
                            pQ.Enqueue(successor, f[successor]);
                        }
                    }

                    successor.Parent = q;
                }
            }

            var cur = closed.FirstOrDefault(n => n.Equals(goal));

            if (cur == null)
            {
                return new();
            }

            List<T> result = new();
            while (cur != null)
            {
                result.Insert(0, cur);
                cur = cur.Parent;
            }

            return result;
        }
    }
}
