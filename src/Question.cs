using System;
using System.Collections.Generic;

namespace QuizGame
{
    [Serializable]
    public class Question
    {
        public string question { get; set; }
        public List<Answer> answers = new List<Answer>();

        public void Show()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" [?] {question}");

            foreach (var answer in answers)
            {
                Console.WriteLine(answer);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public int CountCorrectAnswers()
        {
            int correctAnswers = 0;
            foreach (var answer in answers)
            {
                if (answer.IsRight)
                    correctAnswers++;
            }

            return correctAnswers;
        }
    }
}