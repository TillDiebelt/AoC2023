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

namespace Solutions.Day12
{
    public class SolverDay12 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            List<(string s, List<int> arrangement)> springs = new();
            for(int i = 0; i < lines.Length; i++)
            {
                var splits = lines[i].Split(" ");
                string sp = splits[0].Trim();
                var arr = splits[1].Trim().Split(",").Select(x => Int32.Parse(x)).ToList();
                springs.Add((sp+".", arr));
            }


            long result = 0;
            for (int i = 0; i < springs.Count; i++)
            {
                Console.WriteLine(i + ": " + Possibilities(springs[i].s, springs[i].arrangement));
            }
            /*
            //Bruteforce yeah, part 2 wont punish that D:
            for (int i = 0; i < springs.Count; i++)
            {
                var cobis = Combine(springs[i].s);
                foreach(var c in cobis)
                {
                    var counts = Count(c);
                    if (counts.Count() != springs[i].arrangement.Count()) continue;
                    bool sa = true;
                    
                    for(int j = 0; j < counts.Count(); j++)
                    {
                        sa &= springs[i].arrangement[j] == counts[j];
                    }
                        
                    if(sa)
                        result++;
                }
            }*/

            return result;
        }

        private List<int> Count(string input)
        {
            List<int> res = new();
            int cur = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '.')
                {
                    if (cur != 0) res.Add(cur);
                    cur = 0;
                }
                else
                    cur++;
            }
            if (cur != 0) res.Add(cur);
            return res;
        }
        
        private List<string> Combine(string input)
        {
            List<string> res = new();
            if (input[0] == '?')
            {
                res.Add("#");
                res.Add(".");
            }
            else
            {
                res.Add(input[0]+"");
            }
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == '?')
                {
                    int c = res.Count();
                    for (int j = 0; j < c; j++)
                    {
                        res.Add(res[j] + "#");
                        res[j] += ".";
                    }
                }
                else
                {
                    int c = res.Count();
                    for (int j = 0; j < c; j++)
                    {
                        res[j] += input[i];
                    }
                }
            }
            return res;
        }

        public long SolvePart2(string[] lines, string text)
        {
            List<(string s, List<int> arrangement)> springs = new();

            for (int i = 0; i < lines.Length; i++)
            {
                var splits = lines[i].Split(" ");
                string sp = splits[0].Trim();
                string spR = "";
                for(int j = 0; j < 4; j++)
                {
                    spR += sp + "?";
                }
                spR += sp + "."; //dot for safety +-1
                var arr = splits[1].Trim().Split(",").Select(x => Int32.Parse(x)).ToList();
                List<int> arr5 = new List<int>();
                for(int j = 0; j < 5; j++)
                {
                    arr5.AddRange(arr);
                }
                springs.Add((spR, arr5));
            }


            long result = 0;
            for (int i = 0; i < springs.Count; i++)
            {
                Console.WriteLine(i + ": " + Possibilities(springs[i].s, springs[i].arrangement));
            }

            //Solve
            return result;
        }

        public static Dictionary<string, long> dyn = new();

        private long Possibilities(string cur, List<int> springs)
        {
            if (springs.Count == 0)
            {
                if (cur.Contains('#')) return 0; //fuuuuuuuuuuuu
                return 1;
            }

            if (cur.Length == 0) return 0; //duuuh
            if (dyn.ContainsKey((cur+LToS(springs)))) //make fast
                return dyn[(cur+LToS(springs))];
            if (cur[0] == '.') return Possibilities(cur.Substring(1), springs); //always skip starting dot, cant be spring
            int offset = 0;
            int cnt = springs[0];
            long pos = 0;
            while ((cur.Length-offset) >= (springs.Sum() + springs.Count - 1)) //super duper fast
            {
                bool t = true;
                for (int i = 0; i < cnt; i++)
                {
                    t &= cur[offset + i] != '.';
                }
                if(t) //spring at offset is at least of next length
                {
                    if (offset + cnt < cur.Length && cur[offset + cnt] == '#') //spring must end on ? or .
                    {
                        if (cur[offset] == '#') break; //we can only skip . and ?
                        offset++; 
                        continue; 
                    }
                    pos += Possibilities(cur.Substring(offset + cnt + 1), springs.Skip(1).ToList());
                }
                if (cur[offset] == '#') break; //we can only skip . and ?
                offset++;
            }
            if (!dyn.ContainsKey((cur+LToS(springs)))) 
                dyn.Add((cur+LToS(springs)), pos);
            return pos;
        }

        private string LToS(List<int> l)
        {
            string r = "";
            for (int i = 0; i < l.Count - 1; i++)
                r += l[i] + ",";
            r += l.Last();
            return r;
        }
    }
}
