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
using BenchmarkDotNet.Validators;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace Solutions.Day19
{
    public class SolverDay19 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            List<Rule> rules = new List<Rule>();
            List<Part> parts = new List<Part>();
            int idx = 0;
            while (true)
            {
                if (lines[idx].Length == 0)
                    break;
                var splits = lines[idx].Split("{");
                string id = splits[0];
                rules.Add(new Rule(id, splits[1].Substring(0, splits[1].Length - 1)));
                idx++;
            }
            for (int i = idx + 1; i < lines.Length; i++)
            {
                parts.Add(new Part(lines[i]));
            }

            List<Part> acc = new List<Part>();
            foreach(var p in parts)
            {
                string cur = "in";
                while(cur != "A" && cur != "R")
                {
                    Rule r = rules.Where(x => x.id == cur).First();
                    cur = r.apply(p);
                    if (cur == "") throw new Exception();
                }
                if (cur == "A") acc.Add(p);
            }

            //Solve
            long result = acc.Sum(x => x.a + x.m + x.x + x.s);
            return result;
        }

        public class Part
        {
            public long x;
            public long m;
            public long a;
            public long s;

            public Part(string parse)
            {
                var splits = parse.Trim().Substring(1, parse.Length - 2).Split(",");
                x = Int64.Parse(splits[0].Split("=")[1]);
                m = Int64.Parse(splits[1].Split("=")[1]);
                a = Int64.Parse(splits[2].Split("=")[1]);
                s = Int64.Parse(splits[3].Split("=")[1]);
            }

            public long get(char id)
            {
                switch(id)
                {
                    case 'x': return x;
                    case 'm': return m;
                    case 'a': return a;
                    case 's': return s;
                }
                return 0;
            }
        }

        public class Rule
        {
            public string id;
            List<Func<Part, string>> rules;
            public Rule(string id, string parse)
            {
                this.id = id;
                rules = new List<Func<Part, string>>();
                var splits = parse.Split(",");
                foreach(var s in splits)
                {
                    if (s.Length == 1)
                        rules.Add((a) => { return s; });
                    else if (s[1] == '=')
                    {
                        var ret = s.Split(":");
                        rules.Add((a) => { return a.get(s[0]) == Int64.Parse(ret[0].Substring(2)) ? ret[1] : ""; });
                    }
                    else if (s[1] == '>')
                    {
                        var ret = s.Split(":");
                        rules.Add((a) => { return a.get(s[0]) > Int64.Parse(ret[0].Substring(2)) ? ret[1] : ""; });
                    }
                    else if (s[1] == '<')
                    {
                        var ret = s.Split(":");
                        rules.Add((a) => { return a.get(s[0]) < Int64.Parse(ret[0].Substring(2)) ? ret[1] : ""; });
                    }
                    else
                    {
                        rules.Add((a) => { return s; });
                    }
                }
            }
            
            public string apply(Part p)
            {
                foreach(var r in rules)
                {
                    if (r(p) != "") return r(p);
                }
                return "";
            }
        }
        
        public long SolvePart2(string[] lines, string text)
        {
            List<AbstractRule> rules = new List<AbstractRule>();
            List<Part> parts = new List<Part>();
            int idx = 0;
            while (true)
            {
                if (lines[idx].Length == 0)
                    break;
                var splits = lines[idx].Split("{");
                string id = splits[0];
                rules.Add(new AbstractRule(id, splits[1].Substring(0, splits[1].Length - 1)));
                idx++;
            }

            List<(string key, List<string> tos)> graph = new List<(string, List<string>)>();
            foreach (var r in rules)
            {
                List<string> goesTo = new List<string>();
                foreach(var t in r.to)
                {
                    goesTo.Add((t.Item2));
                }
                graph.Add((r.id, goesTo));
            }
            
            List<(string, List<int>)> useful = new List<(string, List<int>)>();
            for (int rep = 0; rep < rules.Count; rep++)
            {
                int c = 0;
                foreach (var g in graph.Where(x => !useful.Any( y => y.Item1 == x.key)))
                {
                    List<int> paths = new List<int>();
                    for (int i = 0; i < g.tos.Count; i++)
                    {
                        if (g.tos[i] == "A") 
                            paths.Add(i);
                        else if (useful.Any( x => x.Item1 == g.tos[i])) 
                            paths.Add(i);
                    }
                    if (paths.Count > 0)
                    {
                        useful.Add((g.key, paths));
                        c++;
                    }
                }
                foreach (var g in graph.Where(x => useful.Any(y => y.Item1 == x.key)))
                {
                    for (int i = 0; i < g.tos.Count; i++)
                    {
                        if (useful.Any(x => x.Item1 == g.tos[i]))
                        {
                            if (!useful.Where(x => g.key == x.Item1).First().Item2.Contains(i))
                            {
                                c++;
                                useful.Where(x => g.key == x.Item1).First().Item2.Add(i);
                            }
                        }
                    }
                }
                if (c == 0) break;
            }

            List<Condition> valids = new List<Condition>();
            /*
            var start = useful.Where(x => x.Item1 == "in").First();
            foreach (var p in start.Item2)
            {
                Condition newC = rules.Where(x => x.id == "in").First().to[p].Item1;
                valids.AddRange(Search(
                        rules.Where(x => x.id == "in").First().to[p].Item2,
                        newC,
                        p,
                        useful,
                        rules
                    )
                );
            }            */
            valids = Search2("in", new Condition(""), graph, rules);

            //Solve

            long result = 0;
            foreach (var v in valids)
            {
                result += v.combs();
            }

            return result;
            //167409079868000
            //496534091000000
        }

        private List<Condition> Search(string key, Condition current, int path, List<(string id, List<int> paths)> useful, List<AbstractRule> rules)
        {
            List<Condition> valids = new List<Condition>();

            Condition newC = current.merge(rules.Where(x => x.id == key).First().to[path].Item1);
            if (newC.combs() == 0) return valids;

            string nextKey = rules.Where(x => x.id == key).First().to[path].Item2;
            if (nextKey == "A")
            {
                valids.Add(newC);
                return valids;
            }
            var next = useful.Where(x => x.id == nextKey).First();
            foreach (var p in next.paths)
            {
                valids.AddRange(Search(
                        next.id,
                        newC,
                        p,
                        useful,
                        rules
                    )
                );
            }
            return valids;
        }
        
        private List<Condition> Search2(string key, Condition current, List<(string id, List<string> paths)> graph, List<AbstractRule> rules)
        {
            List<Condition> valids = new List<Condition>();

            foreach(var path in rules.Where(x => x.id == key).First().to)
            {
                Condition newC = current.merge(path.Item1);
                if (newC.combs() == 0) continue;

                string nextKey = path.Item2;
                if (nextKey == "A")
                {
                    valids.Add(newC);
                    continue;
                }
                if (nextKey == "R")
                {
                    continue;
                }
                valids.AddRange(Search2(
                        nextKey,
                        newC,
                        graph,
                        rules
                    )
                );
            }
            return valids;
        }

        public class Condition
        {
            public string c;

            long minx = 0;
            long maxx = 4001;
            long mina = 0;
            long maxa = 4001;
            long minm = 0;
            long maxm = 4001;
            long mins = 0;
            long maxs = 4001;
            
            public long combs()
            {
                return Math.Max(0, maxx - minx - 1)
                    * Math.Max(0, maxa - mina - 1)
                    * Math.Max(0, maxs - mins - 1)
                    * Math.Max(0, maxm - minm - 1);
            }
            
            public Condition(string parse)
            {
                c = parse;
                if(parse.Length > 0)
                {
                    if (parse.Split("<").Length == 0 || parse.Split(">").Length == 0)
                        return;
                    switch(parse[0])
                    {
                        case 'x':
                            if (parse[1] == '<')
                                maxx = Int64.Parse(parse.Substring(2));
                            else
                                minx = Int64.Parse(parse.Substring(2));
                            break;
                        case 'a':
                            if (parse[1] == '<')
                                maxa = Int64.Parse(parse.Substring(2));
                            else
                                mina = Int64.Parse(parse.Substring(2));
                            break;
                        case 'm':
                            if (parse[1] == '<')
                                maxm = Int64.Parse(parse.Substring(2));
                            else
                                minm = Int64.Parse(parse.Substring(2));
                            break;
                        case 's':
                            if (parse[1] == '<')
                                maxs = Int64.Parse(parse.Substring(2));
                            else
                                mins = Int64.Parse(parse.Substring(2));
                            break;
                        default:
                            break;
                    }
                }
            }
            
            public Condition merge(Condition a)
            {
                Condition newC = new("");
                newC.minx = Math.Max(this.minx, a.minx);
                newC.maxx = Math.Min(this.maxx, a.maxx);
                newC.mina = Math.Max(this.mina, a.mina);
                newC.maxa = Math.Min(this.maxa, a.maxa);
                newC.minm = Math.Max(this.minm, a.minm);
                newC.maxm = Math.Min(this.maxm, a.maxm);
                newC.mins = Math.Max(this.mins, a.mins);
                newC.maxs = Math.Min(this.maxs, a.maxs);
                return newC;
            }
        }

        public class AbstractRule
        {
            public string id;
            List<Func<Part, string>> rules;
            public List<(Condition, string)> to;
            
            public AbstractRule(string id, string parse)
            {
                this.id = id;
                rules = new List<Func<Part, string>>();
                to = new();
                var splits = parse.Split(",");
                foreach (var s in splits)
                {
                    if (s.Length == 1)
                    {
                        rules.Add((a) => { return s; });
                        to.Add((new Condition(""), s));
                    }
                    else if (s[1] == '>')
                    {
                        var ret = s.Split(":");
                        rules.Add((a) => { return a.get(s[0]) > Int64.Parse(ret[0].Substring(2)) ? ret[1] : ""; });
                        to.Add((new Condition(ret[0]), ret[1]));
                    }
                    else if (s[1] == '<')
                    {
                        var ret = s.Split(":");
                        rules.Add((a) => { return a.get(s[0]) < Int64.Parse(ret[0].Substring(2)) ? ret[1] : ""; });
                        to.Add((new Condition(ret[0]), ret[1]));
                    }
                    else
                    {
                        rules.Add((a) => { return s; });
                        to.Add((new Condition(""), s));
                    }
                }
            }

            public string apply(Part p)
            {
                foreach (var r in rules)
                {
                    if (r(p) != "") return r(p);
                }
                return "";
            }
        }
    }
}
