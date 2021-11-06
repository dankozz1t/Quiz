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
        public LeaderBoard leaderBoard = new LeaderBoard();

        public void ShowLeaderBoard()
        {
            ConsoleGui.SetPosition(50, 2, true);
            ConsoleGui.WriteLineColor($"ДОСКА ЛИДЕРОВ", ConsoleColor.Red, true);
            ConsoleGui.WriteLineColor($"ВИКТОРИНА <<{field}>>", ConsoleColor.Yellow, true);
            Console.CursorTop++;

            ConsoleGui.WriteLineColor($"{"ИГРОК".PadRight(16)} {"БАЛЛОВ".PadRight(10)} {"     ДАТА      ВРЕМЯ".PadRight(10)}", ConsoleColor.DarkCyan, true);

            leaderBoard.Show();
        }

        public void Show()
        {
            ConsoleGui.WriteLineColor($" ВИКТОРИНА: \"{field}\"", ConsoleColor.Red, true);
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