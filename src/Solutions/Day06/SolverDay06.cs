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

namespace Solutions.Day06
{
    public class SolverDay06 : ISolver
    {
        public long SolvePart1(string[] lines)
        {
            //Parse
            // 0,3 -> 1,4
            // 2,1 -> 4,3
            //var rocklines = lines.Select(x => x.Split("->").ToList().Select(y => (Int32.Parse(y.Trim().Split(',')[0]), Int32.Parse(y.Trim().Split(',')[1]))).ToList());

            //1,3,5,1,2,455,6
            //var longs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

            //1
            //3
            //var sum = lines.Select(y => Convert.ToInt64(y)).Map(x => x).Reduce((x, y) => x + y);

            long result = 1;
            var array = new int[lines.Length, lines[0].Length];

            var times = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();
            var distances = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();

            for (int i = 0; i < times.Count(); i++)
            {
                long tmp = 0;
                for (long j = 0; j < times[i]; j++)
                {
                    long speed = j;
                    long travel = (j * (times[i] - j));
                    if (travel > distances[i]) tmp++;
                }
                result *= tmp;
            }

            //Solve
            return result;
        }

        public long SolvePart2(string[] lines)
        {
            //Parse
            // 0,3 -> 1,4
            // 2,1 -> 4,3
            //var rocklines = lines.Select(x => x.Split("->").ToList().Select(y => (Int32.Parse(y.Trim().Split(',')[0]), Int32.Parse(y.Trim().Split(',')[1]))).ToList());

            //1,3,5,1,2,455,6
            //var longs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

            //1
            //3
            //var sum = lines.Select(y => Convert.ToInt64(y)).Map(x => x).Reduce((x, y) => x + y);

            long result = 0;
            var array = new int[lines.Length, lines[0].Length];

            var times = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();
            var distances = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();
            long time = long.Parse(times.Aggregate("", (string x, long y) => x + "" + y));
            long dist = long.Parse(distances.Aggregate("", (string x, long y) => x + "" + y));

            for (long j = 0; j < time; j++)
            {
                long speed = j;
                long travel = (j * (time - j));
                if (travel > dist) result++;
            }

            //Solve
            return result;
        }
    }
}
