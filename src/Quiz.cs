using System;
using System.Collections.Generic;

namespace QuizGame
{
    [Serializable]
    public class Quiz
    {
        public string saveAs { get; set; } //Имя файла
        public string field { get; set; } //Сфера деятельности 
        public List<Question> questions = new List<Question>();

        //public int TotalPoints()
        //{
        //    int totalPoints = 0;
        //    foreach (var question in questions)
        //    {
        //       totalPoints += question.CountCorrectAnswers();
        //    }

        //    return totalPoints;
        //}

        public override string ToString()
        {
            return $" Викторина: {field} | Количество вопросов: {questions.Count} ";
        }

        public void FullShow()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" ВИКТОРИНА: {field}");
            foreach (var question in questions)
            { 
                question.Show();
            }
        }
    }
}