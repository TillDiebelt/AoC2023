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

namespace Solutions.Day16
{
    public class SolverDay16 : ISolver
    {

        private const int UP = 1;
        private const int DOWN = 2;
        private const int LEFT = 3;
        private const int RIGHT = 4;

        const int VERT = 1;
        const int HOR = 2;
        const int LD = 3;
        const int UL = 4;

        public long SolvePart1(string[] lines, string text)
        {
            List<long> longs = new();
            int[,] map = new int[lines.Length, lines[0].Length];


            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch(lines[i][j])
                    {
                        case '\\':
                            map[i,j] = LD;
                            break;
                        case '/':
                            map[i,j] = UL;
                            break;
                        case '-':
                            map[i,j] = HOR;
                            break;
                        case '|':
                            map[i,j] = VERT;
                            break;
                    }
                }
            }

            List<((int x, int y), int dir)> beams = new();
            beams.Add(((0, 0), RIGHT));

            var seen = search(beams, map);
            
            //Solve
            long result = seen.Select(x => x.Item1).ToHashSet().Count();
            return result;
        }

        private List<((int,int),int)> search(List<((int, int), int)> start, int[,] map)
        {
            List<((int x, int y), int dir)> todo = new();
            List<((int, int), int)> seen = new();
            todo.Add(start.First());
            while (todo.Count > 0)
            {
                List<((int, int), int)> newTodo = new List<((int, int), int)>();
                foreach (var t in todo)
                {
                    if (t.Item1.x < 0 || t.Item1.y < 0 || t.Item1.x > map.GetUpperBound(1) || t.Item1.y > map.GetUpperBound(0))
                    {
                        continue;
                    }
                    var pos = t.Item1;
                    if (!seen.Contains((t.Item1, t.Item2)))
                    {
                        switch(map[t.Item1.y,t.Item1.x])
                        {
                            case HOR:
                                if (t.dir == LEFT || t.dir == RIGHT)
                                {
                                    newTodo.Add((Next(pos, t.dir), t.Item2));
                                }
                                else
                                {
                                    newTodo.Add(((pos.x - 1, pos.y), LEFT));
                                    newTodo.Add(((pos.x + 1, pos.y), RIGHT));
                                }
                                break;
                            case VERT:
                                if (t.dir == UP || t.dir == DOWN)
                                {
                                    newTodo.Add((Next(pos,t.dir), t.Item2));
                                }
                                else
                                {
                                    newTodo.Add(((pos.x, pos.y - 1), UP));
                                    newTodo.Add(((pos.x, pos.y + 1), DOWN));
                                }
                                break;
                            case UL: // /
                                if (t.dir == UP)
                                {
                                    newTodo.Add(((pos.x + 1, pos.y), RIGHT));
                                }
                                else if (t.dir == DOWN)
                                {
                                    newTodo.Add(((pos.x - 1, pos.y), LEFT));
                                }
                                else if (t.dir == LEFT)
                                {
                                    newTodo.Add(((pos.x, pos.y + 1), DOWN));
                                }
                                else if (t.dir == RIGHT)
                                {
                                    newTodo.Add(((pos.x, pos.y - 1), UP));
                                }
                                break;
                            case LD: // \
                                if (t.dir == UP)
                                {
                                    newTodo.Add(((pos.x - 1, pos.y), LEFT));
                                }
                                else if (t.dir == DOWN)
                                {
                                    newTodo.Add(((pos.x + 1, pos.y), RIGHT));
                                }
                                else if (t.dir == LEFT)
                                {
                                    newTodo.Add(((pos.x, pos.y - 1), UP));
                                }
                                else if (t.dir == RIGHT)
                                {
                                    newTodo.Add(((pos.x, pos.y + 1), DOWN));
                                }
                                break;
                            default:
                                newTodo.Add((Next(t.Item1, t.Item2), t.Item2));
                                break;
                        }
                    }
                    seen.Add((pos, t.Item2));
                }
                todo = newTodo;
            }
            return seen;
        }

        private (int x, int y) Next((int x, int y) cur, int move)
        {
            switch(move)
            {
                case UP: return (cur.x, cur.y - 1);
                case DOWN: return (cur.x, cur.y + 1);
                case LEFT: return (cur.x - 1, cur.y);
                case RIGHT: return (cur.x + 1, cur.y);
            }
            return cur;
        }

        public long SolvePart2(string[] lines, string text)
        {
            List<long> longs = new();
            int[,] map = new int[lines.Length, lines[0].Length];


            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch (lines[i][j])
                    {
                        case '\\':
                            map[i, j] = LD;
                            break;
                        case '/':
                            map[i, j] = UL;
                            break;
                        case '-':
                            map[i, j] = HOR;
                            break;
                        case '|':
                            map[i, j] = VERT;
                            break;
                    }
                }
            }
            long max = 0;

            for (int i = 0; i <= map.GetUpperBound(0); i++)
            {
                List<((int x, int y), int dir)> beams1 = new();
                List<((int x, int y), int dir)> beams2 = new();
                beams1.Add(((0, i), RIGHT));
                beams2.Add(((map.GetUpperBound(1), i), LEFT));

                var seen1 = search(beams1, map);
                var seen2 = search(beams2, map);
                //dfs would be MUCH faster if dynamic and all paths cached
                max = Math.Max(max, seen1.Select(x => x.Item1).ToHashSet().Count());
                max = Math.Max(max, seen2.Select(x => x.Item1).ToHashSet().Count());
            }
            for (int i = 0; i <= map.GetUpperBound(1); i++)
            {
                List<((int x, int y), int dir)> beams1 = new();
                List<((int x, int y), int dir)> beams2 = new();
                beams1.Add(((i, 0), DOWN));
                beams2.Add(((i, map.GetUpperBound(0)), UP));

                var seen1 = search(beams1, map);
                var seen2 = search(beams2, map);
                max = Math.Max(max, seen1.Select(x => x.Item1).ToHashSet().Count());
                max = Math.Max(max, seen2.Select(x => x.Item1).ToHashSet().Count());
            }

            long result = max;
            return result;
        }
    }
}
