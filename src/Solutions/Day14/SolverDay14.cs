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

namespace Solutions.Day14
{
    public class SolverDay14 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Trim();
            int[,] map = new int[lines.Length, lines[0].Length];
            for(int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch (lines[i][j])
                    {
                        case 'O':
                            map[i, j] = 2;
                            break;
                        case '#':
                            map[i, j] = 1;
                            break;
                    }
                }
            }


            for (int i = 1; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (map[i,j] == 2)
                    {
                        map[i, j] = 0;
                        int y = i;
                        while(y >= 0 && map[y, j] == 0)
                        {
                            y--;
                        }
                        y++;
                        map[y, j] = 2;
                    }
                }
            }
            
            //Solve
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (map[i, j] == 2)
                    {
                        result += lines.Length - i;
                    }
                }
            }
            
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Trim();
            int[,] map = new int[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch (lines[i][j])
                    {
                        case 'O':
                            map[i, j] = 2;
                            break;
                        case '#':
                            map[i, j] = 1;
                            break;
                    }
                }
            }


            List<(string,int)> seen = new List<(string, int)>();
            bool finalLoops = false;
            int loops = 1000;
            string key = MapToString(map);
            seen.Add((key,0));
            for (int r = 1; r <= loops; r++)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (map[i, j] == 2)
                        {
                            map[i, j] = 0;
                            int y = i;
                            while (y >= 0 && map[y, j] == 0)
                            {
                                y--;
                            }
                            y++;
                            map[y, j] = 2;
                        }
                    }
                }
                for (int i = 1; i < lines[0].Length; i++)
                {
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (map[j, i] == 2)
                        {
                            map[j, i] = 0;
                            int x = i;
                            while (x >= 0 && map[j, x] == 0)
                            {
                                x--;
                            }
                            x++;
                            map[j,x] = 2;
                        }
                    }
                }
                for (int i = lines.Length - 2; i >= 0; i--)
                {
                    for (int j = 0; j < lines[0].Length; j++)
                    {
                        if (map[i, j] == 2)
                        {
                            map[i, j] = 0;
                            int y = i;
                            while (y < lines.Length && map[y, j] == 0)
                            {
                                y++;
                            }
                            y--;
                            map[y, j] = 2;
                        }
                    }
                }
                for (int i = lines[0].Length - 2; i >= 0; i--)
                {
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (map[j, i] == 2)
                        {
                            map[j, i] = 0;
                            int x = i;
                            while (x < lines[0].Length && map[j, x] == 0)
                            {
                                x++;
                            }
                            x--;
                            map[j, x] = 2;
                        }
                    }
                }
                if (!finalLoops)
                {
                    key = MapToString(map);
                    if (seen.Any(x => x.Item1 == key))
                    {
                        int foo = seen.Where(x => x.Item1 == key).First().Item2;
                        int foo2 = r- foo;
                        while (foo + foo2 <= loops)
                            foo += foo2;
                        r = foo;
                        finalLoops = true;
                    }
                    else
                        seen.Add((key,r));
                }
            }

            //Solve
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (map[i, j] == 2)
                    {
                        result += lines.Length - i;
                    }
                }
            }

            return result;
        }

        private string MapToString(int[,] m)
        {
            string res = "";

            for (int i = 0; i <= m.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= m.GetUpperBound(1); j++)
                {
                    res += ""+m[i,j];
                }
            }
            return res;
        }
    }
}
