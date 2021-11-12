using System;
using QuizGame;

namespace QuizProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            Console.Title = "МЕНЮ";

            PlayersDatabase.LoadUsers();
            QuizzesDatabase.LoadQuizzes();

            GlobalMenu menu = new GlobalMenu();
            menu.Start();
        }
    }
}
