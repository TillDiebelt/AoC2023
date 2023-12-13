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
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Disassemblers;
using System.Drawing;

namespace Solutions.Day10
{
    public class SolverDay10 : ISolver
    {
        //up = 1; right = 2; down = 3; //left = 4
        public const int UP = 1;
        public const int RIGHT = 2;
        public const int DOWN = 3;
        public const int LEFT = 4;
        
        public (int, int) VERT = (UP, DOWN);
        public (int, int) HOR = (RIGHT, LEFT);
        public (int, int) UR = (UP, RIGHT);
        public (int, int) UL = (UP, LEFT);
        public (int, int) DL = (DOWN, LEFT);
        public (int, int) DR = (DOWN, RIGHT);
        public (int, int) START = (0,0);
        public (int, int) NONE = (-1,-1);


        public long SolvePart1(string[] lines, string text)
        {
            (int,int)[,] pipes = new (int, int)[lines.Length, lines[0].Length];
            (int, int) start = (0,0);
            
            for(int i = 0; i < lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    (int, int) p = (i, j);
                    switch (lines[i][j])
                    {
                        case '.':
                            pipes[p.Item1, p.Item2] = NONE;
                            break;
                        case '-':
                            pipes[p.Item1, p.Item2] = HOR;
                            break;
                        case '|':
                            pipes[p.Item1, p.Item2] = VERT;
                            break;
                        case 'L':
                            pipes[p.Item1, p.Item2] = UR;
                            break;
                        case 'J':
                            pipes[p.Item1, p.Item2] = UL;
                            break;
                        case '7':
                            pipes[p.Item1, p.Item2] = DL;
                            break;
                        case 'F':
                            pipes[p.Item1, p.Item2] = DR;
                            break;
                        case 'S':
                            pipes[p.Item1, p.Item2] = START;
                            start = (j,i);
                            break;
                    }
                }
            }

            //see.Add(start);
            bool running = true;
            var ini = WalkStep(pipes, start, 0, start);
            (int, int) prev = start;
            while (running)
            {
                var step = WalkStep(pipes, ini.next, ini.i, prev);
                if (ini.next == start)
                    break;
                prev = ini.next;
                ini.next = step.next;
                ini.i = step.i;
            }
            long result = ini.i;
            
            return (result) / 2;
        }
        
                
        private ((int, int) next, int i) WalkStep((int, int)[,] m, (int, int) cur, int i, (int, int) prev)
        {
            var neigh = Neighbour(m, cur, prev);

            return (neigh, i+1);
        }
        
        private (int, int) Neighbour((int, int)[,] map, (int, int) current, (int, int) prev)
        {
            int x = current.Item1;
            int y = current.Item2;
            if (x - 1 >= 0 && x - 1 != prev.Item1)
            {
                if (IsValidNeighbor(map, current, (x - 1, y)))
                    return (x - 1, y);
            }
            if (x + 1 <= map.GetUpperBound(1) && x + 1 != prev.Item1)
            {
                if (IsValidNeighbor(map, current, (x + 1, y)))
                    return (x + 1, y);
            }
            if (y - 1 >= 0 && y - 1 != prev.Item2)
                if (IsValidNeighbor(map, current, (x, y - 1)))
                    return (x, y - 1);
            if (y + 1 <= map.GetUpperBound(0) && y + 1 != prev.Item2)
                if (IsValidNeighbor(map, current, (x, y + 1)))
                    return (x, y + 1);
            throw new Exception();
        }

