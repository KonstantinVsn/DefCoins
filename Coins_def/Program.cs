using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coins_def
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"D:\\5 курс\\Coins_def\\Coins_def\\input.txt";

            string[] lines = System.IO.File.ReadAllLines(path);
            Definer definer = new Definer(lines);
        }
    }
}
