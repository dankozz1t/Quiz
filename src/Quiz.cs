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

        public void Show()
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