using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace QuizGame
{
    public class Game
    {
        private List<User> Users = USERS_DATABASE.GetUsers();
        private List<Quiz> Quizzes = QUIZZES_DATABASE.GetQuizes();
        private User UserNOW = new User("dankozz1", "123", "Alex", new DateTime(2003, 02, 12), Access.ADMIN);

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
                    foreach (var user in Users)
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
                        Quizzes[posQuiz].Show();


                        Console.ReadKey();

                        break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                }
            }
        }

        //---------------------------------------------------------РЕГИСТРАЦИЯ 
        private void SingUP()
        {
            string login = "";

            ConsoleGui.SetPosition(48, 10, true);
            ConsoleGui.WriteColor("Введите логин: ", ConsoleColor.Yellow, true);
            login = Console.ReadLine();

            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Login == login)
                {
                    i = -1;
                    ConsoleGui.SetPosition(48, 15, true);
                    ConsoleGui.WriteLineColor("Такой логин уже существует!", ConsoleColor.Red, true);
                    ConsoleGui.Wait();

                    ConsoleGui.SetPosition(48, 10, true);
                    ConsoleGui.WriteColor("Введите логин: ", ConsoleColor.Yellow, true);
                    login = Console.ReadLine();
                }
            }
            ConsoleGui.WriteColor("Введите пароль: ", ConsoleColor.Yellow, true);
            string password = Console.ReadLine();

            ConsoleGui.WriteColor("Введите имя: ", ConsoleColor.Yellow, true);
            string name = Console.ReadLine();

            ConsoleGui.WriteColor("Введите Дату рождения [дд.мм.гггг]: ", ConsoleColor.Yellow, true);
            string birthDayS = Console.ReadLine();
            DateTime birthDay = DateTime.ParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture);

            ConsoleGui.SetPosition(48, 14);
            ConsoleGui.WriteColor("Введите доступ: ", ConsoleColor.Yellow);
            Access access = (Access)Menu.VerticalMenu(new[] { "USER", "ADMIN" });

            User userReg = new User(login, password, name, birthDay, access);

            USERS_DATABASE.AddUser(userReg);
            UserNOW = Users[Users.Count - 1];
        }

        //---------------------------------------------------------ВХОД 
        private bool SingIN()
        {
            ConsoleGui.SetPosition(48, 13, true);
            if (Users.Count <= 0)
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
                    ConsoleGui.WriteColor("Введите логин: ", ConsoleColor.Yellow, true);
                    login = Console.ReadLine();

                    ConsoleGui.SetPosition(48, 11);
                    ConsoleGui.WriteColor("Введите пароль: ", ConsoleColor.Yellow, true);
                    password = Console.ReadLine();

                    if (login == "q")
                    {
                        if (password == "q")
                            return false;
                    }

                    for (int i = 0; i < Users.Count; i++)
                    {
                        if (Users[i].Login == login)
                        {
                            if (Users[i].Password == password.GetHashCode().ToString())
                            {
                                UserNOW = Users[i];
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