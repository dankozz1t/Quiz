using System;
using QuizGame;

namespace QuizProject
{
    public class GlobalMenu
    {
        private Game quiz = new Game();
        private QuizEditor.QuizEditor quizEditor = new QuizEditor.QuizEditor();

        public void Start()
        {
            string[] menu = { "    Игра Викторина  ", "  Редактор Викторин  ", "        Выход  " };
            int pos = 0;

            while (pos != 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                ConsoleGui.SetPosition(48, 13, true);
                pos = Menu.VerticalMenu(menu);
                if (pos == 0)
                {
                    Console.Title = "ВИКТОРИНА!";
                    quiz.Start();
                }
                else if (pos == 1)
                {
                    Console.Title = "РЕДАКТОР ВИКТОРИН!";
                    quizEditor.Start();
                }
            }
        }
    }
}