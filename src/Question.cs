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
            ConsoleGui.WriteLineColor($" [?] {question}", ConsoleColor.Yellow, true);

            foreach (var answer in answers)
            {
                if(answer.IsRight)
                    ConsoleGui.WriteLineColor($" [*] {answer.answer}   [{answer.IsRight}]", ConsoleColor.Green, true);
                else
                {
                    ConsoleGui.WriteLineColor($" [*] {answer.answer}   [{answer.IsRight}]", ConsoleColor.Red, true);
                }
            }
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