        private bool IsValidNeighbor((int, int)[,] map, (int, int) current, (int, int) neighbor)
        {
            (int, int) currentP = map[current.Item2, current.Item1];
            int x = current.Item1;
            int y = current.Item2;
            (int, int) neighborP = map[neighbor.Item2, neighbor.Item1];
            int nx = neighbor.Item1;
            int ny = neighbor.Item2;
            if (neighborP == NONE) return false;
            if (nx == x - 1) //left
            {
                if ((currentP.Item1 == LEFT || currentP.Item2 == LEFT || currentP == START) && (neighborP.Item1 == RIGHT || neighborP.Item2 == RIGHT || neighborP == START))
                    return true;
            }
            if (nx == x + 1) //right
            {
                if ((currentP.Item1 == RIGHT || currentP.Item2 == RIGHT || currentP == START) && (neighborP.Item1 == LEFT || neighborP.Item2 == LEFT || neighborP == START))
                    return true;
            }
            if (ny == y - 1) //up
            {
                if ((currentP.Item1 == UP || currentP.Item2 == UP || currentP == START) && (neighborP.Item1 == DOWN || neighborP.Item2 == DOWN || neighborP == START))
                    return true;
            }
            if (ny == y + 1) //down
            {
                if ((currentP.Item1 == DOWN || currentP.Item2 == 3 || currentP == START) && (neighborP.Item1 == UP || neighborP.Item2 == UP || neighborP == START))
                    return true;
            }
            return false;
        }

        public long SolvePart2(string[] lines, string text)
        {

            (int, int)[,] pipesP1 = new (int, int)[lines.Length, lines[0].Length];
            (int, int) start = (0, 0);

            for (int i = 0; i < lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    (int, int) p = (i, j);
                    switch (lines[i][j])
                    {
                        case '.':
                            pipesP1[p.Item1, p.Item2] = NONE;
                            break;
                        case '-':
                            pipesP1[p.Item1, p.Item2] = HOR;
                            break;
                        case '|':
                            pipesP1[p.Item1, p.Item2] = VERT;
                            break;
                        case 'L':
                            pipesP1[p.Item1, p.Item2] = UR;
                            break;
                        case 'J':
                            pipesP1[p.Item1, p.Item2] = UL;
                            break;
                        case '7':
                            pipesP1[p.Item1, p.Item2] = DL;
                            break;
                        case 'F':
                            pipesP1[p.Item1, p.Item2] = DR;
                            break;
                        case 'S':
                            pipesP1[p.Item1, p.Item2] = START;
                            start = (j, i);
                            break;
                    }
                }
            }

            bool running = true;
            var ini = WalkStep(pipesP1, start, 0, start);
            List<(int, int)> path = new List<(int, int)>();
            (int, int) prev = start;
            (int, int) firstPipe = ini.next;
            (int, int) lastPipe = start;
            while (running)
            {
                path.Add(prev);
                var step = WalkStep(pipesP1, ini.next, ini.i, prev);
                if (ini.next == start)
                {
                    lastPipe = prev;
                    break;
                }
                prev = ini.next;
                ini.next = step.next;
                ini.i = step.i;
            }

            int startType = 0;
            int move1X = start.Item1 - firstPipe.Item1;
            int move1Y = start.Item2 - firstPipe.Item2;
            
            int move2X = lastPipe.Item1 - start.Item1;
            int move2Y = lastPipe.Item2 - start.Item2;

            if (move1X == -1) //right start
            {
                if (move2X == -1) //upper end
                    startType = 2;
                if (move2Y == -1) //lower end
                    startType = 6;
                if (move2Y == 1)
                    startType = 3;
            }
            if (move1X == 1) //left start
            {
                if (move2X == 1) //upper end
                    startType = 2;
                if (move2Y == -1) //lower end
                    startType = 4;
                if (move2Y == 1)
                    startType = 5;
            }
            if (move1Y == -1) //lower start
            {
                if (move2Y == -1) //upper end
                    startType = 1;
                if (move2X == -1) //lower end
                    startType = 5;
                if (move2X == 1)
                    startType = 6;
            }
            if (move1Y == 1) //lower start
            {
                if (move2Y == 1) //upper end
                    startType = 1;
                if (move2X == -1) //lower end
                    startType = 3;
                if (move2X == 1)
                    startType = 4;
            }

