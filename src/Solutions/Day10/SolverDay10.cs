using System;
using System.IO;
using System.Linq;
using TillSharp.Math.Parser;
using TillSharp.Math.Functions;
using TillSharp.Math.Vectors;
using TillSharp.Math.Array;
using TillSharp.Math.ArrayExtender;
using TillSharp.Math;
using TillSharp.Extenders.Collections;
using TillSharp.Extenders.String;
using TillSharp.Extenders.Numerical;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Collections;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.Diagnostics.Runtime;
using BenchmarkDotNet.Toolchains.CoreRun;
using TillSharp.Base;
using System.Text.RegularExpressions;
using Microsoft.Diagnostics.Runtime.DacInterface;
using AOCLib;

namespace Solutions.Day10
{
    public class SolverDay10 : ISolver
    {
        //up = 1; right = 2; down = 3; //left = 4
        public const int UP = 1;
        public const int RIGHT = 2;
        public const int DOWN = 3;
        public const int LEFT = 4;
        
        public (int, int) VERT = (UP, DOWN);
        public (int, int) HOR = (RIGHT, LEFT);
        public (int, int) UR = (UP, RIGHT);
        public (int, int) UL = (UP, LEFT);
        public (int, int) DL = (DOWN, LEFT);
        public (int, int) DR = (DOWN, RIGHT);
        public (int, int) START = (0,0);
        public (int, int) NONE = (-1,-1);


        public long SolvePart1(string[] lines)
        {
            //Parse
            // 0,3 -> 1,4
            // 2,1 -> 4,3
            //var rocklines = lines.Select(x => x.Split("->").ToList().Select(y => (Int32.Parse(y.Trim().Split(',')[0]), Int32.Parse(y.Trim().Split(',')[1]))).ToList());

            //1,3,5,1,2,455,6
            //var longs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

            //1
            //3
            //var sum = lines.Select(y => Convert.ToInt64(y)).Map(x => x).Reduce((x, y) => x + y);


            long result = 0;
            List<long> longs = new();
            Dictionary<string, long> dic = new();
            (int,int)[,] pipes = new (int, int)[lines.Length, lines[0].Length];
            (int, int) start = (0,0);
            
            for(int i = 0; i < lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    (int, int) p = (i, j);
                    switch (lines[i][j])
                    {
                        case '.':
                            pipes[p.Item1, p.Item2] = NONE;
                            break;
                        case '-':
                            pipes[p.Item1, p.Item2] = HOR;
                            break;
                        case '|':
                            pipes[p.Item1, p.Item2] = VERT;
                            break;
                        case 'L':
                            pipes[p.Item1, p.Item2] = UR;
                            break;
                        case 'J':
                            pipes[p.Item1, p.Item2] = UL;
                            break;
                        case '7':
                            pipes[p.Item1, p.Item2] = DL;
                            break;
                        case 'F':
                            pipes[p.Item1, p.Item2] = DR;
                            break;
                        case 'S':
                            pipes[p.Item1, p.Item2] = START;
                            start = (j,i);
                            break;
                    }
                }
            }

            //start = (110, 107);
            //pipes[110, 107] = VERT;
            Func<(int,int)[,], (int, int), IEnumerable<(int, int)>> Neighbours =
            (map, current) => 
            {
                (int, int) currentP = map[current.Item1, current.Item2];
                int x = current.Item1;
                int y = current.Item2;
                List<(int x, int y)> neighbours = new List<(int x, int y)>();
                List<(int x, int y)> result = new List<(int x, int y)>();
                if (x - 1 >= 0)
                {
                    if(IsValidNeighbor(map, current, (x-1,y)))
                        neighbours.Add((x - 1, y));
                }
                if (x + 1 <= map.GetUpperBound(1))
                {
                    if (IsValidNeighbor(map, current, (x + 1, y)))
                        neighbours.Add((x + 1, y));
                }
                if (y - 1 >= 0)
                    if (IsValidNeighbor(map, current, (x, y -1))) 
                        neighbours.Add((x, y - 1));
                if (y + 1 <= map.GetUpperBound(0))
                    if (IsValidNeighbor(map, current, (x, y + 1))) 
                        neighbours.Add((x, y + 1));
                
                foreach(var n in neighbours)
                {
                    if (map[n.x, n.y] != NONE) result.Add(n);
                }
                return result;
            };            
            
            see.Add(start);
            result = WalkStep(pipes, start, 0, start, start);

            return (result + 1) / 2; // (seen.Max(x => x.Item2)+6)/2; //6813,6814,6815
        }

