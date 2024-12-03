using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore.Drawing;
using System;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Class for methods and properties related to player comparison
    /// </summary>
    public class ComparePlayersModel : PageModel
    {
        // Logger instance for logging information and errors
        private readonly ILogger<ComparePlayersModel> _logger;

        /// <summary>
        /// Constructor for initializing the ComparePlayersModel with logger and player service dependencies.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="playerService">Service for handling player data retrieval.</param>

        public ComparePlayersModel(ILogger<ComparePlayersModel> logger,
           JsonFileProductService playerService)
        {
            _logger = logger;
            PlayerService = playerService;
        }

        // Service to retrieve player data
        public JsonFileProductService PlayerService { get; }

        // List of players retrieved from the PlayerService
        public IEnumerable<ProductModel> PlayerList { get; set; }

        // List of items to display in a dropdown for selecting players
        public IEnumerable<SelectListItem> PlayerDropDownItems { get; set; }

        // Player selected on the left side for comparison
        public ProductModel SelectedPlayerLeft { get; set; }
        [BindProperty(SupportsGet = true)]

        // ID of the selected player on the left side
        public string SelectedPlayerLeftId { get; set; }

        // Player selected on the right side for comparison
        public ProductModel SelectedPlayerRight { get; set; }

        [BindProperty(SupportsGet = true)]
        // ID of the selected player on the right side
        public string SelectedPlayerRightId { get; set; }

        public string comparisonChartBase64 { get; set; }

        /// <summary>
        /// Handles GET requests to populate the player list, selected players, and dropdown items for player selection.
        /// </summary>
        public void OnGet()
        {
            // Retrieve all player data from the PlayerService
            PlayerList = PlayerService.GetAllData();

            // Set the left player based on SelectedPlayerLeftId
            if (SelectedPlayerLeftId != null)
            {
                SelectedPlayerLeft = PlayerList.FirstOrDefault(p => p.Id.ToString() == SelectedPlayerLeftId);
            }

            // Set the right player based on SelectedPlayerRightId
            if (SelectedPlayerRightId != null)
            {
                SelectedPlayerRight = PlayerList.FirstOrDefault(p => p.Id.ToString() == SelectedPlayerRightId);
            }
            GenerateChart();
            // Populate dropdown items with player names for selection
            PlayerDropDownItems = PlayerList.Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();
        }

        /// <summary>
        /// Generates a polar chart comparing the selected players' statistics 
        /// and saves the chart as an image.
        /// </summary>
        public void GenerateChart()
        {
            if (SelectedPlayerLeft == null)
            {
                return;
            }
            if (SelectedPlayerRight == null)
            {
                return;
            }

            // Calculate the maximum appearance and add 500 to it
            int maxMinutesplayed = Math.Max(SelectedPlayerLeft.Minutesplayed, SelectedPlayerRight.Minutesplayed);

            // Scale player 1's Minutes played to the new scale
            int scaledPlayer1Minutesplayed = (int)((SelectedPlayerLeft.Minutesplayed / (float)maxMinutesplayed) * 40);

            // Scale player 2's Minutes played to the new scale
            int scaledPlayer2Minutesplayed = (int)((SelectedPlayerRight.Minutesplayed / (float)maxMinutesplayed) * 40);

            // Series for the chart's data
            var series = new List<PolarLineSeries<int>>
            {
                new PolarLineSeries<int>
                {
                    Values = new List<int> { SelectedPlayerLeft.Goals, SelectedPlayerLeft.Assists, SelectedPlayerLeft.Appearances, SelectedPlayerLeft.CleanSheets, SelectedPlayerLeft.Age,  SelectedPlayerLeft.HeightInCMs/10, scaledPlayer1Minutesplayed },
                    LineSmoothness = 0,
                    GeometrySize = 0,
                    Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(90)),
                    Name = SelectedPlayerLeft.Title,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Blue.WithAlpha(90)),
                    DataLabelsSize = 40
                },
                new PolarLineSeries<int>
                {
                    Values = new List<int> { SelectedPlayerRight.Goals, SelectedPlayerRight.Assists, SelectedPlayerRight.Appearances, SelectedPlayerRight.CleanSheets, SelectedPlayerRight.Age,  SelectedPlayerRight.HeightInCMs/10, scaledPlayer2Minutesplayed },
                    LineSmoothness = 0,
                    GeometrySize = 0,
                    Fill = new SolidColorPaint(SKColors.Red.WithAlpha(90)),
                    Name = SelectedPlayerRight.Title,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Red.WithAlpha(90)),
                    DataLabelsSize = 40
                }
            };

            // Configure the angle axes with labels and styling
            List<PolarAxis> AngleAxes = new List<PolarAxis>()
           {
               new PolarAxis
               {
                   LabelsRotation = LiveCharts.TangentAngle,
                   Labels = new[] { "Goals", "Assists", "Appearances", "CleanSheets", "Age", "Height", "Minutes Played" },
                   LabelsBackground = LvcColor.FromArgb(0, 0, 0, 0),
                   LabelsPaint = new SolidColorPaint
                        {
                           Color = SKColors.White,
                            SKTypeface = SKTypeface.FromFamilyName("Arial"),
                            SKFontStyle = new SKFontStyle(SKFontStyleWeight.Black, SKFontStyleWidth.SemiExpanded, SKFontStyleSlant.Upright),
                        },
                   TextSize = 40,
                   NamePaint = new SolidColorPaint
                        {
                           Color = SKColors.White,
                            SKTypeface = SKTypeface.FromFamilyName("Arial"),
                            SKFontStyle = new SKFontStyle(SKFontStyleWeight.Black, SKFontStyleWidth.SemiExpanded, SKFontStyleSlant.Upright),
                        },
                   NameTextSize = 40
               }
           };

            // Configure the polar chart with series and axes
            var polarChart = new SKPolarChart
            {
                Width = 1500,
                Height = 1500,
                Series = series,
                AngleAxes = AngleAxes,
                Background = SKColors.Transparent,
                InitialRotation = -45.0,
                LegendTextSize = 50,
                FitToBounds = false
            };


            // Save the polar chart as an image
            using var image = polarChart.GetImage();
            using var data = image.Encode();
            comparisonChartBase64 = Convert.ToBase64String(data.AsSpan());
        }
    }
}