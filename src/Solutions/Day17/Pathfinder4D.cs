using Microsoft.Diagnostics.Runtime.DacInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Solutions.Day17
{
    public class Pathfinder4D
    {
        public int[,] Map;
        public (int y, int x, int dx, int dy) Start = (0, 0, 0, 0);
        public (int y, int x, int dx, int dy) Goal = (0, 0, 0, 0);
        public Func<(int, int, int, int), (int, int, int, int), int> Distance = (a, b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);

        public Pathfinder4D(int[,] map)
        {
            Map = map;
            Goal = (map.GetUpperBound(1), map.GetUpperBound(0), 0, 0);
        }

        public List<(int x, int y)> FindPath(int min, int max)
        {
            (int x, int y, int dx, int dy) current = Start;
            Dictionary<(int, int, int dx, int dy), (int, int, int dx, int dy)> walk = new Dictionary<(int, int, int dx, int dy), (int, int, int dx, int dy)>();
            Dictionary<(int, int, int dx, int dy), int> cost = new Dictionary<(int, int, int dx, int dy), int>();
            var queue = new PriorityQueue<((int, int, int dx, int dy), (int, int, int dx, int dy), int), int>();
            (int, int, int dx, int dy) back = current;
            queue.Enqueue((current, current, 0), 0);
            int step = 0;
            while (queue.Count > 0)
            {
                ((int, int, int dx, int dy) cur, (int, int, int dx, int dy) prev, int c) todo = queue.Dequeue();
                current = todo.cur;

                if (current.x == Goal.x && current.y == Goal.y && step > 0)
                {
                    walk.Add(current, todo.prev);
                    back = current;
                    break;
                }
                step++;
                if (cost.ContainsKey(current))
                {
                    if (cost[current] <= todo.c)
                        continue;
                    else
                    {
                        walk[current] = todo.prev;
                        cost[current] = todo.c;
                    }
                }
                else
                {
                    walk.Add(current, todo.prev);
                    cost.Add(current, todo.c);
                }
                List<(int x, int y, int dx, int dy)> neighbours = new List<(int x, int y, int dx, int dy)>();

                int MaxAllowed = max;
                if (current.x - 1 >= 0 && current.dx <= 0 && current.dx - 1 >= -MaxAllowed && (Math.Abs(current.dy) >= min || current.dy == 0))
                    neighbours.Add((current.x - 1, current.y, current.dx - 1, 0));
                if (current.x + 1 <= Map.GetUpperBound(1) && current.dx >= 0 && current.dx + 1 <= MaxAllowed && (Math.Abs(current.dy) >= min || current.dy == 0))
                    neighbours.Add((current.x + 1, current.y, current.dx + 1, 0));

                if (current.y - 1 >= 0 && current.dy <= 0 && current.dy - 1 >= -MaxAllowed && (Math.Abs(current.dx) >= min || current.dx == 0))
                    neighbours.Add((current.x, current.y - 1, 0, current.dy - 1));
                if (current.y + 1 <= Map.GetUpperBound(0) && current.dy >= 0 && current.dy + 1 <= MaxAllowed && (Math.Abs(current.dx) >= min || current.dx == 0))
                    neighbours.Add((current.x, current.y + 1, 0, current.dy + 1));

                foreach (var neighbour in neighbours)
                {
                    int Newcost = todo.c + Map[neighbour.y, neighbour.x];
                    queue.Enqueue((neighbour, current, Newcost), Newcost + (Distance(neighbour, Goal) - 1));
                }
            }

            var path = new List<(int, int)>();
            while (current != Start)
            {
                path.Add((current.x, current.y));
                current = walk[current];
            }
            path.Reverse();
            return path;
        }
    }
}
