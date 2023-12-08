using System;
using System.IO;
using System.Linq;
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
using AOCLib;

namespace Solutions.Day08
{
    public class SolverDay08 : ISolver
    {
        public long SolvePart1(string[] lines)
        {
            long result = 0;
            Dictionary<string,(string,string)> rules = new Dictionary<string, (string, string)>();

            string LR = lines[0];
            for (int i = 1; i < lines.Length; i++)
            {
                var splits = lines[i].Split('=');
                string key = splits[0].Trim();
                var values = splits[1].Trim().Split(",");
                string v1 = values[0].Trim().Substring(1);
                string v2 = values[1].Trim().Substring(0,3);
                rules.Add(key, (v1, v2));
            }

            //Solve
            string current = "AAA";
            int index = 0;
            while (current != "ZZZ")
            {
                if (index >= LR.Length) index = 0;
                if (LR[index] == 'R')
                    current = rules[current].Item2;
                else
                    current = rules[current].Item1;
                index++;
                result++;
            }

            return result;
        }

        public long SolvePart2(string[] lines)
        {
            long result = 1;
            Dictionary<string, (string, string)> rules = new Dictionary<string, (string, string)>();
            List<string> starts = new List<string>();

            string LR = lines[0];
            for (int i = 1; i < lines.Length; i++)
            {
                var splits = lines[i].Split('=');
                string key = splits[0].Trim();
                var values = splits[1].Trim().Split(",");
                string v1 = values[0].Trim().Substring(1);
                string v2 = values[1].Trim().Substring(0, 3);
                rules.Add(key, (v1, v2));
                if(key.EndsWith("A")) starts.Add(key);
            }

            //Solve
            List<long> fooos = new List<long>();

            foreach (string foosEntry in starts)
            {
                string current = foosEntry;
                int index = 0;
                long foo = 0;
                while (!current.EndsWith("Z"))
                {
                    if (index >= LR.Length) index = 0;
                    if (LR[index] == 'R')
                            current = rules[current].Item2;
                    else
                            current = rules[current].Item1;
                    index++;
                    foo++;
                }
                fooos.Add(foo);
            }

            result = fooos[0];
            for(int i = 1; i < fooos.Count; i++)
            {
                result = lcm(result, fooos[i]);
            }

            return result;
        }

        static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}
