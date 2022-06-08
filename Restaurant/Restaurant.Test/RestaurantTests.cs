using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Restaurant.Test
{
    [TestClass]
    public class RestaurantPickTests
    {
        public RestaurantPick RestaurantPicker;

        [TestInitialize]
        public void InitializeTests()
        {
            Console.WriteLine("Here");
            RestaurantPicker = new RestaurantPick();
            RestaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv")
                    )
                );
        }

        [TestMethod]
        public void PickBestRestaurantTest_OneItem()
        {
            var result = RestaurantPicker.PickBestRestaurant("gac");
            Assert.AreEqual(result, new Tuple<int, decimal>(9, (decimal)3.35));
        }

        [TestMethod]
        public void PickBestRestaurantTest_TwoItems()
        {
            var result = RestaurantPicker.PickBestRestaurant("almond_biscuit,joe_frogger");
            Assert.AreEqual(result, new Tuple<int, decimal>(12, (decimal)1.70));
        }

        [TestMethod]
        public void PickBestRestaurantTest_CombinationItems_1()
        {
            var result = RestaurantPicker.PickBestRestaurant("rimu,red_mombin,sageretia");
            Assert.AreEqual(result, new Tuple<int, decimal>(9, (decimal)3.17));
        }

        [TestMethod]
        public void PickBestRestaurantTest_CombinationItems_2()
        {
            var result = RestaurantPicker.PickBestRestaurant("burdekin_plum,canistel,black_walnut,black_raspberry,black_sapote");
            Assert.AreEqual(result, new Tuple<int, decimal>(14, (decimal)5.69));
        }

        [TestMethod]
        public void PickBestRestaurantTest_NotFound()
        {
            var result = RestaurantPicker.PickBestRestaurant("chocolate_teacake,pigeon_plum,bolivian_coconut,magdalena,extreme_fajita");
            Assert.IsNull(result);
        }
    }
}
