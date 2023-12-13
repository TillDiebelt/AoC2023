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
using System.Runtime.InteropServices;

namespace Solutions.Day02
{
    public class SolverDay02 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var split = lines[i].Split(':')[1].Split(';');
                int red = 0;
                int blue = 0;
                int green = 0;
                foreach(var s in split)
                {
                    var items = s.Split(',');
                    foreach (var item in items)
                    {
                        var tupel = item.Trim().Split(' ');
                        if (tupel[1] == "blue")
                            blue = Math.Max(blue, Convert.ToInt32(tupel[0]));
                        if (tupel[1] == "red")
                            red = Math.Max(red, Convert.ToInt32(tupel[0]));
                        if (tupel[1] == "green")
                            green = Math.Max(green, Convert.ToInt32(tupel[0]));
                    }
                }
                if (green <= 13 && red <= 12 && blue <= 14)
                {
                    result += Convert.ToInt32(lines[i].Split(':')[0].Split(' ')[1]);
                }
            }

            //Solve
            return result; 
        }

        public long SolvePart2(string[] lines, string text)
        {
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var split = lines[i].Split(':')[1].Split(';');
                int red = 0;
                int blue = 0;
                int green = 0;
                foreach (var s in split)
                {
                    var items = s.Split(',');
                    foreach (var item in items)
                    {
                        var tupel = item.Trim().Split(' ');
                        if (tupel[1] == "blue")
                            blue = Math.Max(blue, Convert.ToInt32(tupel[0]));
                        if (tupel[1] == "red")
                            red = Math.Max(red, Convert.ToInt32(tupel[0]));
                        if (tupel[1] == "green")
                            green = Math.Max(green, Convert.ToInt32(tupel[0]));
                    }
                }
                result += red * green * blue;
            }
            //Solve
            return result;
        }
    }
}
