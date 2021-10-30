using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using QuizEditor;

namespace QuizGame
{
    public class Game
    {
        private List<User> Users = USERS_DATABASE.GetUsers();
        private List<Quiz> Quizzes = QUIZZES_DATABASE.GetQuizes();
        private User UserNOW;

        public void Start()
        {
            Console.WriteLine("Главное меню");
            string[] menu = { "Вход", "Регистрация", "Все пользователи", "Выход" };
            int pos = 0;

            while (pos != 3)
            {
                pos = Menu.VerticalMenu(menu);
                if (pos == 0)
                {
                    SingIN();
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
            LoadQuizzes();
            Console.WriteLine($"{UserNOW.Name}, привет!");

            string[] menu = { "Викторины", "Мои результаты", "Общая статистика", "Настройки", "Выход" };
            int pos = 0;

            while (pos != 4)
            {
                pos = Menu.VerticalMenu(menu);

                switch (pos)
                {
                    case 0:
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


            Console.ReadKey();
        }

        private void LoadQuizzes()
        {
            string[] path = Directory.GetFiles("../../Save/");

            for (int i = 0; i < path.Length; i++)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Quiz));
                using (FileStream fs = new FileStream(path[i], FileMode.OpenOrCreate))
                {
                    Quiz quizzes = (Quiz)formatter.Deserialize(fs);

                    Quizzes.Add(quizzes);
                }
            }
        }

        //---------------------------------------------------------РЕГИСТРАЦИЯ 
        private void SingUP()
        {
            string login = "";
            bool chek = true;

            Console.Write("Введите логин: ");
            login = Console.ReadLine();

            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Login != login)
                {
                    continue;
                }
                else
                {
                    i = -1;
                    Console.WriteLine("Такой логин уже существует!");
                    Console.Write("Введите логин: ");
                    login = Console.ReadLine();
                }
            }

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите Дату рождения [дд.мм.гггг]: ");
            string birthDayS = Console.ReadLine();
            DateTime birthDay = DateTime.ParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture);

            Console.Write("Введите доступ: ");
            string[] menu = { "USER", "ADMIN" };
            Access access = (Access)Menu.VerticalMenu(menu);

            User userReg = new User(login, password, name, birthDay, access);

            USERS_DATABASE.AddUser(userReg);
            UserNOW = Users[Users.Count - 1];
        }

        //---------------------------------------------------------ВХОД 
        private void SingIN()
        {
            if (Users.Count <= 0)
                Console.WriteLine("Еще нет пользователей");
            else
            {
                bool log = false, pas = false;
                string login = "", password = "";

                while (!pas)
                {
                    if (!log)
                    {
                        Console.Write("Введите логин: ");
                        login = Console.ReadLine();
                    }

                    if (!pas)
                    {
                        Console.Write("Введите пароль: ");
                        password = Console.ReadLine();
                    }

                    for (int i = 0; i < Users.Count; i++)
                    {
                        if (Users[i].Login == login)
                        {
                            log = true;
                            if (Users[i].Password == password.GetHashCode().ToString())
                            {
                                pas = true;
                                UserNOW = Users[i];
                            }
                            else
                            {
                                Console.WriteLine("Неверный пароль");
                                pas = false;
                            }
                        }
                        else
                        {
                            log = false;
                            Console.WriteLine("Неверный логин");
                        }

                    }

                }

                Console.WriteLine("ВХОД УСПЕШЕН");
            }

            Console.ReadKey();
        }
    }
}