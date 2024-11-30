using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ContosoCrafts.WebSite.Pages
{
    public class TriviaChallengeModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSpinWheel()
        {
            await SpinWheel();
            return Page();
        }

        public IActionResult OnPostCheckAnswer(string selectedOption)
        {
            // Your check answer logic
            CheckAnswer(selectedOption);
            return Page();
        }

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

        // Trivia question model
        public class TriviaQuestion
        {
            public string QuestionText { get; set; }
            public string[] Options { get; set; }
            public string CorrectAnswer { get; set; }
        }

        // Trivia questions dictionary
        private Dictionary<string, List<TriviaQuestion>> _trivia = new Dictionary<string, List<TriviaQuestion>>
        {
            ["Player Trivia"] = new List<TriviaQuestion>
        {
            new TriviaQuestion
            {
                QuestionText = "Who won the MVP award last season?",
                Options = new[] { "Player A", "Player B", "Player C", "Player D" },
                CorrectAnswer = "Player A"
            }
        },
            ["Team Trivia"] = new List<TriviaQuestion>
        {
            new TriviaQuestion
            {
                QuestionText = "Which team won the championship last year?",
                Options = new[] { "Team X", "Team Y", "Team Z", "Team W" },
                CorrectAnswer = "Team Y"
            }
        },
            ["History Trivia"] = new List<TriviaQuestion>
        {
            new TriviaQuestion
            {
                QuestionText = "In what year was the first championship held?",
                Options = new[] { "1950", "1960", "1970", "1980" },
                CorrectAnswer = "1960"
            }
        },
            ["Stats Trivia"] = new List<TriviaQuestion>
        {
            new TriviaQuestion
            {
                QuestionText = "What is the highest average points per game in a season?",
                Options = new[] { "28.5", "30.1", "35.2", "40.3" },
                CorrectAnswer = "35.2"
            }
        }
        };

        // Method to spin the wheel and select a category and question
        public async Task SpinWheel()
        {
            IsSpinning = true;
            SelectedAnswer = null;
            IsCorrectAnswer = false;

            // Simulate wheel spinning for 3 seconds
            await Task.Delay(3000);

            // Select a random category
            Random random = new Random();
            CurrentCategory = Categories[random.Next(Categories.Length)];

            // Select a random question from the category
            CurrentQuestion = _trivia[CurrentCategory][random.Next(_trivia[CurrentCategory].Count)];

            IsSpinning = false;
        }

        // Method to check the selected answer
        public void CheckAnswer(string selectedAnswer)
        {
            SelectedAnswer = selectedAnswer;
            IsCorrectAnswer = selectedAnswer == CurrentQuestion.CorrectAnswer;
        }

        // Method to determine button styling based on answer selection
        public string GetButtonClass(string option)
        {
            if (SelectedAnswer == null)
                return "btn-outline-primary";

            if (option == CurrentQuestion.CorrectAnswer)
                return "btn-success";
            else if (option == SelectedAnswer)
                return "btn-danger";
            else
                return "btn-secondary disabled";
        }
    }
}
