using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coins_def
{
    public class Country
    {
        public Country(string _name, int _balance)
        {
            this.balance = _balance;
            this.name = _name;
        }
        public int balance { get; set; }
        public string name { get; set; }
    }
}