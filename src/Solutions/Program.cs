using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Solutions;
using Solutions.Day00;
using Solutions.Day01;
using Solutions.Day02;
using Solutions.Day03;
using Solutions.Day04;
using Solutions.Day05;
using Solutions.Day09;
using Solutions.Day10;
using Solutions.Day11;
using Solutions.Day12;
using Solutions.Day13;
using Solutions.Day14;

namespace Solutions
{
    public class Benchmark
    {
        public string[] lines;
        public int Day = 0;
        public ISolver Solver = new SolverDay00();

        [GlobalSetup]
        public void GlobalSetup()
        {
            lines = File.ReadAllLines(@"../../../../../../../input/inputDay"+Day);
        }

        [Benchmark]
        public void BenchmarkPart1()
        {
            Solver.SolvePart1(lines);
        }
        
        [Benchmark]
        public void BenchmarkPart2()
        {
            Solver.SolvePart2(lines);
        }
    }
    
    class Program
    {
        public static int day = 13;
        public static ISolver solver = new SolverDay13();
        public static string inputPath = "../../../input/inputDay"+day;
        public static string inputPathTest = "../../../input/inputTest";
        
        static void Main(string[] args)
        {
            string input = File.ReadAllText(inputPath).Replace("\r", "");
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            Console.WriteLine("AOC 2023 Day " + day);
            Console.WriteLine("Solution Part 1:");
            Console.WriteLine(solver.SolvePart1(lines, input));
            Console.WriteLine("Solution Part 2:");
            Console.WriteLine(solver.SolvePart2(lines, input));

            
            input = File.ReadAllText(inputPathTest).Replace("\r", "");
            lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);            
            Console.WriteLine("\nTests:");
            var test1 = solver.SolvePart1(lines, input);
            Console.WriteLine(test1);
            if (test1 == 8) Console.WriteLine("test 1 successful"); else Console.WriteLine("test 1 failed");
            var test2 = solver.SolvePart2(lines, input);
            Console.WriteLine(test2);
            if (test2 == 2286) Console.WriteLine("test 2 successful"); else Console.WriteLine("test 2 failed");
            
            //Benchmark
            //Console.WriteLine("\nBenchmark:");
            //BenchmarkRunner.Run<Benchmark>();

            Console.WriteLine("\nPress enter to leave:");
            Console.ReadLine();
        }

    }
}
