using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NLog;

namespace QuizGame
{
    public class Game
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<QuizPlayer> QuizPlayers = QuizPlayerDatabase.GetQuizPlayer();
        private List<Quiz> Quizzes = QUIZZES_DATABASE.GetQuizes();
        private int indexUserNow;


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
                    if (SingIN())
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
            ConsoleGui.WriteLineColor($"    {QuizPlayers[indexUserNow].Name}, привет!", ConsoleColor.Cyan);
            logger.Info($" Вход в меню пользователя. | Login: {QuizPlayers[indexUserNow].Login} ");

            int pos = 0;

            while (pos != 4)
            {
                ConsoleGui.SetPosition(50, 13, false);
                pos = Menu.VerticalMenu(new[] { "  Викторины  ", "  Мои результаты  ", "  Общая статистика  ", "  Настройки  ", "  Выход  " });

                switch (pos)
                {
                    case 0:
                        logger.Info($" Вход в Викторины. | Login: {QuizPlayers[indexUserNow].Login} ");

                        ConsoleGui.SetPosition(37, 2, true);
                        string[] menuQuiz = new string[Quizzes.Count];
                        for (int i = 0; i < Quizzes.Count; i++)
                        {
                            menuQuiz[i] = $"Викторина: {Quizzes[i].field} | Количество Вопросов: {Quizzes[i].questions.Count}";
                        }
                        int posQuiz = Menu.VerticalMenu(menuQuiz);

                        Console.WriteLine();
                        logger.Info($" Выбор Викторины \"{Quizzes[posQuiz].field}\". | Login: {QuizPlayers[indexUserNow].Login} ");

                        QuizPlayers[indexUserNow].Play(Quizzes[posQuiz]);
                        QuizPlayers[indexUserNow].Show();


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

            try
            {

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

                string birthDayS =
                    ConsoleGui.WhiteReadLine("Введите Дату рождения [дд.мм.гггг]: ", ConsoleColor.Yellow, true);
                DateTime birthDay = DateTime.ParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture);

                ConsoleGui.SetPosition(48, 14);
                ConsoleGui.WriteColor("Введите доступ: ", ConsoleColor.Yellow);
                Access access = (Access)Menu.VerticalMenu(new[] { "USER", "ADMIN" });

                QuizPlayer userReg = new QuizPlayer(login, password, name, birthDay, access);

                QuizPlayerDatabase.AddQuizPlayer(userReg);
                indexUserNow = QuizPlayers.Count - 1;

                logger.Info($" Регистрация пользователя. | Login: {login} ");
            }
            catch (Exception ex)
            {
                ConsoleGui.WriteLineColor(ex.Message, ConsoleColor.Red, true);
                logger.Warn($" Словил исключение при регистрации. | {ex.Message} ");
                ConsoleGui.Wait();
            }
        }

        //---------------------------------------------------------ВХОД 
        private bool SingIN()
        {
            try
            {
                ConsoleGui.SetPosition(48, 13, true);
                if (QuizPlayers.Count <= 0)
                {
                    ConsoleGui.WriteLineColor("Еще нет пользователей", ConsoleColor.Cyan, true);
                    ConsoleGui.Wait();
                    logger.Info($" Неудачная попытка входа. Не существует пользователей.");
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
                            {
                                logger.Info($" Выход из меню входа. По кнопке q. ");
                                return false;
                            }
                        }

                        for (int i = 0; i < QuizPlayers.Count; i++)
                        {
                            if (QuizPlayers[i].Login == login)
                            {
                                if (QuizPlayers[i].Password == password.GetHashCode().ToString())
                                {
                                    indexUserNow = i;
                                    logger.Info($" Успешный Вход пользователя. | Login: {login} ");
                                    return true;
                                }
                                else
                                {
                                    ConsoleGui.SetPosition(48, 8, true);
                                    ConsoleGui.WriteLineColor("*Неверный пароль*", ConsoleColor.Red, true);
                                    logger.Info($" Неверно введенный пароль. | Login: {login} ");
                                }
                            }
                            else
                            {
                                ConsoleGui.SetPosition(48, 8, true);
                                ConsoleGui.WriteLineColor("*Неверный логин*", ConsoleColor.Red, true);
                                logger.Info($" Неверно введенный логин. | Login: {login} ");
                            }
                        }
                    }
                    ConsoleGui.Wait();
                    logger.Info($" Неудачный Вход пользователя. | Login: {login} ");
                    return false;

                }
            }
            catch (Exception ex)
            {
                ConsoleGui.WriteLineColor(ex.Message, ConsoleColor.Red, true);
                logger.Warn($" Словил исключение при Входе. | {ex.Message} ");
                ConsoleGui.Wait();
                return false;
            }
        }
    }
}