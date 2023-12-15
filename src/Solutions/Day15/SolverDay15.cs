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

namespace Solutions.Day15
{
    public class SolverDay15 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            var items = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            long result = 0;
            foreach (var i in items)
            {
                int cur = 0;
                for(int x = 0; x < i.Length; x++)
                {
                    cur += (int)i[x];
                    cur *= 17;
                    cur %= 256;
                }
                result += cur;
            }

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            var items = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            Dictionary<int, List<(string, int)>> map = new();

            long result = 0;
            foreach (var i in items)
            {
                int cur = 0;
                string s = "";
                for (int x = 0; x < i.Length; x++)
                {
                    if (i[x] == '=' || i[x] == '-')
                    {
                        if (!map.ContainsKey(cur))
                            map[cur] = new List<(string, int)>();

                        if (i[x] == '=')
                        {
                            if (map[cur].Any(x => x.Item1 == s))
                            {
                                for(int y = 0; y < map[cur].Count; y++)
                                {
                                    if (map[cur][y].Item1 == s)
                                        map[cur][y] = (s, i[x + 1].ToDigit());
                                }
                            }
                            else
                            {
                                map[cur].Add((s, i[x + 1].ToDigit()));
                            }
                        }

                        if (i[x] == '-')
                        {
                            if (map[cur].Any(x => x.Item1 == s))
                            {
                                map[cur].RemoveAll(x => x.Item1 == s);
                            }
                        }
                        break;
                    }
                    cur += (int)i[x];
                    cur *= 17;
                    cur %= 256;
                    s += i[x];
                }
            }
            
            foreach(var n in map)
            {
                int i = 1;
                foreach(var m in n.Value)
                {
                    result += (1 + n.Key) * i * m.Item2;
                    i++;
                }
            }

            return result;
        }
    }
}
