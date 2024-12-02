using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using static ContosoCrafts.WebSite.Pages.TriviaChallengeModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContosoCrafts.WebSite.Pages
{
    public class TriviaChallengeModel : PageModel
    {
        // Properties exposed to the Razor page
        public string[] Categories { get; } = new[] {
            "Player Trivia",
            "Team Trivia",
            "History Trivia",
            "Stats Trivia"
        };

        public bool IsSpinning { get; private set; }
        public string CurrentCategory { get; private set; }
        public TriviaQuestion CurrentQuestion { get; private set; }
        public string SelectedAnswer { get; private set; }
        public bool IsCorrectAnswer { get; private set; }

        private readonly Dictionary<string, List<TriviaQuestion>> _trivia;

        public TriviaChallengeModel()
        {
            // Load trivia from JSON file
            _trivia = LoadTriviaFromJson("U:/Repos/DribbleKings/src/wwwroot/data/triviaQuestions.json");
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSpinWheel()
        {
            IsSpinning = true;

            // Simulate spinning logic
            await SpinWheel();

            return Page();
        }


        public IActionResult OnPostCheckAnswer(string selectedOption)
        {
            CheckAnswer(selectedOption);
            return Page();
        }

        // Trivia question model
        public class TriviaQuestion
        {
            public string QuestionText { get; set; }
            public string[] Options { get; set; }
            public string CorrectAnswer { get; set; }
        }

        public class TriviaJson
        {
            public List<TriviaCategory> TriviaQuestions { get; set; }
        }

        public class TriviaCategory
        {
            public string Category { get; set; }
            public List<QuestionData> Questions { get; set; }
        }

        public class QuestionData
        {
            public int Id { get; set; }
            public string Question { get; set; }
            public string[] Options { get; set; }
            public string CorrectAnswer { get; set; }
        }

        // Method to load trivia from a JSON file
        private static Dictionary<string, List<TriviaQuestion>> LoadTriviaFromJson(string jsonFilePath)
        {
            try
            {
                string json = System.IO.File.ReadAllText(jsonFilePath);
                var parsedData = JsonConvert.DeserializeObject<TriviaJson>(json);

                if (parsedData == null || parsedData.TriviaQuestions == null)
                {
                    throw new Exception("The parsed data or TriviaQuestions are null.");
                }

                var trivia = new Dictionary<string, List<TriviaQuestion>>();

                // Log the categories to check if all expected categories are present
                Console.WriteLine("Loaded Categories:");
                foreach (var category in parsedData.TriviaQuestions)
                {
                    Console.WriteLine($"- {category.Category}");
                    var triviaQuestions = new List<TriviaQuestion>();
                    foreach (var question in category.Questions)
                    {
                        triviaQuestions.Add(new TriviaQuestion
                        {
                            QuestionText = question.Question,
                            Options = question.Options,
                            CorrectAnswer = question.CorrectAnswer
                        });
                    }
                    trivia[category.Category] = triviaQuestions;
                }

                return trivia;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading trivia: {ex.Message}");
                return new Dictionary<string, List<TriviaQuestion>>();
            }
        }




        // Method to spin the wheel and select a category and question
        public async Task SpinWheel()
        {
            IsSpinning = true;
            SelectedAnswer = null;
            IsCorrectAnswer = false;

            // Simulate wheel spinning for 3 seconds
            await Task.Delay(3000);

            // Randomly pick a category and question
            Random random = new Random();
            CurrentCategory = Categories[random.Next(Categories.Length)];

            // Ensure the category exists before trying to access it
            if (!_trivia.ContainsKey(CurrentCategory))
            {
                Console.WriteLine($"Category '{CurrentCategory}' not found in trivia.");
                return;  // or set a default category
            }

            CurrentQuestion = _trivia[CurrentCategory][random.Next(_trivia[CurrentCategory].Count)];

            IsSpinning = false;
        }




        // Method to check the selected answer
        public void CheckAnswer(string selectedAnswer)
        {
            SelectedAnswer = selectedAnswer;
            IsCorrectAnswer = selectedAnswer == CurrentQuestion?.CorrectAnswer;
        }

        // Method to determine button styling based on answer selection
        public string GetButtonClass(string option)
        {
            if (SelectedAnswer == null)
                return "btn-outline-primary";

            if (option == CurrentQuestion?.CorrectAnswer)
                return "btn-success";
            else if (option == SelectedAnswer)
                return "btn-danger";
            else
                return "btn-secondary disabled";
        }
    }
}
