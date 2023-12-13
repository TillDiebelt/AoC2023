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

namespace Solutions.Day13
{
    public class SolverDay13 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            var mazes = text.Split("\n\n");

            List<int[,]> parse = new();
            foreach(var m in mazes)
            {
                var mLines = m.Split("\n",StringSplitOptions.RemoveEmptyEntries);
                var cur =  new int[mLines.Length, mLines[0].Length];

                for (int i = 0; i < mLines.Length; i++)
                {
                    for (int j = 0; j < mLines[i].Length; j++)
                    {
                        if (mLines[i][j] == '#')
                            cur[i, j] = 1;
                    }
                }
                parse.Add(cur);
            }


            long result = 0;
            foreach (var m in parse)
            {
                for (int i = 1; i <= m.GetUpperBound(0); i++)
                {
                    if (IsHorCopy(m, i - 1, i))
                    {
                        int c = Math.Min(i-1, m.GetUpperBound(0) - i);
                        bool all = true;
                        for(int j = 1; j <= c; j++)
                        {
                            all &= IsHorCopy(m, i + j, i - j - 1);
                        }
                        if (all)
                        {
                            result += i * 100;
                            break;
                        }
                    }
                }
                for (int i = 1; i <= m.GetUpperBound(1); i++)
                {
                    if (IsVertCopy(m, i - 1, i))
                    {
                        int c = Math.Min(i-1, m.GetUpperBound(1) - i);
                        bool all = true;
                        for (int j = 1; j <= c; j++)
                        {
                            all &= IsVertCopy(m, i + j, i - j - 1);
                        }
                        if (all)
                        {
                            result += i;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public bool IsHorCopy(int[,] m, int y1, int y2)
        {
            bool eq = true;
            for (int j = 0; j <= m.GetUpperBound(1); j++)
            {
                eq &= m[y1, j] == m[y2, j];
            }
            return eq;
        }
        
        public bool IsVertCopy(int[,] m, int x1, int x2)
        {
            bool eq = true;
            for (int j = 0; j <= m.GetUpperBound(0); j++)
            {
                eq &= m[j,x1] == m[j, x2];
            }
            return eq;
        }

        public long SolvePart2(string[] lines, string text)
        {
            var mazes = text.Split("\n\n");

            List<int[,]> parse = new();
            foreach (var m in mazes)
            {
                var mLines = m.Split("\n",StringSplitOptions.RemoveEmptyEntries);
                var cur = new int[mLines.Length, mLines[0].Length];

                for (int i = 0; i < mLines.Length; i++)
                {
                    for (int j = 0; j < mLines[i].Length; j++)
                    {
                        if (mLines[i][j] == '#')
                            cur[i, j] = 1;
                    }
                }
                parse.Add(cur);
            }

            
            long result = 0;
            foreach (var m in parse)
            {
                for (int i = 1; i <= m.GetUpperBound(0); i++)
                {
                    int c = Math.Min(i - 1, m.GetUpperBound(0) - i);
                    int sum = 0;
                    for(int j = 0; j <= c; j++)
                    {
                        for (int x = 0; x <= m.GetUpperBound(1); x++)
                        {
                            sum += m[i+j, x] ^ m[i-j-1, x];
                        }
                    }
                    if(sum == 1)
                    {
                        result += i * 100;
                        break;
                    }
                }
                for (int i = 1; i <= m.GetUpperBound(1); i++)
                {
                    int c = Math.Min(i - 1, m.GetUpperBound(1) - i);
                    int sum = 0;
                    for (int j = 0; j <= c; j++)
                    {
                        for (int y = 0; y <= m.GetUpperBound(0); y++)
                        {
                            sum += m[y,i + j] ^ m[y,i - j - 1];
                        }
                    }
                    if (sum == 1)
                    {
                        result += i;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
