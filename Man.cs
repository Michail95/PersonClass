using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D1
{
   public class Man
    {
        public string name { get; set; }
        public string pol { get; set; }
        public int tail { get; set; }
        public int mas { get; set; }
        public string birth { get; set; }

        public Man(string _n, string _p, int _t, int _m, string _b)
        {
            name = _n;
            pol = _p;
            tail = _t;
            mas = _m;
            birth = _b;
        }
    }
}
