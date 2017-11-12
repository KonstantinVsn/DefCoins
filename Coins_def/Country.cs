using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coins_def
{
    public class Country
    {
        public Country(string name, int xl, int  yl, int xh, int yh)
        {
            this.name = name;
            this.xl = xl;
            this.yl = yl;
            this.xh = xh;
            this.yh = yh;
        }
        public Country() { }
        public Country(string name)
        {
            this.name = name;
        }
        public string name { get; set; }
        public int xl { get; set; }
        public int yl { get; set; }
        public int xh { get; set; }
        public int yh { get; set; }
        //public int countryGroup { get; set; }
        public bool resolved { get; set; }
        public int days { get; set; }
        public int citycount { get; set; }
    }
}