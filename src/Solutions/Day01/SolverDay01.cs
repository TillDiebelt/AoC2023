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

namespace Solutions.Day01
{
    public class SolverDay01 : ISolver
    {
        public long SolvePart1(string[] lines)
        {
            //Solve
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                int num1 = 0;
                int num2 = 0;
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (char.IsDigit(lines[i][j]))
                    {
                        num1 = lines[i][j].ToDigit();
                        break;
                    }
                }
                for (int j = lines[i].Length-1; j >= 0; j--)
                {
                    if (char.IsDigit(lines[i][j]))
                    {
                        num2 = lines[i][j].ToDigit();
                        break;
                    }
                }
                result += Convert.ToInt64(num1 + "" + num2);
            }
            return result;
        }

        public long SolvePart2(string[] lines)
        {
            //Solve
            long result = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i].Substring(j).StartsWith("one"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "1" + lines[i].Substring(j+2);
                    }
                    if (lines[i].Substring(j).StartsWith("two"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "2" + lines[i].Substring(j + 2);
                    }
                    if (lines[i].Substring(j).StartsWith("three"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "3" + lines[i].Substring(j + 4);
                    }
                    if (lines[i].Substring(j).StartsWith("four"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "4" + lines[i].Substring(j + 3);
                    }
                    if (lines[i].Substring(j).StartsWith("five"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "5" + lines[i].Substring(j + 3);
                    }
                    if (lines[i].Substring(j).StartsWith("six"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "6" + lines[i].Substring(j + 2);
                    }
                    if (lines[i].Substring(j).StartsWith("seven"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "7" + lines[i].Substring(j + 4);
                    }
                    if (lines[i].Substring(j).StartsWith("eight"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "8" + lines[i].Substring(j + 4);
                    }
                    if (lines[i].Substring(j).StartsWith("nine"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "9" + lines[i].Substring(j + 3);
                    }
                    if (lines[i].Substring(j).StartsWith("zero"))
                    {
                        lines[i] = lines[i].Substring(0, j) + "0" + lines[i].Substring(j + 3);
                    }
                }
                int num1 = 0;
                int num2 = 0;
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (char.IsDigit(lines[i][j]))
                    {
                        num1 = lines[i][j].ToDigit();                        
                        break;
                    }          
                }
                for (int j = lines[i].Length - 1; j >= 0; j--)
                {
                    if (char.IsDigit(lines[i][j]))
                    {
                        num2 = lines[i][j].ToDigit();
                        break;
                    }
                }
                result += Convert.ToInt64(num1 + "" + num2);
            }
            return result;
        }
    }
}
