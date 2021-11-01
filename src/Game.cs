using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace QuizGame
{
    public class Game
    {
        private List<QuizPlayer> QuizPlayers = QuizPlayerDatabase.GetQuizPlayer();
        private List<Quiz> Quizzes = QUIZZES_DATABASE.GetQuizes();
        private QuizPlayer UserNOW = new QuizPlayer("dankozz1", "123", "Alex", new DateTime(2003, 02, 12), Access.ADMIN);

        public void Start()
        {
            ConsoleGui.SetPosition(44, 11, true);
            ConsoleGui.WriteColor("ГЛАВНОЕ МЕНЮ ИГРЫ <ВИКТОРИНА>", ConsoleColor.Cyan, true);

            int pos = 0;
            while (pos != 3)
            {
                ConsoleGui.SetPosition(48, 13, true);
                pos = Menu.VerticalMenu(new[] { "  Вход  ", "  Регистрация  ", "  Все пользователи  ", "  Выход  " });
                if (pos == 0)
                {
                    //if (SingIN())
                    MenuUser();
                }
                else if (pos == 1)
                {
                    SingUP();
                }
                else if (pos == 2)
                {
                    foreach (var user in QuizPlayers)
                    {
                        Console.WriteLine(user);
                    }

                    Console.ReadKey();
                }
            }
        }

        private void MenuUser()
        {
            QUIZZES_DATABASE.LoadQuizzes(true);
            Console.Clear();
            ConsoleGui.SetPosition(48, 11, true);
            ConsoleGui.WriteLineColor($"    {UserNOW.Name}, привет!", ConsoleColor.Cyan);

            int pos = 0;

            while (pos != 4)
            {
                ConsoleGui.SetPosition(50, 13, false);
                pos = Menu.VerticalMenu(new[] { "  Викторины  ", "  Мои результаты  ", "  Общая статистика  ", "  Настройки  ", "  Выход  " });

                switch (pos)
                {
                    case 0:
                        ConsoleGui.SetPosition(37, 2, true);
                        string[] menuQuiz = new string[Quizzes.Count];
                        for (int i = 0; i < Quizzes.Count; i++)
                        {
                            menuQuiz[i] = $"Викторина: {Quizzes[i].field} | Количество Вопросов: {Quizzes[i].questions.Count}";
                        }
                        int posQuiz = Menu.VerticalMenu(menuQuiz);

                        Console.WriteLine();
                        UserNOW.Play(Quizzes[posQuiz]);
                        UserNOW.Show();


                        Console.ReadKey();

                        break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                }
            }
        }

       

        private bool Unique()
        {
            return false;
        }

        //---------------------------------------------------------РЕГИСТРАЦИЯ 
        private void SingUP()
        {
            string login = "";

            ConsoleGui.SetPosition(48, 10, true);
            login = ConsoleGui.WhiteReadLine("Введите логин: ", ConsoleColor.Yellow, true);

            for (int i = 0; i < QuizPlayers.Count; i++)
            {
                if (QuizPlayers[i].Login == login)
                {
                    i = -1;
                    ConsoleGui.SetPosition(48, 15, true);
                    ConsoleGui.WriteLineColor("Такой логин уже существует!", ConsoleColor.Red, true);
                    ConsoleGui.Wait();

                    ConsoleGui.SetPosition(48, 10, true);
                    login = ConsoleGui.WhiteReadLine("Введите логин: ", ConsoleColor.Yellow, true);
                }
            }
            string password = ConsoleGui.WhiteReadLine("Введите пароль: ", ConsoleColor.Yellow, true);
            string name = ConsoleGui.WhiteReadLine("Введите имя: ", ConsoleColor.Yellow, true);

            string birthDayS = ConsoleGui.WhiteReadLine("Введите Дату рождения [дд.мм.гггг]: ", ConsoleColor.Yellow, true);
            DateTime birthDay = DateTime.ParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture);

            ConsoleGui.SetPosition(48, 14);
            ConsoleGui.WriteColor("Введите доступ: ", ConsoleColor.Yellow);
            Access access = (Access)Menu.VerticalMenu(new[] { "USER", "ADMIN" });

            QuizPlayer userReg = new QuizPlayer(login, password, name, birthDay, access);

            QuizPlayerDatabase.AddQuizPlayer(userReg);
            UserNOW = QuizPlayers[QuizPlayers.Count - 1];
        }

        //---------------------------------------------------------ВХОД 
        private bool SingIN()
        {
            ConsoleGui.SetPosition(48, 13, true);
            if (QuizPlayers.Count <= 0)
            {

                ConsoleGui.WriteLineColor("Еще нет пользователей", ConsoleColor.Cyan, true);
                ConsoleGui.Wait();
                return false;
            }
            else
            {
                string login = "", password = "";

                Console.Clear();
                while (login != "q" || password != "q")
                {
                    ConsoleGui.SetPosition(48, 28);
                    ConsoleGui.WriteColor("log and pas -'q' = Выход", ConsoleColor.Red, true);

                    ConsoleGui.SetPosition(48, 10);
                    login = ConsoleGui.WhiteReadLine("Введите логин: ", ConsoleColor.Yellow, true);

                    ConsoleGui.SetPosition(48, 11);
                    password = ConsoleGui.WhiteReadLine("Введите пароль: ", ConsoleColor.Yellow, true);

                    if (login == "q")
                    {
                        if (password == "q")
                            return false;
                    }

                    for (int i = 0; i < QuizPlayers.Count; i++)
                    {
                        if (QuizPlayers[i].Login == login)
                        {
                            if (QuizPlayers[i].Password == password.GetHashCode().ToString())
                            {
                                UserNOW = QuizPlayers[i];
                                return true;
                            }
                            else
                            {
                                ConsoleGui.SetPosition(48, 8, true);
                                ConsoleGui.WriteLineColor("*Неверный пароль*", ConsoleColor.Red, true);
                            }
                        }
                        else
                        {
                            ConsoleGui.SetPosition(48, 8, true);
                            ConsoleGui.WriteLineColor("*Неверный логин*", ConsoleColor.Red, true);
                        }
                    }
                }
                ConsoleGui.Wait();
                return false;
            }
        }
    }
}