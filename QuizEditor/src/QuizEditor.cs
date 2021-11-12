using System;
using QuizGame;

namespace QuizEditor
{
    public class QuizEditor
    {
        public void Start()
        {
            Console.WriteLine("Редактор Викторин (ТОЛЬКО АДМИНАМ)");

            string[] menu = { "Вход", "Регистрация  ", "Выход" };
            int pos = 0;

            Console.ForegroundColor = ConsoleColor.Red;
            Authorization authorization = new Authorization();
            while (pos != 2)
            {
                ConsoleGui.SetPosition(58, 13, true);
                pos = QuizGame.Menu.VerticalMenu(menu);

                if (pos == 0)
                {
                    if (authorization.SingIN())
                    {
                        if (authorization.IsAdmin())
                        {
                            Menu();
                        }
                        else
                        {
                            ConsoleGui.WriteLineColor("Вход разрешен лишь админам.", ConsoleColor.Cyan, true);
                            ConsoleGui.Wait();
                        }
                    }
                }
                else if (pos == 1)
                {
                    authorization.SingUP();
                }
            }
        }


        private void Menu()
        {
            Console.WriteLine("Главное меню Редактора Викторин");
            Console.ForegroundColor = ConsoleColor.Yellow;

            string[] menu = { "Создать Викторину", "Посмотреть Викторины", "-Редактировать Викторину-", "-Удалить Викторину-", "Выход" };
            int pos = 0;

            while (pos != 4)
            {
                ConsoleGui.SetPosition(48, 13, true);
                pos = QuizGame.Menu.VerticalMenu(menu);
                switch (pos)
                {
                    case 0:
                        Console.Clear();
                        CreateQuiz();
                        break;
                    case 1:
                        Console.Clear();
                        ConsoleGui.SetPosition(37, 2, true);

                        var Quizzes = QuizzesDatabase.GetQuizes();
                        string[] menuQuiz = new string[Quizzes.Count];
                        for (int i = 0; i < Quizzes.Count; i++)
                        {
                            menuQuiz[i] = $"Викторина: {Quizzes[i].name} | Количество Вопросов: {Quizzes[i].questions.Count}";
                        }
                        int posQuiz = QuizGame.Menu.VerticalMenu(menuQuiz);

                        Quizzes[posQuiz].FullShow();

                        ConsoleGui.Wait();
                        break;
                    case 2:
                        ConsoleGui.WriteLineColor("Функция в стадии разработки", ConsoleColor.Red, true);
                        ConsoleGui.Wait();
                        break;
                    case 3:
                        ConsoleGui.WriteLineColor("Функция в стадии разработки", ConsoleColor.Red, true);
                        ConsoleGui.Wait();
                        break;
                }
                QuizzesDatabase.SaveQuizzes();
                Console.ReadLine();
            }
        }

        private void CreateQuiz()
        {
            Quiz quiz = new Quiz();
            quiz.name = ConsoleGui.WhiteReadLine("Введите название Викторины: ", ConsoleColor.Cyan, true);

            ConsoleGui.WriteColor("Введите Количество вопросов Викторины: ", ConsoleColor.Red, true);
            int countQuestion = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < countQuestion; i++)
            {
                ConsoleGui.WriteLineColor($"Введите {i + 1} вопрос: ", ConsoleColor.Yellow, true);
                AddQuestion(quiz);

                Console.WriteLine();
                ConsoleGui.WriteColor($"Введите Количество Ответов на вопрос: ", ConsoleColor.Red, true);
                int countAnswer = Int32.Parse(Console.ReadLine());
                AddAnswers(quiz, countAnswer, i);
            }

            quiz.saveAs = ConsoleGui.WhiteReadLine("Введите имя файла: ", ConsoleColor.Red, true);
            QuizzesDatabase.AddQuiz(quiz);
        }

        private void AddQuestion(Quiz quiz)
        {
            Question tempQuestion = new Question();
            Console.SetCursorPosition(41, Console.CursorTop);
            tempQuestion.question = ConsoleGui.WhiteReadLine("[?] ", ConsoleColor.Yellow);

            quiz.questions.Add(tempQuestion);
        }

        private void AddAnswers(Quiz quiz, int countAnswer, int index)
        {
            string[] menu = { "Неправильный", "Правильный" };

            for (int i = 0; i < countAnswer; i++)
            {
                Answer answer = new Answer();
                ConsoleGui.WriteLineColor($"Введите {i + 1} ответ: ", ConsoleColor.Magenta, true);

                Console.SetCursorPosition(41, Console.CursorTop);
                answer.answer = ConsoleGui.WhiteReadLine("[*] ", ConsoleColor.Magenta);

                Console.SetCursorPosition(41, Console.CursorTop);
                answer.IsRight = Convert.ToBoolean(QuizGame.Menu.VerticalMenu(menu));
                quiz.questions[index].answers.Add(answer);
            }
        }
    }
}