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
using GIGACHADINT = System.Numerics.BigInteger;

namespace Solutions.Day06
{
    public class SolverDay06 : ISolver
    {
        public long SolvePart1(string[] lines)
        {            
            long result = 1;

            var times = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();
            var distances = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();

            for (int i = 0; i < times.Count(); i++)
            {
                long wins = 0;
                for (long j = 0; j < times[i]; j++)
                {
                    long speed = j;
                    long travel = (j * (times[i] - j));
                    if (travel > distances[i]) wins++;
                }
                result *= wins;
            }

            //Solve
            return result;
        }

        public long SolvePart2(string[] lines)
        {
            long result = 0;

            var times = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();
            var distances = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(y => Convert.ToInt64(y)).ToList();
            long time = MergeNums(times);
            long dist = MergeNums(distances);

            //could be off by 1, idc
            result = (long)Math.Ceiling(Math.Sqrt(Math.Pow(time, 2) - 4 * dist));

            //Solve
            return result;
        }

        private long MergeNums(List<long> nums) 
        {
            nums.Reverse();
            long result = nums.First();
            long mult = 1;
            for (int i = 1; i < nums.Count; i++)
            {
                while (result > mult) mult *= 10;
                result += nums[i]*mult;
            }
            return result;
        }
    }
}
