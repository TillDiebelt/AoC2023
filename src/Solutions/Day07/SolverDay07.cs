﻿using System;
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
using System.Xml;

namespace Solutions.Day07
{
    public class SolverDay07 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            long result = lines
                .Select(l => (
                    Int64.Parse(l.Split(" ").Last()), 
                    l.Split(" ").First().GroupBy(x => x).ToDictionary(z => z.Key, y => y.Count()),
                    l.Split(" ").First()))
                .Select(x => (
                    x.Item1, 
                    2*(5-x.Item2.Count())- x.Item2.Where(x => x.Value == 2).Count(), 
                    x.Item3.Select((t, index) => (char.IsDigit(t) ? t.ToDigit() : (((int)t) % 2 == 0 ? (100 - ((int)t)) : (200 - ((int)t)))) * (Math.Pow(1000, 5 - index))).Sum()))
                .Select(x => (
                    x.Item1,
                    x.Item2 * Math.Pow(1000, 6) + x.Item3))
                .OrderBy(x => x.Item2)
                .Select((n, index) => n.Item1*(index+1))
                .Sum();

            return result;
        }
        
        //for part 1
        private class Hand : IComparable<Hand>
        {
            public string hand;
            public long value;
            public int type;
            public List<int> deciders;
            public Hand(string hand, long value)
            {
                this.hand = hand;
                this.value = value;
                this.deciders = new List<int>();

                //just for the fun of it here a messed up parsing. look at JHand to see how it was originally done
                Dictionary<char, int> uniques = new Dictionary<char, int>();
                for (int i = 0; i < hand.Length; i++)
                {
                    //magic conversion
                    if (!char.IsDigit(hand[i]))
                        if (((int)hand[i]) % 2 == 0) deciders.Add(100 - ((int)hand[i]));
                        else deciders.Add(200-((int)hand[i]));
                    else
                        deciders.Add(hand[i].ToDigit());

                    if (!uniques.TryAdd(hand[i], 1)) uniques[hand[i]]++;
                }

                type = (2 * (5 - uniques.Count())) - uniques.Where(x => x.Value == 2).Count(); //magic math
            }

            public int CompareTo(Hand? other)
            {
                if (this.type > other.type) return -1;
                if (other.type > this.type) return 1;
                for (int i = 0; i < this.hand.Length; i++)
                {
                    if (this.deciders[i] > other.deciders[i]) return -1;
                    if (other.deciders[i] > this.deciders[i]) return 1;
                }
                return 0;
            }
        }

        public long SolvePart2(string[] lines, string text)
        {
            long result = 0;
            List<JHand> hands = new List<JHand>();

            for (int i = 0; i < lines.Length; i++)
            {
                hands.Add(new JHand(lines[i].Split(" ").First(), Int64.Parse(lines[i].Split(" ").Last())));
            }

            hands.Sort();
            hands.Reverse();

            for (int i = 0; i < hands.Count; i++)
            {
                result += hands[i].value * (i + 1);
            }

            //Solve
            return result;
        }
        private class JHand : IComparable<JHand>
        {
            public string hand;
            public long value;
            public int type;
            public int js;
            public List<int> handvalues;
            public JHand(string hand, long value)
            {
                this.hand = hand;
                this.value = value;
                this.handvalues = new List<int>();
                js = 0;

                Dictionary<char, int> uniques = new Dictionary<char, int>();
                for (int i = 0; i < hand.Length; i++)
                {
                    if (hand[i] == 'A')
                        handvalues.Add(99);
                    else if (hand[i] == 'K')
                        handvalues.Add(98);
                    else if (hand[i] == 'Q')
                        handvalues.Add(97);
                    else if (hand[i] == 'J')
                    {
                        handvalues.Add(1);
                        js++;
                    }
                    else if (hand[i] == 'T')
                        handvalues.Add(95);
                    else
                        handvalues.Add(hand[i].ToDigit());

                    if (!uniques.TryAdd(hand[i], 1)) uniques[hand[i]]++;
                }
                if (uniques.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 5))
                {
                    type = 9;
                }
                else if (uniques.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 4))
                {
                    type = 8;
                }
                else if ((uniques.Any(x => x.Value == 3) && uniques.Any(x => x.Value == 2))
                         || (uniques.Where(x => x.Value == 2).Count() == 2 && js == 1))
                {
                    type = 7;
                }
                else if (uniques.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 3))
                {
                    type = 6;
                }
                else if (uniques.Where(x => x.Value == 2).Count() == 2 || (uniques.Any(x => x.Value == 2) && js == 1))
                {
                    type = 5;
                }
                else if (uniques.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 2))
                {
                    type = 4;
                }
                else
                {
                    type = 3;
                }
            }

            public int CompareTo(JHand? other)
            {
                if (this.type > other.type) return -1;
                if (other.type > this.type) return 1;
                for (int i = 0; i < this.hand.Length; i++)
                {
                    if (this.handvalues[i] > other.handvalues[i]) return -1;
                    if (other.handvalues[i] > this.handvalues[i]) return 1;
                }
                return 0;
            }
        }

    }
}
