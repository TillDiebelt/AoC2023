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

namespace Solutions.Day04
{
    public class SolverDay04 : ISolver
    {
        public long SolvePart1(string[] lines)
        {
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var splits = lines[i].Split(":")[1];
                var winning = splits.Split("|")[0].Trim().Split(" ",StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                var me = splits.Split("|")[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                var foo = winning.Intersect(me);
                if(foo.Count()>0)
                    result += (long)Math.Pow(2, foo.Count()-1);
            }
            
            //Solve
            return result;
        }

        public long SolvePart2(string[] lines)
        {
            long result = 0;
            Dictionary<int, int> count = new Dictionary<int, int>();
            for (int i = 0; i < lines.Length; i++)
            {
                var splits = lines[i].Split(":")[1];
                var winning = splits.Split("|")[0].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                var me = splits.Split("|")[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                var foo = winning.Intersect(me);
                result++;
                if (foo.Count() > 0 || count.ContainsKey(i))
                {
                    if (count.ContainsKey(i))
                    {
                        result--;
                        for (int t = 0; t <= count[i]; t++)
                        {
                            result++;
                            for (int j = 1; j <= foo.Count(); j++)
                            {
                                if (count.ContainsKey(i + j))
                                {
                                    count[i + j]++;
                                }
                                else
                                {
                                    count.Add(i + j, 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 1; j <= foo.Count(); j++)
                        {
                            if (count.ContainsKey(i + j))
                            {
                                count[i + j]++;
                            }
                            else
                            {
                                count.Add(i + j, 1);
                            }
                        }
                    }
                }
            }

            //Solve
            return result;
        }
    }
}
