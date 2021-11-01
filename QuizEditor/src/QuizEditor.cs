using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using QuizGame;

namespace QuizEditor
{
    public class QuizEditor
    {
        public void Start()
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


        private void Menu()
        {
            Console.WriteLine("Главное меню Редактора Викторин");

            string[] menu = { "Создать Викторину", "Посмотреть Викторины", "Редактировать Викторину", "Удалить Викторину", "Выход" };
            int pos = 0;

            QUIZZES_DATABASE.LoadQuizzes(false);
            while (pos != 4)
            {
                pos = QuizGame.Menu.VerticalMenu(menu);
                switch (pos)
                {
                    case 0:
                        CreateQuiz();
                        break;
                    case 1:
                        foreach (var quiz in QUIZZES_DATABASE.GetQuizes())
                        {
                            quiz.FullShow();
                            Console.WriteLine("------------------");
                        }
                        ConsoleGui.Wait();
                        break;
                    case 2: break;
                    case 3: break;
                }
               QUIZZES_DATABASE.SaveQuizzes();
                Console.ReadLine();
            }
        }

        private void CreateQuiz()
        {

            Quiz quiz = new Quiz();

            Console.Write("Введите сферу деятельности викторины: ");
            quiz.field = Console.ReadLine();

            Console.Write("Введите Количество вопросов Викторины: ");
            int countQuestion = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < countQuestion; i++)
            {
                Console.Write($"Введите {i + 1} вопрос: ");
                AddQuestion(quiz);

                Console.Write("Введите Количество Ответов на вопрос: ");
                int countAnswer = Int32.Parse(Console.ReadLine());

                AddAnswers(quiz, countAnswer, i);

            }

            Console.Write("Введите имя файла: ");
            quiz.saveAs = Console.ReadLine();

            QUIZZES_DATABASE.AddQuiz(quiz);
        }

        private void AddQuestion(Quiz quiz)
        {
            Question tempQuestion = new Question();
            tempQuestion.question = Console.ReadLine();

            quiz.questions.Add(tempQuestion);
        }

        private void AddAnswers(Quiz quiz, int countAnswer, int index)
        {
            string[] menu = { "Неправильный", "Правильный" };

            for (int i = 0; i < countAnswer; i++)
            {
                Answer answer = new Answer();
                Console.WriteLine($"Введите {i + 1} ответ: ");

                answer.answer = Console.ReadLine();
                answer.IsRight = Convert.ToBoolean(QuizGame.Menu.VerticalMenu(menu));
                quiz.questions[index].answers.Add(answer);
            }

        }
    }
}