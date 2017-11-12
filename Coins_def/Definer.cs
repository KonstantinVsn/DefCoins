using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Coins_def
{
    class Definer
    {
        public List<Country> countries = new List<Country>();
        public List<Country> resolvedCountries = new List<Country>();
        public CountryGroupe<int, List<Country>> CountryGroups = new CountryGroupe<int, List<Country>>();
        int group = 0;
        Map map = new Map();
        int mapsize = 10;

        int PORTION = 1000;
        int COINS_COUNT = 1000000;
        public Definer(string[] inputData)
        {
            InitMap();
            foreach (var line in inputData)
            {
                var parsedCity = line.Split(' ');

                if (parsedCity.Length > 1)
                {
                    var country = new Country(parsedCity[0],
                                              xl: Int32.Parse(parsedCity[1]),
                                              yl: mapsize - 1 - Int32.Parse(parsedCity[2]),
                                              xh: Int32.Parse(parsedCity[3]), yh: mapsize - 1 - Int32.Parse(parsedCity[4]));
                    countries.Add(country);
                }
                else
                {
                    if (group != 0)
                        CountryGroups.Add(group, countries);
                    group++;
                    countries = new List<Country>();
                }
            }
            CountryGroups.Add(group, countries);
            List<Country> countryToCheck = new List<Country>();
            var ZeroDayCountry = new Country();
            foreach (var group in CountryGroups)
            {
                FillMap(group.Item2, false);
                foreach (var country in group.Item2)
                {
                    if (CheckIsNeighbors(country))
                    {
                        countryToCheck.Add(country);
                    }
                    else
                    {
                        ZeroDayCountry.name = country.name;
                    }
                }
                ResetMap();
                Console.Write("\n=============================CASE===================================\n");
                if (countryToCheck.Count > 1)
                {
                    FillMap(countryToCheck, true);
                    ShareCoins(countryToCheck);
                    countryToCheck = new List<Country>();
                    ResetMap();
                }
                else
                {
                    Console.WriteLine(ZeroDayCountry.name + " days = " + 0);
                    countryToCheck = new List<Country>();
                    ResetMap();
                }
            }
        }


        public void ShareCoins(List<Country> cs)
        {
            var resolve = false;
            var day = 0;
            while (!resolve)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    for (int i = 0; i < mapsize; i++)
                    {
                        if (map[j][i] != null)
                        {
                            var citycoins = map[j][i].coins;
                            var target = new List<Coin>();
                            //ищем сверху 
                            if (j - 1 >= 0 && map[j - 1][i] != null)
                            {
                                target = Transaction(map[j][i].coins, map[j - 1][i].coins);
                            }
                            //ищем справа
                            if (i + 1 >= 0 && map[j][i + 1] != null)
                            {
                                target = Transaction(map[j][i].coins, map[j][i + 1].coins);
                            }

                            //ищем снизу
                            if (j + 1 >= 0 && map[j + 1][i] != null)
                            {
                                target = Transaction(map[j][i].coins, map[j + 1][i].coins);
                            }

                            //ищем слева
                            if (i - 1 >= 0 && map[j][i - 1] != null)
                            {
                                target = Transaction(map[j][i].coins, map[j][i - 1].coins);
                            }
                            map[j][i].coins = target;

                        }
                    }

                }
                day++;
                //check
                var countryChecked = 0;
                foreach (var country in cs)
                {
                    if (IsCountryResolved(country))
                    {
                        countryChecked++;
                        if (country.days == 0)
                        {
                            country.days = day;
                        }
                    }

                }
                resolve = countryChecked == cs.Count ? true : false;


            }
            foreach (var country in cs)
            {
                Console.WriteLine(country.name + " days = " + country.days);
            }


            day = 0;
        }

        public bool IsCountryResolved(Country country)
        {

            for (var j = country.yh; j < country.yl + 1; j++)
            {
                for (var i = country.xl; i < country.xh + 1; i++)
                {
                    foreach (var coin in map[j][i].coins)
                    {
                        if (coin.balance <= 0)
                            return false;
                    }
                }
            }
            return true;
        }

        public List<Coin> Transaction(List<Coin> from, List<Coin> to)
        {
            var _from = AddBalance(from, to);
            _from = SubBalance(from, to);

            return _from;
        }

        //share coins of same countries to each other like:
        // cityfrom                   cityto 
        //        france coin      →        france coin
        //        spanish coin      →        spanish con
        //
        //        NOT spanish coin  →     france coin
        public List<Coin> AddBalance(List<Coin> from, List<Coin> to)
        {
            foreach (var cityfrom in from)
            {
                foreach (var cityto in to)
                {
                    if (cityto.name == cityfrom.name)
                    {
                        cityto.balance += cityfrom.balance / PORTION;
                    }
                }
            }
            return from;
        }

        public List<Coin> SubBalance(List<Coin> from, List<Coin> to)
        {
            foreach (var cityfrom in from)
            {
                foreach (var cityto in to)
                {
                    if (cityto.name == cityfrom.name)
                    {
                        cityfrom.balance -= cityfrom.balance / PORTION;
                    }
                }
            }
            return from;
        }

        public bool CheckIsNeighbors(Country country)
        {
            for (var j = country.yh; j < country.yl + 1; j++)
            {
                for (var i = country.xl; i < country.xh + 1; i++)
                {
                    if (map[j - 1][i] != null && map[j - 1][i].country_name != country.name)
                    {
                        return true;
                    }
                    if (map[j + 1][i] != null && map[j + 1][i].country_name != country.name)
                    {
                        return true;
                    }
                    if (map[j][i - 1] != null && map[j][i - 1].country_name != country.name)
                    {
                        return true;
                    }
                    if (map[j][i + 1] != null && map[j][i + 1].country_name != country.name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void InitMap()
        {
            for (int i = 0; i < mapsize; i++)
            {
                var mapline = new CityList();
                for (int j = 0; j < mapsize; j++)
                {
                    mapline.Add(null);
                }

                map.Add(mapline);
            }
        }

        public void ResetMap()
        {
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    map[i][j] = null;
                }
            }
        }

        public void FillMap(List<Country> countries, bool print)
        {
            var citycount = 0;
            foreach (var country in countries)
            {
                for (var j = country.yh; j < country.yl + 1; j++)
                {
                    for (var i = country.xl; i < country.xh + 1; i++)
                    {
                        var _city = new City()
                        {
                            country_name = country.name
                        };
                        foreach (var con in countries)
                        {
                            var coin = new Coin()
                            {
                                name = country.name
                            };
                            if (con == country)
                            {
                                coin.balance = COINS_COUNT;
                                _city.coins.Add(coin);
                            }
                            else
                            {
                                coin.name = con.name;
                                coin.balance = 0;
                                _city.coins.Add(coin);
                            }
                        }
                        citycount++;
                        map[j][i] = _city;
                    }

                }
                if (print)
                {
                    Console.WriteLine(country.name + " - " + citycount + " cities");
                }

                citycount = 0;
            }
            if (print)
                PrittyPrint();
        }

        public void PrittyPrint()
        {
            Console.WriteLine('\n');
            Console.OutputEncoding = Encoding.UTF8;
            for (var j = 0; j < map.Count; j++)
            {
                for (var i = 0; i < map.Count; i++)
                {

                    Console.Write(map[j][i] == null ? "0 " : "█ ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        public class Map : List<CityList> { }

        public class CityList : List<City> { }

        public class CountryGroupe<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }
    }
}