using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coins_def
{
    public class BalanceCity
    {
        public int balance { get; set; }
        public string country_name { get; set; }
        public List<Country> countries = new List<Country>();
    }
}