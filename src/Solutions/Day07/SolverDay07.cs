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

namespace Solutions.Day07
{
    public class SolverDay07 : ISolver
    {
        public long SolvePart1(string[] lines)
        {
            long result = 0;
            List<Hand> hands = new List<Hand>();

            for (int i = 0; i < lines.Length; i++)
            {
                hands.Add(new Hand(lines[i].Split(" ").First(), Int64.Parse(lines[i].Split(" ").Last())));
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
        
        private class Hand : IComparable<Hand>
        {
            public string hand;
            public long value;
            public int type;
            public List<int> handvalues;
            public Hand(string hand, long value)
            {
                this.hand = hand;
                this.value = value;
                this.handvalues = new List<int>();

                Dictionary<char, int> chars = new Dictionary<char, int>();
                for (int i = 0; i < hand.Length; i++)
                {
                    //K, Q, J, T,
                    if (hand[i] == 'A')
                        handvalues.Add(99);
                    else if (hand[i] == 'K')
                        handvalues.Add(98);
                    else if (hand[i] == 'Q')
                        handvalues.Add(97);
                    else if (hand[i] == 'J')
                        handvalues.Add(96);
                    else if (hand[i] == 'T')
                        handvalues.Add(95);
                    else
                        handvalues.Add(hand[i].ToDigit());


                    if (chars.ContainsKey(hand[i]))
                    {
                        chars[hand[i]]++;
                    }
                    else
                        chars.Add(hand[i], 1);
                }
                if (chars.Any(x => x.Value == 5))
                {
                    type = 9;
                }
                else if (chars.Any(x => x.Value == 4))
                {
                    type = 8;
                }
                else if (chars.Any(x => x.Value == 3) && chars.Any(x => x.Value == 2))
                {
                    type = 7;
                }
                else if (chars.Any(x => x.Value == 3))
                {
                    type = 6;
                }
                else if (chars.Where(x => x.Value == 2).Count() == 2)
                {
                    type = 5;
                }
                else if (chars.Any(x => x.Value == 2))
                {
                    type = 4;
                }
                else
                {
                    type = 3;
                }
            }

            public int CompareTo(Hand? other)
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

        public long SolvePart2(string[] lines)
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

                Dictionary<char, int> chars = new Dictionary<char, int>();
                for (int i = 0; i < hand.Length; i++)
                {
                    //K, Q, J, T,
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


                    if (chars.ContainsKey(hand[i]))
                    {
                        chars[hand[i]]++;
                    }
                    else
                        chars.Add(hand[i], 1);
                }
                if (chars.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 5))
                {
                    type = 9;
                }
                else if (chars.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 4))
                {
                    type = 8;
                }
                else if ((chars.Any(x => x.Value == 3) && chars.Any(x => x.Value == 2))
                         || (chars.Where(x => x.Value == 2).Count() == 2 && js == 1))
                {
                    type = 7;
                }
                else if (chars.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 3))
                {
                    type = 6;
                }
                else if (chars.Where(x => x.Value == 2).Count() == 2 || (chars.Any(x => x.Value == 2) && js == 1))
                {
                    type = 5;
                }
                else if (chars.Any(x => x.Value + (x.Key == 'J' ? 0 : js) == 2))
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
