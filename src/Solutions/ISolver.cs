using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TillSharp.Extenders.Collections;

namespace Solutions
{
    public interface ISolver
    {
        public long SolvePart1(string[] lines);
        public long SolvePart2(string[] lines);
    }
}
