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
            foreach (var p in parts)
            {
                string cur = "in";
                while (cur != "A" && cur != "R")
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
                switch (id)
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
                foreach (var s in splits)
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
                foreach (var r in rules)
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
                foreach (var t in r.to)
                {
                    goesTo.Add((t.Item2));
                }
                graph.Add((r.id, goesTo));
            }

            List<Condition> valids = new List<Condition>();
            valids = Search2("in", new Condition(""), graph, rules);

            //Solve
            long result = 0;
            List<Condition> seen = new List<Condition>();
            foreach (var v in valids)
            {
                List<Condition> merged = new List<Condition>();
                foreach (var c in seen)
                {
                    var foo = v.merge(c);
                    if (foo.combs() > 0)
                    {
                        foo.mul = -(c.mul * v.mul);
                        merged.Add(foo);
                    }
                }
                seen.Add(v);
                seen.AddRange(merged);
            }

            foreach (var v in seen)
            {
                result += v.combs() * v.mul;
            }
            return result;
        }

        private List<Condition> Search2(string key, Condition current, List<(string id, List<string> paths)> graph, List<AbstractRule> rules)
        {
            List<Condition> valids = new List<Condition>();

            Condition addMerge = current;
            foreach (var path in rules.Where(x => x.id == key).First().to)
            {
                Condition newC = addMerge.merge(path.Item1);
                if (newC.combs() == 0) continue;

                string nextKey = path.Item2;
                if (nextKey == "A")
                {
                    valids.Add(newC);
                    addMerge = addMerge.inv_merge(newC);
                    continue;
                }
                if (nextKey == "R")
                {
                    addMerge = addMerge.inv_merge(newC);
                    continue;
                }
                valids.AddRange(Search2(
                        nextKey,
                        newC,
                        graph,
                        rules
                    )
                );
                addMerge = addMerge.inv_merge(newC);
            }
            return valids;
        }

        public class Condition
        {
            public string c;

            public long minx = 0;
            public long maxx = 4001;
            public long mina = 0;
            public long maxa = 4001;
            public long minm = 0;
            public long maxm = 4001;
            public long mins = 0;
            public long maxs = 4001;

            public long mul = 1;

            public long combs()
            {
                return Math.Max(0, maxx - minx-1)
                    * Math.Max(0, maxa - mina-1)
                    * Math.Max(0, maxs - mins-1)
                    * Math.Max(0, maxm - minm-1);
            }

            public Condition(string parse)
            {
                c = parse;
                if (parse.Length > 0)
                {
                    if (parse.Split("<").Length == 0 || parse.Split(">").Length == 0)
                        return;
                    switch (parse[0])
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

            public Condition inv_merge(Condition a)
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

                int offset = 1;
                if (this.maxa == a.maxa);
                else
                {
                    newC.mina = Math.Min(a.maxa, this.maxa) - offset;
                    newC.maxa = Math.Max(a.maxa, this.maxa);
                }
                if (this.mina == a.mina);
                else
                {
                    newC.mina = Math.Min(a.mina, this.mina);
                    newC.maxa = Math.Max(a.mina, this.mina) + offset;
                }

                if (this.maxx == a.maxx) ;
                else
                {
                    newC.minx = Math.Min(a.maxx, this.maxx) - offset;
                    newC.maxx = Math.Max(a.maxx, this.maxx);
                }
                if (this.minx == a.minx) ;
                else
                {
                    newC.minx = Math.Min(a.minx, this.minx);
                    newC.maxx = Math.Max(a.minx, this.minx) + offset;
                }

                if (this.maxs == a.maxs);
                else
                {
                    newC.mins = Math.Min(a.maxs, this.maxs) - offset;
                    newC.maxs = Math.Max(a.maxs, this.maxs);
                }
                if (this.mins == a.mins) ;
                else
                {
                    newC.mins = Math.Min(a.mins, this.mins);
                    newC.maxs = Math.Max(a.mins, this.mins) + offset;
                }

                if (this.maxm == a.maxm) ;
                else
                {
                    newC.minm = Math.Min(a.maxm, this.maxm) - offset;
                    newC.maxm = Math.Max(a.maxm, this.maxm);
                }
                if (this.minm == a.minm) ;
                else
                {
                    newC.minm = Math.Min(a.minm, this.minm);
                    newC.maxm = Math.Max(a.minm, this.minm) + offset;
                }
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
