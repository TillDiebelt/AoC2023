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

namespace Solutions.Day03
{
    public class SolverDay03 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            var needParse = new bool[lines.Length, lines[0].Length];
            for(int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    if (!char.IsDigit(lines[i][j]) && (lines[i][j] != '.'))
                    {
                        var neigh = needParse.NeighboursDiag(i,j);
                        int count = 0;
                        foreach (var p in neigh)
                        {
                            if(char.IsDigit(lines[p.x][p.y])) count++;
                            needParse[p.x, p.y] = true;
                        }
                    }
                }
            }

            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    if (needParse[i,j] && char.IsDigit(lines[i][j]))
                    {
                        while (j > 0 && char.IsDigit(lines[i][j])) j--;
                        if (!char.IsDigit(lines[i][j])) j++;
                        string toParse = "";
                        while (j < lines[0].Length && char.IsDigit(lines[i][j]))
                        {
                            toParse += lines[i][j];
                            j++;
                        }
                        result += Convert.ToInt64(toParse);
                    }
                }
            }

            //Solve
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            int uni = 1;
            var needParse = new int[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    if ((lines[i][j] == '*'))
                    {
                        var neigh = needParse.NeighboursDiag(i, j);
                        int count = 0;
                        bool t = false;
                        int a = 0;
                        for(int y = -1; y < 2; y++)
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                if((i + y >= 0 && i + y < lines.Length && j + x >= 0 && j + x < lines[0].Length) && char.IsDigit(lines[i+y][j+x]))
                                {
                                    count++;
                                    while ((i + y >= 0 && i + y < lines.Length && j + x >= 0 && j + x < lines[0].Length) && char.IsDigit(lines[i + y][j + x])) x++;
                                }
                            }
                        }
                        if (count == 2)
                        {
                            foreach (var p in neigh)
                            {
                                needParse[p.x, p.y] = uni;
                            }
                            uni++;
                        }
                    }
                }
            }

            long result = 0;
            Dictionary<int, long> values = new Dictionary<int, long>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    if ((needParse[i, j] > 0) && char.IsDigit(lines[i][j]))
                    {
                        var k = j;
                        while (j > 0 && char.IsDigit(lines[i][j])) j--;
                        if (!char.IsDigit(lines[i][j])) j++;
                        string toParse = "";
                        while (j < lines[0].Length && char.IsDigit(lines[i][j]))
                        {
                            toParse += lines[i][j];
                            j++;
                        }
                        if (values.ContainsKey(needParse[i, k]))
                        {
                            values[needParse[i, k]] *= Convert.ToInt64(toParse);
                        }
                        else
                        {
                            values.Add(needParse[i, k], Convert.ToInt64(toParse));
                        }
                    }
                }
            }

            foreach (var k in values)
            {
                result += k.Value;
            }
            return result;
        }
    }
}
