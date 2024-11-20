using System.Collections.Generic;
using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace UnitTests.Components
{
    public class PlayerDataTests : Bunit.TestContext
    {
        // Creating the Test product service
        private TestJsonFileProductService testProductService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Creating mock data for testing
            var testPlayers = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = "1",
                    Title = "Player One",
                    Image = "player1.jpg",
                    Dateofbirth = "1990-01-01",
                    Age = 34,
                    Placeofbirth = "City A",
                    HeightInCMs = 170,
                    Position = "Forward",
                    Club = "Club A",
                    Appearances = 100,
                    Minutesplayed = 9000,
                    Goals = 50,
                    Assists = 30
                },
                new ProductModel
                {
                    Id = "2",
                    Title = "Player Two",
                    Image = "player2.jpg",
                    Dateofbirth = "1992-02-02",
                    Age = 32,
                    Placeofbirth = "City B",
                    HeightInCMs = 170,
                    Position = "Midfielder",
                    Club = "Club B",
                    Appearances = 80,
                    Minutesplayed = 7200,
                    Goals = 20,
                    Assists = 40
                }
            };

            // Creating a testProductService for use in this unit test 
            testProductService = new TestJsonFileProductService(testPlayers);

            // Adding the mock service to the services collection
            Services.AddSingleton<JsonFileProductService>(testProductService);
        }

        /// <summary>
        /// Setup runs before each test to initialize or reset data.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Setting up playersList before each test execution
            testProductService.SetProducts(new List<ProductModel>
            {
                new ProductModel
                {
                    Id = "1",
                    Title = "Player One",
                    Image = "player1.jpg",
                    Dateofbirth = "1990-01-01",
                    Age = 34,
                    Placeofbirth = "City A",
                HeightInCMs = 174,
                    Position = "Forward",
                    Club = "Club A",
                    Appearances = 100,
                    Minutesplayed = 9000,
                    Goals = 50,
                    Assists = 30
                },
                new ProductModel
                {
                    Id = "2",
                    Title = "Player Two",
                    Image = "player2.jpg",
                    Dateofbirth = "1992-02-02",
                    Age = 32,
                    Placeofbirth = "City B",
                    HeightInCMs = 174,
                    Position = "Midfielder",
                    Club = "Club B",
                    Appearances = 80,
                    Minutesplayed = 7200,
                    Goals = 20,
                    Assists = 40
                }
            });
        }

        /// <summary>
        /// Test to ensure that PlayerData component displays correct data for a valid player ID.
        /// </summary>
        [Test]
        public void PlayerData_Valid_Id_Players_Displays_Correct_Data_Eleements()
        {
            // Arrange
            var validId = "1";

            // Act
            var cut = RenderComponent<PlayerData>(parameters => parameters.Add(p => p.Id, validId));
            var heading = cut.Find("h1");
            var image = cut.Find("img");
            var listItems = cut.FindAll("ol li");

            // Assert
            Assert.That(heading.TextContent, Is.EqualTo("Player One"));

            Assert.That(image.GetAttribute("src"), Is.EqualTo("player1.jpg"));
            Assert.That(image.GetAttribute("alt"), Is.EqualTo("Player One"));

            Assert.That(listItems.Count, Is.EqualTo(9));
            Assert.That(listItems[0].TextContent, Does.Contain("Date of Birth: 1990-01-01 (34)"));
            Assert.That(listItems[1].TextContent, Does.Contain("Place of Birth: City A"));
            Assert.That(listItems[2].TextContent, Does.Contain("Height: 1.74m"));
            Assert.That(listItems[3].TextContent, Does.Contain("Position(s): Forward"));
            Assert.That(listItems[4].TextContent, Does.Contain("Current Team: Club A"));
            Assert.That(listItems[5].TextContent, Does.Contain("Games Played: 100"));
            Assert.That(listItems[6].TextContent, Does.Contain("Minutes Played: 9000"));
            Assert.That(listItems[7].TextContent, Does.Contain("Goals: 50"));
            Assert.That(listItems[8].TextContent, Does.Contain("Assists: 30"));
        }

        /// <summary>
        /// Test implementation of JsonFileProductService that allows setting of mock data.
        /// </summary>
        public class TestJsonFileProductService : JsonFileProductService
        {
            private IEnumerable<ProductModel> players;

            /// <summary>
            /// Constructor for the TestJsonFileProductService.
            /// </summary>
            /// <param name="playersList">Initial set of players to use for testing.</param>
            public TestJsonFileProductService(IEnumerable<ProductModel> playersList)
                : base(null)
            {
                players = playersList;
            }

            /// <summary>
            /// Override to return the mock product data.
            /// </summary>
            /// <returns>List of ProductModel representing playersList.</returns>
            public override IEnumerable<ProductModel> GetAllData()
            {
                return players;
            }

            /// <summary>
            /// Set the playersList to be used in the service.
            /// </summary>
            /// <param name="playersList">New list of players to set.</param>
            public void SetProducts(IEnumerable<ProductModel> playersList)
            {
                players = playersList ?? new List<ProductModel>();
            }
        }
    }
}
