using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests.Pages.Product.AddRating
{
    /// <summary>
    /// Tests for JsonFileProductService.
    /// </summary>
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        /// <summary>
        /// Initializes the test setup.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating

        /// <summary>
        /// Test case for invalid product being null, expecting false.
        /// </summary>
        [Test]
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        /// <summary>
        /// Test case for invalid product being empty, expecting false.
        /// </summary>
        [Test]
        public void AddRating_InValid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        /// <summary>
        /// Test case for product that does not exist, expecting false.
        /// </summary>
        [Test]
        public void AddRating_InValid_Product_Does_Not_Exist_Should_Return_False()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating(int.MaxValue.ToString(), 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        /// <summary>
        /// Test case for valid product, expecting true.
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_Does_Exist_Should_Return_True()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating("alonso-martinez", 1);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }

        /// <summary>
        /// Test case for invalid rating less than 0, expecting false.
        /// </summary>
        [Test]
        public void AddRating_InValid_Rating_Less_Than_Zero_Should_Return_False()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating("alonso-martinez", -1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        /// <summary>
        /// Test case for invalid rating greater than 5, expecting false.
        /// </summary>
        [Test]
        public void AddRating_InValid_Rating_Greater_Than_Five_Should_Return_False()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating("alonso-martinez", 7);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }


        /// <summary>
        /// Test case for valid rating between 1 and 5, expecting true.
        /// </summary>
        [Test]
        public void AddRating_Valid_Rating_1_to_5_Should_Return_True()
        {
            // Arrange

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating("alonso-martinez", 3);

            // Assert
            var updatedData = TestHelper.ProductService.GetAllData().First(x => x.Id.Equals("alonso-martinez"));
            Assert.That(updatedData.Ratings.Last(), Is.EqualTo(3));
        }

        /// <summary>
        /// Test case for valid product with null ratings, should create a rating array and add rating.
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_With_Null_Rating_Should_Initialize_Ratings_Before_Adding_Rating()
        {
            // Arrange

            // Variable to hold product data with null ratings
            var data = TestHelper.ProductService.GetAllData().Skip(1).First();
            data.Ratings = null;

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating(data.Id, 3);

            // Assert

            // Variable to hold updated product data
            var updatedData = TestHelper.ProductService.GetAllData().Skip(1).First();
            Assert.That(updatedData.Ratings.First(), Is.EqualTo(3));
        }

        /// <summary>
        /// Test case for adding a rating to a valid product updates the ratings array correctly.
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_And_Rating_Should_Add_Rating()
        {
            // Arrange

            // Variable to hold initial product data
            var data = TestHelper.ProductService.GetAllData().First();

            // Variable to hold the original count of ratings
            var countOriginal = data.Ratings.Length;

            // Act

            // Variable to hold the result of AddRating method call
            var result = TestHelper.ProductService.AddRating(data.Id, 5);

            // Assert

            // Variable to hold updated product data
            var dataNewList = TestHelper.ProductService.GetAllData().First();
            Assert.That(result, Is.EqualTo(true));
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        #endregion AddRating


        #region UpdateData

        /// <summary>
        /// Test case for attempting to update a product with null data returns null.
        /// </summary>
        [Test]
        public void UpdateData_Invalid_Product_Null_Should_Return_Null()
        {
            // Arrange

            // Variable to hold the first product from data list
            var data = TestHelper.ProductService.GetAllData().First();

            // Act

            // Variable to hold the result of the UpdateData method call
            var result = TestHelper.ProductService.UpdateData(null);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        /// <summary>
        /// Test case for attempting to update a product with a non-existent ID, expecting null to be returned.
        /// </summary>
        [Test]
        public void UpdateData_Invalid_Product_NonExistent_Id_Should_Return_Null()
        {
            // Arrange

            // Variable to hold the first product from data list with modified ID
            var data = TestHelper.ProductService.GetAllData().First();
            data.Id = "1234";

            // Act

            // Variable to hold the result of the UpdateData method call
            var result = TestHelper.ProductService.UpdateData(data);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        /// <summary>
        /// Test case for updating an existing product with valid data, expecting the product to be updated successfully.
        /// </summary>
        [Test]
        public void UpdateData_Valid_Existing_Product_Should_Return_Updated_Product()
        {
            // Arrange

            // Variable to hold the first product from data list with modified title
            var data = TestHelper.ProductService.GetAllData().First();

            var newTitle = "Test Name";
            data.Title = newTitle;

            // Act
            // Variable to hold the result of the UpdateData method call
            var result = TestHelper.ProductService.UpdateData(data);

            // Assert
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(data.Title, Is.EqualTo(newTitle));
        }
        #endregion UpdateData

        #region CreateData

        /// <summary>
        /// Test case for creating a null product, expecting null to be returned.
        /// </summary>
        [Test]
        public void CreateData_Invalid_Product_Should_Return_null()
        {
            // Arrange

            // Act

            // Variable to hold the result of the CreateData method call
            var result = TestHelper.ProductService.CreateData(null);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test case for creating a valid product with no input parameters, expecting the product to be added successfully and returned.
        /// </summary>
        [Test]
        public void CreateData_Valid_Product_No_Parameters_Should_Return_Created_Product()
        {
            // Arrange

            // Act

            // Variable to hold the result of the CreateData method call
            var result = TestHelper.ProductService.CreateData();

            // Assert

            // Variable to hold the new data list after creation
            var dataNewList = TestHelper.ProductService.GetAllData();
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(dataNewList.First(x => x.Id.Equals(result.Id)).Title, Is.EqualTo(result.Title));
        }

        /// <summary>
        /// Test case for creating a valid product, expecting the product to be added successfully and returned.
        /// </summary>
        [Test]
        public void CreateData_Valid_Product_With_Parameters_Should_Return_Product()
        {
            // Arrange

            // Variable to hold new product data
            var data = new ProductModel
            {
                Title = "Ready Player",
                Dateofbirth = "15 October 1998",
                Placeofbirth = "Santiago, Chile",
                Height = "1.74 m (5 ft 8 in)",
                Position = "Goalkeeper",
                Club = "Columbus Crew",
                Appearances = 12,
                Minutesplayed = 880,
                Assists = 5,
                Image = "/data/players/ChristianBenteke.png"
            };

            // Act

            // Variable to hold the result of the CreateData method call
            var result = TestHelper.ProductService.CreateData(data);


            // Assert

            // Variable to hold the new data list after creation
            var dataNewList = TestHelper.ProductService.GetAllData();
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(dataNewList.First(x => x.Id.Equals(result.Id)).Title, Is.EqualTo(result.Title));
        }

        /// <summary>
        /// Test case for creating a valid product, expecting the product to be added successfully and returned.
        /// </summary>
        [Test]
        public void CreateData_Valid_Product_With_Duplicate_Parameters_Should_Return_Product_With_Numbered_Id()
        {
            // Arrange

            // Variable to hold new product data
            var data1 = new ProductModel
            {
                Title = "Ready Player",
                Dateofbirth = "15 October 1998",
                Placeofbirth = "Santiago, Chile",
                Height = "1.74 m (5 ft 8 in)",
                Position = "Goalkeeper",
                Club = "Columbus Crew",
                Appearances = 12,
                Minutesplayed = 880,
                Assists = 5,
                Image = "/data/players/ChristianBenteke.png"
            };
            var data2 = new ProductModel
            {
                Title = "Ready Player",
                Dateofbirth = "15 October 1998",
                Placeofbirth = "Santiago, Chile",
                Height = "1.74 m (5 ft 8 in)",
                Position = "Goalkeeper",
                Club = "Columbus Crew",
                Appearances = 12,
                Minutesplayed = 880,
                Assists = 5,
                Image = "/data/players/ChristianBenteke.png"
            };

            // Act

            // Create first player
            var originalPlayer = TestHelper.ProductService.CreateData(data1);
            Console.WriteLine(originalPlayer.Id);

            // Variable to hold the duplicate player
            var duplicatedPlayer = TestHelper.ProductService.CreateData(data2);
            Console.WriteLine(duplicatedPlayer.Id);


            // Assert

            // Variable to hold the new data list after creation
            var dataNewList = TestHelper.ProductService.GetAllData();
            Assert.That(originalPlayer, Is.InstanceOf<ProductModel>());
            Assert.That(duplicatedPlayer, Is.InstanceOf<ProductModel>());
            Assert.That(duplicatedPlayer.Id, Is.Not.EqualTo(originalPlayer.Id));
            Assert.That(duplicatedPlayer.Id, Is.Not.EqualTo(originalPlayer.Id));
            Assert.That(originalPlayer.Id, Does.Contain("Ready-Player"));
            Assert.That(duplicatedPlayer.Id, Does.Contain("Ready-Player"));
        }

        #endregion CreateData

        #region DeleteData

        /// <summary>
        /// Test case for deleting a valid product, expecting the product to be removed and true to be returned.
        /// </summary>
        [Test]
        public void DeleteData_Valid_Product_Should_Return_True()
        {
            // Arrange

            // Variable to hold the third product in the data list
            var data = TestHelper.ProductService.GetAllData().Skip(3).First();

            // Act

            // Variable to hold the result of the DeleteData method call
            var result = TestHelper.ProductService.DeleteData(data.Id);

            // Assert

            // Variable to hold the new data list after deletion
            var dataNewList = TestHelper.ProductService.GetAllData();
            Assert.That(result, Is.InstanceOf<ProductModel>());
            Assert.That(dataNewList.FirstOrDefault(x => x.Id.Equals(result.Id)), Is.EqualTo(null));
        }

        #endregion DeleteData
    }
}