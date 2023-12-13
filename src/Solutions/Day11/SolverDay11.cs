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

namespace Solutions.Day11
{
    public class SolverDay11 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            int[,] gal = new int[lines.Length, lines[0].Length];
            List<(int x, int y)> gals = new();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    gal[i, j] = 1;
                    if (lines[i][j] == '#')
                    {                     
                        gals.Add((j, i));
                    }
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].All(x => x == '.'))
                    for (int j = 0; j < lines[i].Length; j++)
                        {
                            gal[i, j] = 2;
                        }
            }
            for (int i = 0; i < lines[0].Length; i++)
            {
                bool alldot = true;
                for (int j = 0; j < lines.Length; j++)
                {
                    alldot &= lines[j][i] == '.';
                }
                if (alldot)
                    for (int j = 0; j < lines.Length; j++)
                    {
                        gal[j, i] = 2;
                    }
            }


            
            //Solve
            long result = 0;
            
            for (int i = 0; i < gals.Count; i++)
            {
                for (int j = i; j < gals.Count; j++)
                {
                    int movex = gals[j].x - gals[i].x;
                    int movey = gals[j].y - gals[i].y;
                    for (int x = 1; x <= Math.Abs(movex); x++)
                    {
                        result += gal[gals[i].y, gals[i].x + (x*Math.Sign(movex))];
                    }
                    for (int y = 1; y <= Math.Abs(movey); y++)
                    {
                        result += gal[gals[i].y+ (y * Math.Sign(movey)), gals[i].x];
                    }
                }
            }

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {            
            int[,] gal = new int[lines.Length, lines[0].Length];
            List<(int x, int y)> gals = new();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    gal[i, j] = 1;
                    if (lines[i][j] == '#')
                    {
                        gals.Add((j, i));
                    }
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].All(x => x == '.'))
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        gal[i, j] = 1000000;
                    }
            }
            for (int i = 0; i < lines[0].Length; i++)
            {
                bool alldot = true;
                for (int j = 0; j < lines.Length; j++)
                {
                    alldot &= lines[j][i] == '.';
                }
                if (alldot)
                    for (int j = 0; j < lines.Length; j++)
                    {
                        gal[j, i] = 1000000;
                    }
            }

            //Solve
            long result = 0;

            for (int i = 0; i < gals.Count; i++)
            {
                for (int j = i; j < gals.Count; j++)
                {
                    int movex = gals[j].x - gals[i].x;
                    int movey = gals[j].y - gals[i].y;
                    for (int x = 1; x <= Math.Abs(movex); x++)
                    {
                        result += gal[gals[i].y, gals[i].x + (x * Math.Sign(movex))];
                    }
                    for (int y = 1; y <= Math.Abs(movey); y++)
                    {
                        result += gal[gals[i].y + (y * Math.Sign(movey)), gals[i].x];
                    }
                }
            }

            return result;
        }
    }
}
