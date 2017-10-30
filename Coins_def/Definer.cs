using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Coins_def
{
    class Definer
    {
        public List<City> cities = new List<City>();
        int testcase = 0;
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
                    var city = new City(parsedCity[0], xl: Int32.Parse(parsedCity[1]), yl: mapsize - 1 - Int32.Parse(parsedCity[2]),
                       xh: Int32.Parse(parsedCity[3]), yh: mapsize - 1 - Int32.Parse(parsedCity[4]))
                    {
                        testcase = testcase
                    };
                    cities.Add(city);
                }
                else
                {
                    testcase++;
                }
            }
            for (var i = 0; i < testcase; i++)
            {
                ResetMap();
                var citycase = new List<City>();
                foreach (var city in cities)
                {
                    if (city.testcase == i + 1)
                    {
                        citycase.Add(city);
                    }
                }
                FillMap(citycase);
                ShareCoins();
                Console.Write("================================================================\n");
            }
        }
        public void ShareCoins()
        {
            for (int j = 0; j < mapsize; j++)
            {
                for (int i = 0; i < mapsize; i++)
                {
                    List<Country> share_balance_of_city = new List<Country>();
                    if (map[j][i] != null)
                    {
                        foreach (var country in map[j][i].countries)
                        {
                            Country _country = new Country(country.name, country.balance / PORTION);
                            share_balance_of_city.Add(_country);
                        }

                        var cit = map[j][i];
                        //ищем сверху 
                        if (j - 1 >= 0 && map[j - 1][i] != null)
                        {
                            var y = j - 1;
                            map[y][i].countries = AddBalance(map[y][i].countries, share_balance_of_city);
                            map[j][i].countries = SubBalance(map[j][i].countries, share_balance_of_city);
                        }
                        //ищем справа
                        if (i + 1 >= 0 && map[j][i + 1] != null)
                        {
                            var x = i + 1;
                            map[j][x].countries = AddBalance(map[j][x].countries, share_balance_of_city);
                            map[j][i].countries = SubBalance(map[j][i].countries, share_balance_of_city);
                        }

                        //ищем снизу
                        if (j + 1 >= 0 && map[j + 1][i] != null)
                        {
                            var y = j + 1;
                            map[y][i].countries = AddBalance(map[y][i].countries, share_balance_of_city);
                            map[j][i].countries = SubBalance(map[j][i].countries, share_balance_of_city);
                        }

                        //ищем слева
                        if (i - 1 >= 0 && map[j][i - 1] != null)
                        {
                            var x = i - 1;
                            map[j][x].countries = AddBalance(map[j][x].countries, share_balance_of_city);
                            map[j][i].countries = SubBalance(map[j][i].countries, share_balance_of_city);
                        }

                    }
                }

            }
        }


        public List<Country> AddBalance(List<Country> tocities, List<Country> fromcities)
        {
            var targetCity = new List<Country>();
            foreach (var cityto in tocities)
            {
                foreach (var cityfrom in fromcities)
                {
                    if (cityto.name == cityfrom.name)
                    {
                        cityto.balance = cityto.balance + cityfrom.balance;
                        targetCity.Add(cityto);
                    }
                }
            }
            return targetCity;
        }

        public List<Country> SubBalance(List<Country> tocities, List<Country> fromcities)
        {
            var targetCity = new List<Country>();
            foreach (var cityto in tocities)
            {
                foreach (var cityfrom in fromcities)
                {
                    if (cityto.name == cityfrom.name)
                    {
                        cityto.balance = cityto.balance - cityfrom.balance;
                        targetCity.Add(cityto);
                    }
                }
            }
            return targetCity;
        }

        public void CreateNewMap()
        {
            var city = new CityMap<string, int>();
            foreach (var _city in cities)
            {
                city.Add(_city.name, 0);
            }

        }
        public void InitMap()
        {
            for (int i = 0; i < mapsize; i++)
            {
                var mapline = new BalanceCityList();
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
        public void FillMap(List<City> citycase)
        {

            foreach (var city in citycase)
            {
                for (var j = city.yh; j < city.yl + 1; j++)
                {
                    for (var i = city.xl; i < city.xh + 1; i++)
                    {
                        var balanceCity = new BalanceCity();
                        balanceCity.country_name = city.name;
                        foreach (var c in citycase)
                        {
                            var localCountry = new Country(c.name, 0);
                            if (c == city)
                            {
                                localCountry.balance = COINS_COUNT;
                                balanceCity.countries.Add(localCountry);
                            }
                            else
                                balanceCity.countries.Add(localCountry);
                        }

                        map[j][i] = balanceCity;
                    }
                }
                PrittyPrint();
            }
            Console.ReadKey();
        }

        public void PrittyPrint()
        {

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
            Console.Write("\n");
        }
        public class Map : List<BalanceCityList> { }

        public class BalanceCityList : List<BalanceCity> { }
    }
}