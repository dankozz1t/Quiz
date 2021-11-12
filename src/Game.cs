using System;
using System.Collections.Generic;
using System.Globalization;

namespace QuizGame
{
    public class Game
    {
        private List<QuizPlayer> QuizPlayers = PlayersDatabase.GetQuizPlayer();
        private List<Quiz> Quizzes = QuizzesDatabase.GetQuizes();
        private int indexUserNow;
        
        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string[] menu = { "  Вход  ", "  Регистрация  ", "  Все пользователи  ", "  Выход  " };
            int pos = 0;
            Authorization authorization = new Authorization();
            while (pos != 3)
            {
                ConsoleGui.SetPosition(48, 13, true);

                pos = Menu.VerticalMenu(menu);

                if (pos == 0)
                {
                    if (authorization.SingIN())
                    {
                        indexUserNow = authorization.indexUserNow;
                        MenuUser();
                    }
                }
                else if (pos == 1)
                {
                    authorization.SingUP();
                    indexUserNow = authorization.indexUserNow;
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
            LoggerMes.logger.Info($" Вход в меню пользователя. | Login: {QuizPlayers[indexUserNow].Login} ");

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
                        LoggerMes.logger.Info($" Вход в Викторины. | Login: {QuizPlayers[indexUserNow].Login} ");

                        ConsoleGui.SetPosition(37, 2, true);

                        string[] menuQuiz = new string[Quizzes.Count];
                        for (int i = 0; i < Quizzes.Count; i++)
                        {
                            menuQuiz[i] = $"Викторина: {Quizzes[i].name} | Количество Вопросов: {Quizzes[i].questions.Count}";
                        }
                        int posQuiz = Menu.VerticalMenu(menuQuiz);

                        LoggerMes.logger.Info($" Выбор Викторины \"{Quizzes[posQuiz].name}\". | Login: {QuizPlayers[indexUserNow].Login} ");

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
                            menuQuiz2[i] = $"Викторина: {Quizzes[i].name} | Количество Вопросов: {Quizzes[i].questions.Count}";
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
                            LoggerMes.logger.Warn($" Словил исключение при смене даты рождения. | {ex.Message} ");
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
    }
}