using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using NLog;

namespace QuizGame
{
    public class Game
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<QuizPlayer> QuizPlayers = PlayersDatabase.GetQuizPlayer();
        private List<Quiz> Quizzes = QuizzesDatabase.GetQuizes();

        private int indexUserNow;
        private static bool unique = true;

        public void Start()
        {
            if (unique)
            {
                PlayersDatabase.LoadUsers();
                QuizzesDatabase.LoadQuizzes();
                unique = false;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            string[] menu = { "  Вход  ", "  Регистрация  ", "  Все пользователи  ", "  Выход  " };
            int pos = 0;
            while (pos != 3)
            {
                ConsoleGui.SetPosition(48, 13, true);

                pos = Menu.VerticalMenu(menu);

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
                    ViewAllUsers();
                }
                PlayersDatabase.SaveUsers();
                QuizzesDatabase.SaveQuizzes();
            }
        }

        private void MenuUser()
        {
            logger.Info($" Вход в меню пользователя. | Login: {QuizPlayers[indexUserNow].Login} ");

            int pos = 0;
            string[] menu = { "  Викторины  ", "  Мои результаты  ", "  Общая статистика  ", "  Настройки  ", "  Выход  " };
            while (pos != 4)
            {
                ConsoleGui.SetPosition(48, 11, true);
                ConsoleGui.WriteLineColor($"    {QuizPlayers[indexUserNow].Name}, привет!", ConsoleColor.Cyan);

                ConsoleGui.SetPosition(48, 13);
                pos = Menu.VerticalMenu(menu);

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

                        logger.Info($" Выбор Викторины \"{Quizzes[posQuiz].field}\". | Login: {QuizPlayers[indexUserNow].Login} ");

                        int points = QuizPlayers[indexUserNow].Play(Quizzes[posQuiz]);
                        Quizzes[posQuiz].leaderBoard.AddPlayerToBoard(QuizPlayers[indexUserNow], points);

                        ConsoleGui.SetPosition(50, 0, true);
                        ConsoleGui.Wait();
                        break;
                    case 1:
                        ConsoleGui.SetPosition(50, 1, true);
                        QuizPlayers[indexUserNow].Show(true);
                        ConsoleGui.Wait();
                        break;
                    case 2:
                        ConsoleGui.SetPosition(37, 2, true);

                        string[] menuQuiz2 = new string[Quizzes.Count];
                        for (int i = 0; i < Quizzes.Count; i++)
                        {
                            menuQuiz2[i] = $"Викторина: {Quizzes[i].field} | Количество Вопросов: {Quizzes[i].questions.Count}";
                        }
                        int posQuiz2 = Menu.VerticalMenu(menuQuiz2);
                        Quizzes[posQuiz2].ShowLeaderBoard();
                        break;
                    case 3:
                        Settings();
                        pos = 4;
                        break;
                }
            }
        }

        private void Settings()
        {
            int pos = 0;
            string[] menu = { "  Изменить пароль  ", "  Изменить дату рождения  ", "  Удалить аккаунт  ", "  Выход  " };
            while (pos != 3)
            {
                ConsoleGui.SetPosition(48, 1, true);
                QuizPlayers[indexUserNow].Show(false);

                ConsoleGui.SetPosition(48, 7);
                pos = Menu.VerticalMenu(menu);

                switch (pos)
                {
                    case 0:
                        string password = ConsoleGui.WhiteReadLine("Введите пароль: ", ConsoleColor.Yellow, true);

                        QuizPlayers[indexUserNow].Password = password.GetHashCode().ToString();

                        ConsoleGui.WriteLineColor($"Пароль изменен на {password}", ConsoleColor.Red, true);
                        ConsoleGui.Wait();
                        break;
                    case 1:
                        try
                        {
                            DateTime oldBirthDay = QuizPlayers[indexUserNow].BirthDay;

                            string birthDayS = ConsoleGui.WhiteReadLine("Введите Дату рождения [дд.мм.гггг]: ", ConsoleColor.Yellow, true);

                            DateTime birthDay = DateTime.ParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture);

                            QuizPlayers[indexUserNow].BirthDay = birthDay;

                            ConsoleGui.WriteLineColor($"Дата рождения изменена с {oldBirthDay.ToShortDateString()} на {birthDay.ToShortDateString()}", ConsoleColor.Red, true);
                            ConsoleGui.Wait();

                        }
                        catch (Exception ex)
                        {
                            ConsoleGui.WriteLineColor(ex.Message, ConsoleColor.Red, true);
                            logger.Warn($" Словил исключение при смене даты рождения. | {ex.Message} ");
                            ConsoleGui.Wait();
                        }
                        break;
                    case 2:
                        string verificationPass = ConsoleGui.WhiteReadLine("Для удаления аккаунта введите свой пароль:", ConsoleColor.Yellow, true);
                        ConsoleGui.WriteLineColor($"Аккаунт { QuizPlayers[indexUserNow].Login} удален.", ConsoleColor.Red, true);
                        PlayersDatabase.RemovePlayer(QuizPlayers[indexUserNow]);
                        //Quizzes[indexUserNow].leaderBoard.PlayerStats.Remove(Quizzes[indexUserNow].leaderBoard.PlayerStats[indexUserNow]);
                        indexUserNow = 0;

                        ConsoleGui.Wait();

                        pos = 3; //Выход
                        break;
                }
            }
        }

        private void ViewAllUsers()
        {
            ConsoleGui.SetPosition(50, 1, true);
            foreach (var user in QuizPlayers)
            {
                user.Show(false);
            }
            ConsoleGui.Wait();
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
                        logger.Info($" Попытка регистрации существующего логина | Login: {login} ");
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

                PlayersDatabase.AddPlayer(userReg);
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