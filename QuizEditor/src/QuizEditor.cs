using System;
using QuizGame;

namespace QuizEditor
{
    public class QuizEditor
    {
        public static void Start()
        {
            Console.WriteLine("Редактор Викторин (ТОЛЬКО АДМИНАМ)");

            string[] menu = { "Вход", "Выход" };
            int pos = 0;

            while (pos != 1)
            {
                pos = QuizGame.Menu.VerticalMenu(menu);
                if (pos == 0)
                {
                    //Логика проверки Юзера

                    Menu();
                }
            }
        }


        private static void Menu()
        {
            Console.WriteLine("Главное меню Редактора Викторин");

            string[] menu = { "Создать Викторину", "Редактировать Викторину", "Удалить Викторину", "Выход" };
            int pos = 0;

            while (pos != 3)
            {
                pos = QuizGame.Menu.VerticalMenu(menu);
                if (pos == 0)
                {

                }
            }
        }

        private void CreateQuiz()
        {

        }
    }
}