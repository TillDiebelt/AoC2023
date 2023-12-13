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
using GIGACHADINT = System.Numerics.BigInteger;

namespace Solutions.Day09
{
    public class SolverDay09 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            List<List<long>> seq = new List<List<long>>();
            
            for(int i = 0; i < lines.Length; i++)
            {
                var longs = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                seq.Add(longs);
            }

            long result = 0;
            
            for (int i = 0; i < seq.Count(); i++)
            {
                List<List<long>> colls = new List<List<long>>();
                List<long> diffs = new List<long>(seq[i]);
                colls.Add(diffs);
                while(!diffs.All(x => x == 0))
                {
                    List<long> tmp = new List<long>();
                    for (int j = 1; j < diffs.Count; j++)
                    {
                        tmp.Add(diffs[j] - diffs[j-1]);
                    }
                    colls.Add(tmp);
                    diffs = tmp;
                }
                long current = 0;
                for(int j = colls.Count()-2; j >= 0; j--)
                {
                    current += colls[j].Last();
                }
                result += current;
            }

            //Solve
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            List<List<long>> seq = new List<List<long>>();

            for (int i = 0; i < lines.Length; i++)
            {
                var longs = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                seq.Add(longs);
            }

            long result = 0;

            for (int i = 0; i < seq.Count(); i++)
            {
                List<List<long>> colls = new List<List<long>>();
                List<long> diffs = new List<long>(seq[i]);
                colls.Add(diffs);
                while (!diffs.All(x => x == 0))
                {
                    List<long> tmp = new List<long>();
                    for (int j = 1; j < diffs.Count; j++)
                    {
                        tmp.Add(diffs[j] - diffs[j - 1]);
                    }
                    colls.Add(tmp);
                    diffs = tmp;
                }
                long current = 0;
                for (int j = colls.Count() - 2; j >= 0; j--)
                {
                    current = (colls[j].First() - current);
                }
                result += current;
            }

            //Solve
            return result;
        }
    }
}
