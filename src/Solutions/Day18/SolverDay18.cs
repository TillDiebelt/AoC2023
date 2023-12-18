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

namespace Solutions.Day18
{
    public class SolverDay18 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            List<(string, int, string)> items = new();
            
            for (int i = 0; i < lines.Length; i++)
            {
                var splits = lines[i].Split(" ");
                items.Add((splits[0], Int32.Parse(splits[1]), splits[2]));
            }

            List<(int, int)> path = new List<(int, int)>();
            (int x, int y) current = (0,0);
            path.Add(current);
            foreach(var item in items)
            {
                switch(item.Item1)
                {
                    case "R":
                        current.x += item.Item2;
                        break;
                    case "U":
                        current.y -= item.Item2;
                        break;
                    case "D":
                        current.y += item.Item2;
                        break;
                    case "L":
                        current.x -= item.Item2;
                        break;
                }
                path.Add(current);
            }


            int[,] map = new int[path.Select(x => x.Item2).Max()+ Math.Abs(path.Select(x => x.Item2).Min())+2, path.Select(x => x.Item1).Max() + Math.Abs(path.Select(x => x.Item1).Min())+2];

            current = (Math.Abs(path.Select(x => x.Item1).Min()), Math.Abs(path.Select(x => x.Item2).Min()));
            foreach(var item in items)
            {
                switch (item.Item1)
                {
                    case "R":
                        for(int x = 0; x <= item.Item2; x++)
                        {
                            map[current.y, current.x + x] = 1;
                        }
                        current.x += item.Item2;
                        break;
                    case "U":
                        for (int y = item.Item2; y >= 0; y--)
                        {
                            map[current.y - y, current.x] = 1;
                        }
                        current.y -= item.Item2;
                        break;
                    case "D":
                        for (int y = 0; y <= item.Item2; y++)
                        {
                            map[current.y + y, current.x] = 1;
                        }
                        current.y += item.Item2;
                        break;
                    case "L":
                        for (int x = item.Item2; x >= 0; x--)
                        {
                            map[current.y, current.x - x] = 1;
                        }
                        current.x -= item.Item2;
                        break;
                }
            }

            map = FloodFill(map, (Math.Abs(path.Select(x => x.Item1).Min())+1, Math.Abs(path.Select(x => x.Item2).Min())+1), 0, 1);

            long result = 0;
            for (int y = 0; y <= map.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= map.GetUpperBound(1); x++)
                {
                    result += map[y,x];
                }
            }

            //Solve
            return result;
        }


        private int[,] FloodFill(int[,] map, (int, int) start, int col, int paint)
        {
            Stack<(int, int)> pixels = new Stack<(int, int)>();
            pixels.Push(start);
            int[,] result = map;

            while (pixels.Count > 0)
            {
                (int X, int Y) a = pixels.Pop();
                if (a.X <= map.GetUpperBound(1) && a.X >= 0 &&
                        a.Y <= map.GetUpperBound(0) && a.Y >= 0)
                {

                    if (result[a.Y, a.X] == col)
                    {
                        result[a.Y, a.X] = paint;
                        pixels.Push((a.X - 1, a.Y));
                        pixels.Push((a.X + 1, a.Y));
                        pixels.Push((a.X, a.Y - 1));
                        pixels.Push((a.X, a.Y + 1));
                    }
                }
            }
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        { 
            List<(string, int, string)> items = new();

            for (int i = 0; i < lines.Length; i++)
            {
                var splits = lines[i].Split(" ");
                items.Add((splits[0], Int32.Parse(splits[1]), splits[2].Substring(2, 6)));
            }

            List<(long x, long y)> path = new List<(long, long)>();
            (long x, long y) current = (0, 0);
            path.Add(current);
            long result = 0;
            foreach (var item in items)
            {
                result += Convert.ToInt32(item.Item3.Substring(0, 5), 16); //edge is part of area
                switch (item.Item3.Last())
                {
                    case '0':
                        current.x += Convert.ToInt32(item.Item3.Substring(0, 5), 16);
                        break;
                    case '3':
                        current.y -= Convert.ToInt32(item.Item3.Substring(0,5),16);
                        break;
                    case '1':
                        current.y += Convert.ToInt32(item.Item3.Substring(0, 5), 16);
                        break;
                    case '2':
                        current.x -= Convert.ToInt32(item.Item3.Substring(0, 5), 16);
                        break;
                }
                path.Add(current);
            }

            //Solve                        
            for(int i = 1; i < path.Count; i++)
            {
                result += path[i - 1].x * path[i].y - path[i].x * path[i - 1].y;
            }            
            
            return Math.Abs(result)/2;
        }
    }
}
