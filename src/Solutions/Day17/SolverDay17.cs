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
using Microsoft.VisualBasic;
using System.Runtime.ConstrainedExecution;
using BenchmarkDotNet.Disassemblers;
using Microsoft.CodeAnalysis;

namespace Solutions.Day17
{
    public class SolverDay17 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            int[,] map = new int[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j].ToDigit();
                }
            }
            
            int max = 3;
            int min = 0;
            var path = Pathfinder<int[,], (int x, int y, int dx, int dy)>.FindPath(
                map,
                (a) => new List<(int, int,int,int)>() { (0,0,0,0) },
                (a, b) => (b.x == a.GetUpperBound(1) && b.y == a.GetUpperBound(0)),
                (a, b) => Math.Abs(b.x - a.GetUpperBound(1)) + Math.Abs(b.y - a.GetUpperBound(0)),
                (a, b, c) => a[c.y, c.x],
                (a, current) => {
                        var res = new List<(int x, int y, int dx, int dy)>();
                        if (current.x - 1 >= 0 && current.dx <= 0 && current.dx - 1 >= -max && (Math.Abs(current.dy) >= min || current.dy == 0))
                        res.Add((current.x - 1, current.y, current.dx - 1, 0));
                        if (current.x + 1 <= a.GetUpperBound(1) && current.dx >= 0 && current.dx + 1 <= max && (Math.Abs(current.dy) >= min || current.dy == 0))
                        res.Add((current.x + 1, current.y, current.dx + 1, 0));

                        if (current.y - 1 >= 0 && current.dy <= 0 && current.dy - 1 >= -max && (Math.Abs(current.dx) >= min || current.dx == 0))
                        res.Add((current.x, current.y - 1, 0, current.dy - 1));
                        if (current.y + 1 <= a.GetUpperBound(0) && current.dy >= 0 && current.dy + 1 <= max && (Math.Abs(current.dx) >= min || current.dx == 0))
                        res.Add((current.x, current.y + 1, 0, current.dy + 1));
                        return res;
                    }
                ); ;
            //Solve   
            long result = 0;
            
            foreach(var i in path)
            {
                result += map[i.y,i.x];   
            }
            
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            int[,] map = new int[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j].ToDigit();
                }
            }

            int max = 10;
            int min = 4;
            var path = Pathfinder<int[,], (int x, int y, int dx, int dy)>.FindPath(
                map,
                (a) => new List<(int, int, int, int)>() { (0, 0, 0, 0) },
                (a, b) => (b.x == a.GetUpperBound(1) && b.y == a.GetUpperBound(0)),
                (a, b) => Math.Abs(b.x - a.GetUpperBound(1)) + Math.Abs(b.y - a.GetUpperBound(0)),
                (a, b, c) => a[c.y, c.x],
                (a, current) => {
                    var res = new List<(int x, int y, int dx, int dy)>();
                    if (current.x - 1 >= 0 && current.dx <= 0 && current.dx - 1 >= -max && (Math.Abs(current.dy) >= min || current.dy == 0))
                        res.Add((current.x - 1, current.y, current.dx - 1, 0));
                    if (current.x + 1 <= a.GetUpperBound(1) && current.dx >= 0 && current.dx + 1 <= max && (Math.Abs(current.dy) >= min || current.dy == 0))
                        res.Add((current.x + 1, current.y, current.dx + 1, 0));

                    if (current.y - 1 >= 0 && current.dy <= 0 && current.dy - 1 >= -max && (Math.Abs(current.dx) >= min || current.dx == 0))
                        res.Add((current.x, current.y - 1, 0, current.dy - 1));
                    if (current.y + 1 <= a.GetUpperBound(0) && current.dy >= 0 && current.dy + 1 <= max && (Math.Abs(current.dx) >= min || current.dx == 0))
                        res.Add((current.x, current.y + 1, 0, current.dy + 1));
                    return res;
                }
                );
            
            //Solve   
            long result = 0;

            foreach (var i in path)
            {
                result += map[i.y, i.x];
            }

            return result;
        }
    }
}
