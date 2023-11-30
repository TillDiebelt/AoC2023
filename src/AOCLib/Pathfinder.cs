using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOCLib
{
    public class Pathfinder<T>
    {
        public T[,] Map;
        public (int y, int x) Start = (0, 0);
        public (int y, int x) Goal = (0, 0);
        public Func<(int, int), (int, int), int> Distance = (a, b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
        public Func<T[,], (int, int), (int, int), int> StepCost = (x, a, b) => 1;
        public Func<T[,], (int, int), IEnumerable<(int, int)>> Neighbours;
        public Func<T, T, bool> Filter = (a, b) => true;

        public Pathfinder(T[,] map)
        {
            this.Map = map;
        }
        
        public List<(int, int)> FindPath((int, int) start, (int, int) end)
        {
            Start = start;
            Goal = end;
            (int x, int y) current = start;
            Dictionary<(int, int), (int, int)> walk = new Dictionary<(int, int), (int, int)>();
            Dictionary<(int, int), int> cost = new Dictionary<(int, int), int>();
            var queue = new PriorityQueue<((int, int), (int, int), int), int>();
            (int, int) back = current;
            queue.Enqueue((current, current, 0), 0);
            while (queue.Count > 0)
            {
                ((int, int) cur, (int, int) prev, int c) todo = queue.Dequeue();
                current = todo.cur;
                if (current == Goal)
                {
                    walk.Add(current, todo.prev);
                    back = current;
                    break;
                }
                if (cost.ContainsKey(current))
                {
                    if (cost[current] <= todo.c)
                        continue;
                    else
                    {
                        walk[(current)] = todo.prev;
                        cost[current] = todo.c;
                    }
                }
                else
                {
                    walk.Add(current, todo.prev);
                    cost.Add(current, todo.c);
                }
                IEnumerable<(int x, int y)> neighbours;
                if (Neighbours == null)
                    neighbours = Map.Neighbours(current.x, current.y);
                else
                    neighbours = Neighbours(Map, current);
                neighbours = neighbours.Where(x => Filter(Map[current.y, current.x], Map[x.y, x.x])).ToList();
                foreach (var neighbour in neighbours)
                {
                    if (cost.ContainsKey(neighbour))
                    {
                        if (cost[neighbour] <= todo.c + StepCost(Map, current, neighbour))
                            continue;
                    }
                    queue.Enqueue((neighbour, current, todo.c + StepCost(Map, current, neighbour)), todo.c + Distance(neighbour, Goal));
                }
            }
            if (current != Goal) return new List<(int, int)>();

            var path = new List<(int, int)>();
            while (current != Start)
            {
                path.Add(current);
                current = walk[current];
            }
            path.Reverse();
            return path;
        }
    }    
}
