using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant
{
    public class RestaurantPick
    {
        #region You_should_not_modify_this_region


        private class Restaurant
        {
            public int RestaurantId { get; set; }
            public Dictionary<string, decimal> Menu { get; set; }
        }

        private readonly List<Restaurant> _restaurants = new List<Restaurant>();

        /// <summary>
        /// Reads the file specified at the path and populates the restaurants list
        /// </summary>
        /// <param name="filePath">Path to the comma separated restuarant menu data</param>
        public void ReadRestaurantData(string filePath)
        {
            try
            {
                var records = File.ReadLines(filePath);

                foreach (var record in records)
                {
                    var data = record.Split(',');
                    var restaurantId = int.Parse(data[0].Trim());
                    var restaurant = _restaurants.Find(r => r.RestaurantId == restaurantId);

                    if (restaurant == null)
                    {
                        restaurant = new Restaurant { Menu = new Dictionary<string, decimal>() };
                        _restaurants.Add(restaurant);
                    }

                    restaurant.RestaurantId = restaurantId;
                    restaurant.Menu.Add(data.Skip(2).Select(s => s.Trim()).Aggregate((a, b) => a.Trim() + "," + b.Trim()), decimal.Parse(data[1].Trim()));
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Main(string[] args)
        {
            var restaurantPicker = new RestaurantPick();

            restaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv")
                    )
                );

            // Item is found in restaurant 2 at price 6.50
            Console.WriteLine("Starting Pick");
            var bestRestaurant = restaurantPicker.PickBestRestaurant("gac");

            Console.WriteLine(bestRestaurant.Item1 + ", " + bestRestaurant.Item2);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        #endregion

        #region You_can_modify_this_region

        /// <summary>
        /// Takes in items you would like to eat and returns the best restaurant that serves them.
        /// </summary>
        /// <param name="items">Items you would like to eat (seperated by ',')</param>
        /// <returns>Restaurant Id and price tuple</returns>

        public Tuple<int, decimal> PickBestRestaurant(string items)
        {  
            var items_list = items.Split(',');
            var matches = new Dictionary<int, decimal>();
            foreach (var restaurant in _restaurants)
            {
                decimal current_price = 0.0M;
                Console.WriteLine(restaurant.RestaurantId);
                var bought_items = new List<string>();
                foreach (var item in items_list)
                {
                    Console.WriteLine("Finding price for: " + item);

                    //Skip item if already purchased as part of a meal
                    if (bought_items.Contains(item))
                    {
                        continue;
                    }

                    var valid_meals = new Dictionary<string, decimal>();
                    foreach (var target_meal in restaurant.Menu)
                    {
                        if (target_meal.Key.Contains(item))
                        {
                            valid_meals.Add(target_meal.Key, target_meal.Value);
                        }

                    }

                    if (valid_meals.Count > 0)
                    {
                        var best_price = valid_meals.Aggregate((l, r) => l.Value < r.Value ? l : r);
                        Console.WriteLine(best_price.Key + ": " + best_price.Value);
                        bought_items.AddRange(best_price.Key.Split(","));
                        current_price += best_price.Value;

                    }
                    else
                    {
                        //Item isn't avaliable move on to the next restaurant
                        current_price = -1;
                        break;
                    }
                }

                if (current_price > -1)
                {
                    Console.WriteLine("Price: " + current_price);
                    matches.Add(restaurant.RestaurantId, current_price);
                }
            }

            if (matches.Count() > 0)
            {
                var best_match = matches.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                return new Tuple<int, decimal>(best_match, matches[best_match]);
            }
            else
            {
                Console.WriteLine("No matches found");
                return null;
            }
        }
        #endregion
    }
}
