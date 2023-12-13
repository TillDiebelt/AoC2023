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
using System.Text.RegularExpressions;
using AOCLib;
using System.Net.Http.Headers;

namespace Solutions.Day05
{
    public class SolverDay05 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //parse
            var lows = lines[0].Split(':')[1].Split(" ",StringSplitOptions.RemoveEmptyEntries).Select(y => Convert.ToInt64(y)).ToList();

            List<(long, long, long)> sTos = new List<(long, long, long)>();
            List<(long, long, long)> sTof = new List<(long, long, long)>();
            List<(long, long, long)> fTow = new List<(long, long, long)>();
            List<(long, long, long)> wTol = new List<(long, long, long)>();
            List<(long, long, long)> lTot = new List<(long, long, long)>();
            List<(long, long, long)> tToh = new List<(long, long, long)>();
            List<(long, long, long)> hTol = new List<(long, long, long)>();
            List<List<(long, long, long)>> maps = new List<List<(long, long, long)>>();
            maps.Add(sTos);
            maps.Add(sTof);
            maps.Add(fTow);
            maps.Add(wTol);
            maps.Add(lTot);
            maps.Add(tToh);
            maps.Add(hTol);

            string s = "";
            foreach(var l in lines) 
            {
                s += l + "\n";
            }
            var splits = s.Split(':');
            for (int i = 2; i < splits.Count(); i++)
            {
                var items = splits[i].Split("\n\n").First().Split("\n",StringSplitOptions.RemoveEmptyEntries);
                foreach(var item in items)
                {
                    try
                    {
                        maps[i - 2].Add((Convert.ToInt64(item.Split(" ")[0]), Convert.ToInt64(item.Split(" ")[1]), Convert.ToInt64(item.Split(" ")[2])));
                    }
                    catch { };
                }
            }

            //Solve
            for (int t = 0; t < maps.Count(); t++)
            {
                for (int j = 0; j < lows.Count(); j++)
                {
                    lows[j] = GetMatch(lows[j], maps[t]);
                }
            }
            return lows.Min();
        }

        private long GetMatch(long search, List<(long, long, long)> b)
        {
            long a = search;
            foreach(var item in b)
            {
                if(item.Item2 + item.Item3-1 >= search && item.Item2 <= search)
                {
                    a = item.Item1 + Math.Abs(search - (item.Item2));
                    return a;
                }
            }
            return search;
        }


        private List<(long,long)> GetMatches((long,long) search, List<(long, long, long)> b)
        {
            List<(long, long)> results = new List<(long, long)>();
            (long, long) todo = search;
            foreach (var item in b)
            {
                if (todo.Item2 <= 0) return results;
                if (RangesOverlap(todo, (item.Item2, item.Item3)))
                {
                    var overLap = RangeOverlap(todo, (item.Item2, item.Item3));
                    if(overLap.Item1 > todo.Item1)
                    {
                        results.Add((todo.Item1, overLap.Item1 - todo.Item1));
                        todo.Item1 = overLap.Item1;
                        todo.Item2 = todo.Item2-(overLap.Item1 - todo.Item1);
                    }
                    if(overLap.Item2 < todo.Item2)
                    {
                        results.Add((todo.Item1+(item.Item1-item.Item2), overLap.Item2));
                        todo.Item1 = overLap.Item1+overLap.Item2;
                        todo.Item2 = todo.Item2 - overLap.Item2;
                    }
                    else
                    {
                        results.Add((todo.Item1 + (item.Item1 - item.Item2), todo.Item2));
                        todo.Item2 = -1;
                    }
                }
            }
            if (todo.Item2 <= 0) return results;
            results.Add(todo);
            return results;
        }

        private bool RangesOverlap((long a, long b) r1, (long a, long b) r2)
        {
            if (r2.a < r1.a)
            {
                var tmp = r1;
                r1 = r2;
                r2 = tmp;
            }
            if ((r2.a >= r1.a) && (r2.a <= r1.a + r1.b - 1)) return true;
            return false;
        }

        private (long, long) RangeOverlap((long a, long b) r1, (long a, long b) r2)
        {
            long overlapStart = Math.Max(r1.a, r2.a);
            long overlapEnd = Math.Min(r1.a + r1.b - 1, r2.a + r2.b - 1);

            return (overlapStart, overlapEnd- overlapStart+1);
        }

        public long SolvePart2(string[] lines, string text)
        {
            //parse
            var seeds = lines[0].Split(':')[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(y => Convert.ToInt64(y)).ToList();
            var lows = new List<(long,long)>();
            for(int i = 0; i < seeds.Count; i+=2)
            {
                lows.Add((seeds[i], seeds[i+1]));
            }

            List<(long, long, long)> sTos = new List<(long, long, long)>();
            List<(long, long, long)> sTof = new List<(long, long, long)>();
            List<(long, long, long)> fTow = new List<(long, long, long)>();
            List<(long, long, long)> wTol = new List<(long, long, long)>();
            List<(long, long, long)> lTot = new List<(long, long, long)>();
            List<(long, long, long)> tToh = new List<(long, long, long)>();
            List<(long, long, long)> hTol = new List<(long, long, long)>();
            List<List<(long, long, long)>> maps = new List<List<(long, long, long)>>();
            maps.Add(sTos);
            maps.Add(sTof);
            maps.Add(fTow);
            maps.Add(wTol);
            maps.Add(lTot);
            maps.Add(tToh);
            maps.Add(hTol);

            string s = "";
            foreach (var l in lines)
            {
                s += l + "\n";
            }
            var splits = s.Split(':');
            for (int i = 2; i < splits.Count(); i++)
            {
                var items = splits[i].Split("\n\n").First().Split("\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in items)
                {
                    try
                    {
                        maps[i - 2].Add((Convert.ToInt64(item.Split(" ")[0]), Convert.ToInt64(item.Split(" ")[1]), Convert.ToInt64(item.Split(" ")[2])));
                    }
                    catch { };
                }
            }

            //Solve
            for (int t = 0; t < maps.Count(); t++)
            {
                maps[t] = maps[t].OrderBy(x => x.Item2).ToList();
                var low = new List<(long, long)>();
                for (int j = 0; j < lows.Count(); j++)
                {
                    low.AddRange(GetMatches(lows[j], maps[t]));
                }
                lows = low;
            }

            return lows.Select(x => x.Item1).Min();
        }
    }
}