        public static int foo = 0;
        public static List<(int, int)> see = new List<(int, int)>();
        
        private long WalkStep((int,int)[,] m, (int, int) cur, int i)
        {
            long res = 0;
            foreach (var n in Neighbours(m, cur, (-1,-1)))
            {
                if (!see.Contains(n))
                {
                    foo++;
                    see.Add(n);
                    res = Math.Max(res, WalkStep(m, n, i + 1));
                }
            }

            return res + 1;
        }

        private long WalkStep((int, int)[,] m, (int, int) cur, int i, (int, int) prev, (int, int) start)
        {
            long res = 0;
            var neigh = Neighbours(m, cur, prev);
            if (neigh.Count() == 0) res = i;
            foreach (var n in neigh)
            {
                if (n != prev)
                {
                    if (n == start)
                    {
                        return i + 1;
                    }
                    res = Math.Max(res, WalkStep(m, n, i + 1, cur, start));
                }
            }

            return res;
        }
        
        /*
        private long WalkStep((int, int)[,] m, (int, int) cur, int i, (int, int) prev, (int, int) start)
        {
            long res = 0;
            var neigh = Neighbours(m, cur, (-1, -1));
            if (neigh.Count() == 0) res = i;
            foreach (var n in neigh)
            {
                if (n != prev)
                {
                    if (n == start)
                    {
                        return i + 1;
                    }
                    res = Math.Max(res, WalkStep(m, n, i + 1, cur, start));
                }
            }

            return res;
        }*/

        private IEnumerable<(int, int)> Neighbours((int, int)[,] map, (int,int) current, (int, int) prev)
        {
            int x = current.Item1;
            int y = current.Item2;
            if (x - 1 >= 0 && x-1 != prev.Item1)
            {
                if (IsValidNeighbor(map, current, (x - 1, y)))
                    yield return (x - 1, y);
            }
            if (x + 1 <= map.GetUpperBound(1) && x + 1 != prev.Item1)
            {
                if (IsValidNeighbor(map, current, (x + 1, y)))
                    yield return (x + 1, y);
            }
            if (y - 1 >= 0 && y - 1 != prev.Item2)
                if (IsValidNeighbor(map, current, (x, y - 1)))
                    yield return (x, y - 1);
            if (y + 1 <= map.GetUpperBound(0) && y + 1 != prev.Item2)
                if (IsValidNeighbor(map, current, (x, y + 1)))
                    yield return (x, y + 1);
        }

        private bool IsValidNeighbor((int, int)[,] map, (int, int) current, (int, int) neighbor)
        {
            (int, int) currentP = map[current.Item2, current.Item1];
            int x = current.Item1;
            int y = current.Item2;
            (int, int) neighborP = map[neighbor.Item2, neighbor.Item1];
            int nx = neighbor.Item1;
            int ny = neighbor.Item2;
            if (neighborP == NONE) return false;
            if (currentP == START) return true;
            if (neighborP == START) return true;
            if (nx == x - 1) //left
            {
                if ((currentP.Item1 == LEFT || currentP.Item2 == LEFT) && (neighborP.Item1 == RIGHT || neighborP.Item2 == RIGHT))
                    return true;
            }
            if (nx == x + 1) //right
            {
                if ((currentP.Item1 == RIGHT || currentP.Item2 == RIGHT) && (neighborP.Item1 == LEFT || neighborP.Item2 == LEFT))
                    return true;
            }
            if (ny == y - 1) //up
            {
                if ((currentP.Item1 == UP || currentP.Item2 == UP) && (neighborP.Item1 == DOWN || neighborP.Item2 == DOWN))
                    return true;
            }
            if (ny == y + 1) //down
            {
                if ((currentP.Item1 == DOWN || currentP.Item2 == 3) && (neighborP.Item1 == UP || neighborP.Item2 == UP))
                    return true;
            }
            return false;
        }

        public long SolvePart2(string[] lines)
        {
            //Parse
            // 0,3 -> 1,4
            // 2,1 -> 4,3
            //var rocklines = lines.Select(x => x.Split("->").ToList().Select(y => (Int32.Parse(y.Trim().Split(',')[0]), Int32.Parse(y.Trim().Split(',')[1]))).ToList());

            //1,3,5,1,2,455,6
            //var longs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

            //1
            //3
            //var sum = lines.Select(y => Convert.ToInt64(y)).Map(x => x).Reduce((x, y) => x + y);


            //Solve
            long result = Utils.GaussSum(10, 5);
            return result;
        }
    }
}
