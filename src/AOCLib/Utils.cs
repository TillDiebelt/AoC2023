using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCLib
{
    public static class Utils
    {
        public static long GaussSum(long end, long start = 0)
        {
            return (end * (end + 1)) / 2 - (start * (start + 1)) / 2;
        }
    }
}
