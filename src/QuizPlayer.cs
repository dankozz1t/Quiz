using System;
using System.Collections.Generic;
using NLog;

namespace QuizGame
{
    [Serializable]
    public class CompleteQuiz
    {
        public Quiz PassedQuiz;
        public DateTime DateComplete;
        public int Points;

        public CompleteQuiz() { }
        public CompleteQuiz(Quiz passedQuiz, int points, DateTime dateComplete)
        {
            PassedQuiz = passedQuiz;
            Points = points;
            DateComplete = dateComplete;
        }
    }


    [Serializable]
    public class QuizPlayer : User
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public List<CompleteQuiz> PassedQuizzes = new List<CompleteQuiz>();

        public int TotalPoints;

        public QuizPlayer() { }
        public QuizPlayer(string login, string password, string name, DateTime birthDay, Access access)
        : base(login, password, name, birthDay, access) { }

        public void Show(bool quizInfo = true)
        {
            ConsoleGui.SetPosition(55, Console.CursorTop + 1);
            ConsoleGui.WriteLineColor($"*USER: {Login}*--------------------------------------------------------------------------------------", ConsoleColor.Cyan, true);
            ConsoleGui.SetPosition(55, Console.CursorTop);
            ConsoleGui.WriteLineColor(base.ToString(), ConsoleColor.Cyan, true);
            ConsoleGui.SetPosition(55, Console.CursorTop);
            ConsoleGui.WriteLineColor($"Всего баллов: {TotalPoints} | Выполненных викторин {PassedQuizzes.Count} :", ConsoleColor.Green, true);

            if (quizInfo)
            {
                for (int i = 0; i < PassedQuizzes.Count; i++)
                {
                    ConsoleGui.WriteLineColor("-------------------------------------------------------------", ConsoleColor.Cyan, true);
                    PassedQuizzes[i].PassedQuiz.Show();
                    ConsoleGui.WriteLineColor($"Баллы: {PassedQuizzes[i].Points} / {PassedQuizzes[i].PassedQuiz.questions.Count}   - {PassedQuizzes[i].DateComplete}", ConsoleColor.Yellow, true);
                }

            }
        }

        private void CompletedQuiz(Quiz quiz, int points)
        {
            PassedQuizzes.Add(new CompleteQuiz(quiz, points, DateTime.Now));
    
            TotalPoints += points;
            Logger.Info($" Завершил Викторину \"{quiz.field}\" Набрав {points} / {quiz.questions.Count} баллов. | Login: {Login}");
        }

        private string[] FillMenu(int count, List<Answer> answers) //Заполняет массив стрингов ответами для меню
        {
            string[] menu = new string[count + 1];

            for (int i = 0; i < count; i++)
            {
                menu[i] = " " + answers[i].answer + " ";
            }
            menu[count] = " Отправить ";

            return menu;
        }

        public int Play(Quiz quiz)
        {
            ConsoleGui.SetPosition(48, 1, true);
            ConsoleGui.WriteLineColor("ВИКТОРИНА: " + quiz.field, ConsoleColor.Cyan, true);
            ConsoleGui.WriteLineColor("Количество вопросов: " + quiz.questions.Count, ConsoleColor.Yellow, true);
            ConsoleGui.Wait();

            int scoresThisQuiz = 0;

            for (int i = 0; i < quiz.questions.Count; i++) //По всем вопросам
            {
                int points = 0;
                int positionAnswer = 0;
                var countAnswers = quiz.questions[i].answers.Count;

                string[] menu = FillMenu(countAnswers, quiz.questions[i].answers);

                ConsoleGui.SetPosition(48, 5, true);
                ConsoleGui.WriteLineColor(" [?] " + quiz.questions[i].question, ConsoleColor.Yellow, true);

                List<int> selectedAnswers = new List<int>();

                while (positionAnswer != countAnswers) //Пока не нажата кнопка ОТПРАВИТЬ 
                {
                    ConsoleGui.SetPosition(55, 7);
                    positionAnswer = Menu.VerticalMenu(menu, true);

                    bool unique = false;
                    if (selectedAnswers.Count <= 0)
                        unique = true;
                    else
                        unique = !(selectedAnswers.Contains(positionAnswer));

                    if (unique) //Если Нажатие на ответ УНИКАЛЬНОЕ
                    {
                        selectedAnswers.Add(positionAnswer);
                        menu[positionAnswer] += "*";
                        if (positionAnswer != countAnswers)
                        {
                            if (quiz.questions[i].answers[positionAnswer].IsRight)
                                points++;
                            else
                                points--;
                        }
                    }
                    else
                    {
                        selectedAnswers.Remove(positionAnswer);
                        menu[positionAnswer] = menu[positionAnswer].Replace("*", "");

                        if (positionAnswer != countAnswers)
                        {
                            if (quiz.questions[i].answers[positionAnswer].IsRight)
                                points--;
                            else
                                points++;
                        }
                    }

                    //---------------------Для тестирования
                    ConsoleGui.SetPosition(55, 27);
                    Console.WriteLine($"                                                  ");
                    ConsoleGui.SetPosition(55, 27);
                    Console.WriteLine($" {points} / {quiz.questions[i].CountCorrectAnswers()}");
                }

                if (points == quiz.questions[i].CountCorrectAnswers())
                {
                    scoresThisQuiz++;
                }
            }
            CompletedQuiz(quiz, scoresThisQuiz);
            return scoresThisQuiz;
        }
    }
}