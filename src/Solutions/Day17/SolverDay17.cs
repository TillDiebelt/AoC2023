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

            Pathfinder4D finder = new Pathfinder4D(map);
            var path = finder.FindPath(0, 3);
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

            Pathfinder4D finder = new Pathfinder4D(map);
            var path = finder.FindPath(4, 10);
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
