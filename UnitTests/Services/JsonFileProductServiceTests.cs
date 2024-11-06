using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        //[Test]
        //public void AddRating_InValid_....()
        //{
        //    // Arrange

        //    // Act
        //    //var result = TestHelper.ProductService.AddRating(null, 1);

        //    // Assert
        //    //Assert.AreEqual(false, result);
        //}

        // ....

        [Test]
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_InValid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_InValid_Product_Does_Not_Exist_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(int.MaxValue.ToString(), 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_Valid_Product_Does_Exist_Should_Return_True()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("alonso-martinez", 1);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void AddRating_InValid_Rating_Less_Than_Zero_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("alonso-martinez", -1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_InValid_Rating_Greater_Than_Five_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("alonso-martinez", 7);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AddRating_Valid_Rating_1_to_5_Should_Return_True()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("alonso-martinez", 3);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void AddRating_Valid_Product_With_Null_Ratings_Should_Initialize_Return_true()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().Skip(1).First();
            data.Ratings = null;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 3);
            var updatedData = TestHelper.ProductService.GetAllData().Skip(1).First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        #endregion AddRating


        #region UpdateData
        [Test]
        public void UpdateData_InValid_Product_Null_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.UpdateData(null);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }


        [Test]
        public void UpdateData_InValid_Non_Existant_Product_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            data.Id = "1234";

            // Act
            var result = TestHelper.ProductService.UpdateData(data);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void UpdateData_Valid_Existing_Product_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var newTitle = "Test Name";
            data.Title = newTitle;

            // Act
            var result = TestHelper.ProductService.UpdateData(data);

            // Assert
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(data.Title, Is.EqualTo(newTitle));
        }
        #endregion UpdateData

        #region CreateData

        [Test]
        public void CreateData_Valid_Product_Should_Return_True()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.CreateData();
            var dataNewList = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(dataNewList.First(x => x.Id.Equals(result.Id)).Title, Is.EqualTo(result.Title));
        }

        [Test]
        public void CreateData_Valid_Product_Should_Return_Product()
        {
            // Arrange
            var data = new ProductModel
            {
                Title = "Ready Player",
                Dateofbirth = "15 October 1998",
                Placeofbirth = "Santiago, Chile",
                Height = "1.74 m (5 ft 8 in)",
                Position = "Goalkeeper",
                Club = "Columbus Crew",
                Appearances = "12",
                Minutesplayed = "880",
                Assists = "5",
                Image = "/data/players/ChristianBenteke.png"
            };

            // Act
            var result = TestHelper.ProductService.CreateData(data);
            var dataNewList = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(dataNewList.First(x => x.Id.Equals(result.Id)).Title, Is.EqualTo(result.Title));
        }

        [Test]
        public void CreateData_Invalid_Product_Should_Return_null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.CreateData(null);

            // Assert
            Assert.That(result, Is.Null);
        }
        #endregion CreateData

        #region DeleteData

        [Test]
        public void DeleteData_Valid_Product_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().Skip(3).First();

            // Act
            var result = TestHelper.ProductService.DeleteData(data.Id);
            var dataNewList = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(dataNewList.FirstOrDefault(x => x.Id.Equals(result.Id)), Is.EqualTo(null));
        }
        #endregion DeleteData
    }
}