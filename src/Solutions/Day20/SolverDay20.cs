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
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using static System.Net.Mime.MediaTypeNames;
using CommandLine;

namespace Solutions.Day20
{
    public class SolverDay20 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            List<FF> FFs = new List<FF>();
            List<Conjunction> Cons = new List<Conjunction>();
            List<string> broadcast = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                var key = lines[i].Substring(1).Split("->")[0].Trim();
                var items = lines[i].Substring(1);
                if (lines[i].StartsWith("%"))
                {
                    FFs.Add(new FF(key, items));
                }
                else if (lines[i].StartsWith("&"))
                {
                    Cons.Add(new Conjunction(key, items));
                }
                else
                {
                    var split = lines[i].Split("->")[1].Trim();
                    var nexts = split.Split(",");
                    foreach (var n in nexts)
                        broadcast.Add(n.Trim());
                }
            }

            foreach (var x in FFs)
            {
                foreach (var y in x.next)
                {
                    var valids = Cons.Where(z => z.key == y);
                    foreach (var v in valids)
                    {
                        v.state.Add(x.key, 0);
                    }
                }
            }
            foreach (var x in Cons)
            {
                foreach (var y in x.next)
                {
                    var valids = Cons.Where(z => z.key == y);
                    foreach (var v in valids)
                    {
                        v.state.Add(x.key, 0);
                    }
                }
            }
            foreach (var x in broadcast)
            {
                var valids = Cons.Where(z => z.key == x);
                foreach (var v in valids)
                {
                    v.state.Add(x, 0);
                }
                
            }

            long lows = 1000;
            long highs = 0;            
            for(int i = 0; i < 1000; i++)
            {
                List<(string key, int signal, string sender)> todos = new List<(string key, int signal, string sender)>();
                foreach(var but in broadcast)
                {
                    todos.Add((but, 0, "broadcaster"));
                }
                while(todos.Count > 0)
                {
                    var current = todos.First();
                    todos.RemoveAt(0);
                    if (current.signal == 0) lows++;
                    if (current.signal == 1) highs++;
                    if (FFs.Where(x => x.key == current.key).Count() > 0)
                    {
                        todos.AddRange(FFs.Where(x => x.key == current.key).First().Receive(current.signal));
                    }
                    if (Cons.Where(x => x.key == current.key).Count() > 0)
                    {
                        todos.AddRange(Cons.Where(x => x.key == current.key).First().Receive(current.signal, current.sender));
                    }
                }
            }

            //Solve
            long result = lows*highs;
            return result;
        }

        public class FF
        {
            public List<string> next;
            public string key;
            public int state;

            public FF(string key, string parse)
            {
                this.key = key;
                next = new List<string>();
                var split = parse.Split("->")[1].Trim();
                var nexts = split.Split(",");
                foreach (var n in nexts)
                    next.Add(n.Trim());
            }

            public List<(string, int, string)> Receive(int signal)
            {
                List<(string, int, string)> send = new List<(string, int, string)>();
                if(signal == 1)
                {
                    
                }
                if(signal == 0)
                {
                    this.state = (this.state + 1) % 2;
                    foreach (var n in next)
                    {
                        send.Add((n, this.state, this.key));
                    }
                }
                return send;
            }
        }
        
        public class Conjunction
        {
            public List<string> next;
            public Dictionary<string,int> state;
            public string key;

            public Conjunction(string key, string parse)
            {
                this.key = key;
                next = new List<string>();
                state = new();
                var split = parse.Split("->")[1].Trim();
                var nexts = split.Split(",");
                foreach (var n in nexts)
                    next.Add(n.Trim());
            }

            public List<(string, int, string)> Receive(int signal, string key)
            {
                List<(string, int, string)> send = new List<(string, int, string)>();
                state[key] = signal;
                if (state.All(x => x.Value == 1))
                {
                    foreach (var n in next)
                    {
                        send.Add((n, 0, this.key));
                    }
                }
                else
                {
                    foreach (var n in next)
                    {
                        send.Add((n, 1, this.key));
                    }
                }
                return send;
            }
        }        

        public long SolvePart2(string[] lines, string text)
        {
            List<FF> FFs = new List<FF>();
            List<Conjunction> Cons = new List<Conjunction>();
            List<string> broadcast = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                var key = lines[i].Substring(1).Split("->")[0].Trim();
                var items = lines[i].Substring(1);
                if (lines[i].StartsWith("%"))
                {
                    FFs.Add(new FF(key, items));
                }
                else if (lines[i].StartsWith("&"))
                {
                    Cons.Add(new Conjunction(key, items));
                }
                else
                {
                    var split = lines[i].Split("->")[1].Trim();
                    var nexts = split.Split(",");
                    foreach (var n in nexts)
                        broadcast.Add(n.Trim());
                }
            }

            foreach (var x in FFs)
            {
                foreach (var y in x.next)
                {
                    var valids = Cons.Where(z => z.key == y);
                    foreach (var v in valids)
                    {
                        v.state.Add(x.key, 0);
                    }
                }
            }
            foreach (var x in Cons)
            {
                foreach (var y in x.next)
                {
                    var valids = Cons.Where(z => z.key == y);
                    foreach (var v in valids)
                    {
                        v.state.Add(x.key, 0);
                    }
                }
            }
            foreach (var x in broadcast)
            {
                var valids = Cons.Where(z => z.key == x);
                foreach (var v in valids)
                {
                    v.state.Add(x, 0);
                }
            }

            List<string> needed = new();
            string prev = "";
            foreach (var x in Cons)
            {
                foreach (var y in x.next)
                {
                    if (y == "rx")
                        prev = x.key;
                }
            }
            foreach (var x in Cons)
            {
                foreach (var y in x.next)
                {
                    if (y == prev)
                        needed.Add(x.key);
                }
            }



            long result = 0;
            Dictionary<string, long> loops = new();         
            for (int i = 0; i < 100000000; i++)
            {
                List<(string key, int signal, string sender)> todos = new List<(string key, int signal, string sender)>();
                foreach (var but in broadcast)
                {
                    todos.Add((but, 0, "broadcaster"));
                }                
                result++;
                while (todos.Count > 0)
                {
                    var current = todos.First();
                    todos.RemoveAt(0);
                    if(needed.Contains(current.key) && current.signal == 0)
                    {
                        if (!loops.ContainsKey(current.key))
                            loops[current.key] = result;
                    }
                    if(loops.Count == needed.Count)
                    {
                        return mlcm(loops.Select(x => x.Value).ToList());
                    }
                    if (FFs.Where(x => x.key == current.key).Count() > 0)
                    {
                        todos.AddRange(FFs.Where(x => x.key == current.key).First().Receive(current.signal));
                    }
                    if (Cons.Where(x => x.key == current.key).Count() > 0)
                    {
                        todos.AddRange(Cons.Where(x => x.key == current.key).First().Receive(current.signal, current.sender));
                    }
                }
            }
            //Solve
            return result;
        }

        private long mlcm(List<long> t)
        {
            long res = 1;
            foreach(var x in t)
            {
                res = lcm(res, x);
            }
            return res;
        }

        public long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}
