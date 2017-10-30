using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coins_def
{
    class CityMap<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 item, T2 item2)
        {
            Add(new Tuple<T1, T2>(item, item2));
        }
    }
}