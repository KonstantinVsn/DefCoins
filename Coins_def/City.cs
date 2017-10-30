using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coins_def
{
    public class City
    {
        public int balance { get; set; }
        public string country_name { get; set; }
        public bool check { get; set; }

        public List<Coin> coins = new List<Coin>();
    }
}