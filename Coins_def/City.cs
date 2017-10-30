using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coins_def
{
    class City
    {
        public City(string name, int xl, int  yl, int xh, int yh)
        {
            this.name = name;
            this.xl = xl;
            this.yl = yl;
            this.xh = xh;
            this.yh = yh;
        }
        public string name { get; set; }
        public int xl { get; set; }
        public int yl { get; set; }
        public int xh { get; set; }
        public int yh { get; set; }
        public int testcase { get; set; }
    }
}