            int[,] ints = new int[lines.Length, lines[0].Length];
            foreach(var p in path)
            {
                ints[p.Item2, p.Item1] = 1;
            }
                
            (int, int)[,] pipes = new (int, int)[lines.Length*3, lines[0].Length*3];

            long result = 0;

            List<(int, int)[,]> foos = new();

            //0
            foos.Add(new (int, int)[3, 3] { { NONE, NONE, NONE},
                                            { NONE, NONE, NONE},
                                            { NONE, NONE, NONE}});

            //1 VERT
            foos.Add(new (int, int)[3, 3] { { NONE, VERT, NONE},
                                            { NONE, VERT, NONE},
                                            { NONE, VERT, NONE}});

            //2 HOR
            foos.Add(new (int, int)[3, 3] { { NONE, NONE, NONE},
                                            { HOR, HOR, HOR},
                                            { NONE, NONE, NONE}});

            //3 UR
            foos.Add(new (int, int)[3, 3] { { NONE, VERT, NONE},
                                            { NONE, UR, HOR},
                                            { NONE, NONE, NONE}});

            //4 UL
            foos.Add(new (int, int)[3, 3] { { NONE, VERT, NONE},
                                            { HOR, UL, NONE},
                                            { NONE, NONE, NONE}});

            //5 DL
            foos.Add(new (int, int)[3, 3] { { NONE, NONE, NONE},
                                            { HOR, DL, NONE},
                                            { NONE, VERT, NONE}});

            //6 DR
            foos.Add(new (int, int)[3, 3] { { NONE, NONE, NONE},
                                            { NONE, DR, HOR},
                                            { NONE, VERT, NONE}});            
            
            for (int i = 0; i < lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    (int X, int Y) p = (j * 3, i * 3);

                    if (ints[i,j] != 1)
                    {
                        for (int n = 0; n < 3; n++)
                            for (int m = 0; m < 3; m++)
                                pipes[p.Y + n, p.X + m] = foos[0][n, m];
                    }
                    else
                        switch (lines[i][j])
                        {
                            case '.':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[0][n, m];
                                break;
                            case '-':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[2][n, m];
                                break;
                            case '|':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[1][n, m];
                                break;
                            case 'L':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[3][n, m];
                                break;
                            case 'J':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[4][n, m];
                                break;
                            case '7':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[5][n, m];
                                break;
                            case 'F':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[6][n, m];
                                break;
                            case 'S':
                                for (int n = 0; n < 3; n++)
                                    for (int m = 0; m < 3; m++)
                                        pipes[p.Y + n, p.X + m] = foos[startType][n, m];
                                break;
                        }
                }
            }


            pipes = FloodFill(pipes, (0, 0), NONE, (9, 9));
            
            for (int i = 0; i <= pipes.GetUpperBound(0); i += 3)
            {
                for (int j = 0; j <= pipes.GetUpperBound(1); j += 3)
                {
                    if (pipes[i + 1, j + 1] == NONE) result++;
                }
            }
            return (result);
        }
        
        private (int,int)[,] FloodFill((int, int)[,] map, (int,int) start, (int,int) col, (int,int) paint)
        {
            Stack<(int, int)> pixels = new Stack<(int, int)>();
            pixels.Push(start);
            (int, int)[,] result = map;
            
            while (pixels.Count > 0)
            {
                (int X, int Y) a = pixels.Pop();
                if (a.X <= map.GetUpperBound(1) && a.X >= 0 &&
                        a.Y <= map.GetUpperBound(0) && a.Y >= 0)
                {
                    
                    if (result[a.Y, a.X] == col)
                    {
                        result[a.Y, a.X] = paint;
                        pixels.Push((a.X - 1, a.Y));
                        pixels.Push((a.X + 1, a.Y));
                        pixels.Push((a.X, a.Y - 1));
                        pixels.Push((a.X, a.Y + 1));
                    }
                }
            }
            return result;
        }
    }
}
