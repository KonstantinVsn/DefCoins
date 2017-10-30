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
        int testcase = 0;
        Map map = new Map();
        int mapsize = 10;
        List<Country> countrycase = new List<Country>();
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
                    var country = new Country(parsedCity[0], xl: Int32.Parse(parsedCity[1]), yl: mapsize - 1 - Int32.Parse(parsedCity[2]),
                       xh: Int32.Parse(parsedCity[3]), yh: mapsize - 1 - Int32.Parse(parsedCity[4]))
                    {
                        testcase = testcase
                    };
                    countries.Add(country);
                }
                else
                {
                    testcase++;
                }
            }
            for (var i = 0; i < testcase; i++)
            {
                ResetMap();
                
                foreach (var country in countries)
                {
                    if (country.testcase == i + 1)
                    {
                        countrycase.Add(country);
                    }
                }
                FillMap(countrycase);
                ShareCoins();
                Console.Write("================================================================\n");
            }
        }


        public void ShareCoins()
        {
            var resolve = false;
            var day = 0;
            while (!resolve)
            {
                day++;
                for (int j = 0; j < mapsize; j++)
                {
                    for (int i = 0; i < mapsize; i++)
                    {
                        //List<Coins> share_balance_of_city = new List<City>();
                        if (map[j][i] != null)
                        {
                            /*foreach (var country in map[j][i].coins)
                            {
                                Country _country = new Country(country.name, country.balance / PORTION);
                                share_balance_of_city.Add(_country);
                            }*/

                            //ищем сверху 
                            if (j - 1 >= 0 && map[j - 1][i] != null)
                            {
                                var y = j - 1;
                                map[y][i].coins = AddBalance(map[y][i].coins, map[y][i].coins);
                                map[j][i].coins = SubBalance(map[j][i].coins, map[y][i].coins);
                            }
                            //ищем справа
                            if (i + 1 >= 0 && map[j][i + 1] != null)
                            {
                                var x = i + 1;
                                map[j][x].coins = AddBalance(map[j][x].coins, map[y][i].coins);
                                map[j][i].coins = SubBalance(map[j][i].coins, map[y][i].coins);
                            }

                            //ищем снизу
                            if (j + 1 >= 0 && map[j + 1][i] != null)
                            {
                                var y = j + 1;
                                map[y][i].coins = AddBalance(map[y][i].coins, map[y][i].coins);
                                map[j][i].coins = SubBalance(map[j][i].coins, map[y][i].coins);
                            }

                            //ищем слева
                            if (i - 1 >= 0 && map[j][i - 1] != null)
                            {
                                var x = i - 1;
                                map[j][x].coins = AddBalance(map[j][x].coins, map[y][i].coins);
                                map[j][i].coins = SubBalance(map[j][i].coins, map[y][i].coins);
                            }

                        }
                    }

                }   

                var country_for_check = new List<Country>();

                foreach (var country in countrycase)
                {
                    Country _country = new Country(country.name, 0);
                    country_for_check.Add(_country);
                }

                //check
                for (int j = 0; j < mapsize; j++)
                {
                    for (int i = 0; i < mapsize; i++)
                    {
                        if (map[j][i] != null)
                        {
                            foreach (var country in map[j][i].countries)
                            {
                                if (country != null && country.balance == 0)
                                {
                                   foreach(var countrycheck in country_for_check)
                                    {
                                        if(countrycheck.name == country.name)
                                        {
                                            countrycheck.check = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(day);
        }


        public List<Coin> AddBalance(List<Coin> tocities, List<Coin> fromcities)
        {
            var targetCity = new List<Coin>();
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

        public List<Coin> SubBalance(List<Coin> tocities, List<Coin> fromcities)
        {
            var targetCity = new List<Coin>();
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
        public void FillMap(List<Country> countries)
        {

            foreach (var country in countries)
            {
                for (var j = country.yh; j < country.yl + 1; j++)
                {
                    for (var i = country.xl; i < country.xh + 1; i++)
                    {
                        var _city = new City(){
                            country_name = country.name
                        };
                        foreach (var con in countries)
                        {
                            var coin = new Coin() {
                                name = country.name
                            };
                            if (con == country)
                            {
                                coin.balance = COINS_COUNT;
                                _city.coins.Add(coin);
                            }
                            else
                                _city.coins.Add(coin);
                        }

                        map[j][i] = _city;
                    }
                }
                PrittyPrint();
            }
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
        public class Map : List<CityList> { }

        public class CityList : List<City> { }
    }
}