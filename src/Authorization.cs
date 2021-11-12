using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuizGame
{
    public class Authorization
    {
        public int indexUserNow { get; set; }
        private List<QuizPlayer> QuizPlayers = PlayersDatabase.GetQuizPlayer();

        public bool IsAdmin()
        {
            if (QuizPlayers[indexUserNow].Access == Access.ADMIN)
                return true;
            else
                return false;
        }

        public string IsCorrectInput(string pattern, string text = "")
        {
            Regex regex = new Regex(pattern);

            int attemptCount = Console.CursorTop;
            ConsoleGui.SetPosition(Console.CursorLeft, attemptCount - 1);
            string input = ConsoleGui.WhiteReadLine(text, ConsoleColor.Yellow, true);

            while (true)
            {
                if (regex.IsMatch(input))
                {
                    ConsoleGui.SetPosition(Console.CursorLeft, attemptCount - 2);
                    ConsoleGui.WriteLineColor(" Правильный ввод ", ConsoleColor.Green, true);
                    ConsoleGui.SetPosition(Console.CursorLeft, Console.CursorTop + 3);
                    return input;
                }
                else
                {
                    ConsoleGui.SetPosition(Console.CursorLeft, attemptCount-1);
                    ConsoleGui.WriteLineColor("                                  ", ConsoleColor.Red, true);
                    ConsoleGui.SetPosition(Console.CursorLeft, attemptCount-2);
                    ConsoleGui.WriteLineColor("Неправильный ввод", ConsoleColor.Red, true);
                    input = ConsoleGui.WhiteReadLine(text, ConsoleColor.Yellow, true);
                }
            }
        }

        public bool SingIN()
        {
            try
            {
                ConsoleGui.SetPosition(48, 13, true);
                if (QuizPlayers.Count <= 0)
                {
                    ConsoleGui.WriteLineColor("Еще нет пользователей", ConsoleColor.Cyan, true);
                    ConsoleGui.Wait();
                    LoggerMes.logger.Info($" Неудачная попытка входа. Не существует пользователей.");
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

                        if ((login == "q" && password == "q") && (login == "q" && password == "q"))
                        {
                            LoggerMes.logger.Info($" Выход из меню входа. По кнопке q. ");
                            return false;
                        }

                        for (int i = 0; i < QuizPlayers.Count; i++)
                        {
                            if (QuizPlayers[i].Login == login)
                            {
                                if (QuizPlayers[i].Password == password.GetHashCode().ToString())
                                {
                                    indexUserNow = i;
                                    LoggerMes.logger.Info($" Успешный Вход пользователя. | Login: {login} ");
                                    return true;
                                }
                                else
                                {
                                    ConsoleGui.SetPosition(48, 8, true);
                                    ConsoleGui.WriteLineColor("*Неверный пароль*", ConsoleColor.Red, true);
                                    LoggerMes.logger.Info($" Неверно введенный пароль. | Login: {login} ");
                                }
                            }
                            else
                            {
                                ConsoleGui.SetPosition(48, 8, true);
                                ConsoleGui.WriteLineColor("*Неверный логин*", ConsoleColor.Red, true);
                                LoggerMes.logger.Info($" Неверно введенный логин. | Login: {login} ");
                            }
                        }
                    }
                    ConsoleGui.Wait();
                    LoggerMes.logger.Info($" Неудачный Вход пользователя. | Login: {login} ");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ConsoleGui.WriteLineColor(ex.Message, ConsoleColor.Red, true);
                LoggerMes.logger.Warn($" Словил исключение при Входе. | {ex.Message} ");
                ConsoleGui.Wait();
                return false;
            }

        }

        public void SingUP()
        {
            string PatternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{4,15}$"; // min 5 max 16 | Минимум 1 нижнего регестра, 1 верхнего регестра и 1 цифра (ENG)
            string PatternLogin = @"^[a-zA-Z][a-zA-Z0-9]{4,15}$"; // min 5 max 16 / цифры-букыв без символов(ENG)
            string PatternName = @"^[A-ZА-Я][a-zа-я]+(\-[A-ZА-Я][a-zа-я]+)*$"; //Двойные имена, рус,англ буквы без цифр
            string login = "";

            try
            {
                ConsoleGui.SetPosition(48, 10, true);

                login = IsCorrectInput(PatternLogin, "Введите логин: ");

                for (int i = 0; i < QuizPlayers.Count; i++)
                {
                    if (QuizPlayers[i].Login == login)
                    {
                        i = -1;
                        ConsoleGui.SetPosition(48, 15, true);
                        ConsoleGui.WriteLineColor("Такой логин уже существует!", ConsoleColor.Red, true);
                        LoggerMes.logger.Info($" Попытка регистрации существующего логина | Login: {login} ");
                        ConsoleGui.Wait();

                        ConsoleGui.SetPosition(48, 10, true);
                        login = IsCorrectInput(PatternLogin, "Введите логин [ abcde ]: ");
                    }
                }
                string password = IsCorrectInput(PatternPassword, "Введите пароль [ aBсd1 ]: ");
                string name = IsCorrectInput(PatternName, "Введите имя: ");


                string birthDayS = ConsoleGui.WhiteReadLine("Введите Дату рождения [дд.мм.гггг]: ", ConsoleColor.Yellow, true);

                DateTime birthDay;
                while (!DateTime.TryParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None,
                    out birthDay))
                {
                    ConsoleGui.SetPosition(Console.CursorLeft, Console.CursorTop - 1);
                    ConsoleGui.WriteLineColor("                                                                         ", ConsoleColor.Red, true);

                    ConsoleGui.SetPosition(Console.CursorLeft, Console.CursorTop - 1);
                    birthDayS = ConsoleGui.WhiteReadLine("Введите Дату рождения [дд.мм.гггг]: ", ConsoleColor.Yellow, true);
                }

                ConsoleGui.SetPosition(48, 18);
                ConsoleGui.WriteColor("Введите доступ: ", ConsoleColor.Yellow);
                Access access = (Access)Menu.VerticalMenu(new[] { "USER", "ADMIN" });

                QuizPlayer userReg = new QuizPlayer(login, password, name, birthDay, access);

                PlayersDatabase.AddPlayer(userReg);
                indexUserNow = QuizPlayers.Count - 1;

                LoggerMes.logger.Info($" Регистрация пользователя. | Login: {login} ");
            }
            catch (Exception ex)
            {
                ConsoleGui.WriteLineColor(ex.Message, ConsoleColor.Red, true);
                LoggerMes.logger.Warn($" Словил исключение при регистрации. | {ex.Message} ");
                ConsoleGui.Wait();
            }
        }

    }
}