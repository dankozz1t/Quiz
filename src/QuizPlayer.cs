using System;
using System.Collections.Generic;
using NLog;

namespace QuizGame
{
    public class QuizPlayer : User
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private List<Quiz> passedQuizzes = new List<Quiz>();
        public List<int> points = new List<int>();
        public int totalPoints;

        public QuizPlayer(string login, string password, string name, DateTime birthDay, Access access) : base(login, password, name, birthDay, access)
        { }

        public void Show()
        {
            Console.WriteLine(base.ToString());
            Console.WriteLine($"Выполненных викторин {passedQuizzes.Count} :");

            for (int i = 0; i < passedQuizzes.Count; i++)
            {
                Console.WriteLine(passedQuizzes[i]);
                Console.WriteLine($"Баллы: {points[i]} / {passedQuizzes[i].questions.Count}");
            }
        }

        private void CompletedQuiz(Quiz quiz, int points)
        {
            passedQuizzes.Add(quiz);
            this.points.Add(points);
            totalPoints += points;
            logger.Info($" Завершил Викторину \"{quiz.field}\" Набрав {points} / {quiz.questions.Count} баллов. | Login: {Login}");

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

        public void Play(Quiz quiz)
        {
            ConsoleGui.SetPosition(48, 1, true);
            ConsoleGui.WriteColor("ВИКТОРИНА: " + quiz.field, ConsoleColor.Cyan, true);
            ConsoleGui.WriteColor("Количество вопросов: " + quiz.questions.Count, ConsoleColor.Yellow, true);


            int scoresThisQuiz = 0;

            for (int i = 0; i < quiz.questions.Count; i++) //По всем вопросам
            {
                Console.Clear();
                int points = 0;
                int positionAnswer = 0;
                var countAnswers = quiz.questions[i].answers.Count;

                string[] menu = FillMenu(countAnswers, quiz.questions[i].answers);

                ConsoleGui.SetPosition(48, 3 + i);
                ConsoleGui.WriteLineColor(" [?] " + quiz.questions[i].question, ConsoleColor.Yellow, true);

                List<int> selectedAnswers = new List<int>();
                while (positionAnswer != countAnswers) //Пока не нажата кнопка ОТПРАВИТЬ 
                {
                    ConsoleGui.SetPosition(55, 5 + i);
                    positionAnswer = Menu.VerticalMenu(menu, true);

                    bool unique = false;
                    if (selectedAnswers.Count <= 0)
                        unique = true;
                    else
                        unique = !(selectedAnswers.Contains(positionAnswer));

                    if (unique)
                    {
                        selectedAnswers.Add(positionAnswer);
                        menu[positionAnswer] += "*";
                        if (positionAnswer != countAnswers) //Если не нажата кнопка ОТПРАВИТЬ
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

                        if (positionAnswer != countAnswers) //Если не нажата кнопка ОТПРАВИТЬ
                        {
                            if (quiz.questions[i].answers[positionAnswer].IsRight)
                                points--;
                            else
                                points++;
                        }
                    }
                    
                    ConsoleGui.SetPosition(55, 27);
                    Console.WriteLine($"                                                  ");
                    ConsoleGui.SetPosition(55, 27);
                    Console.WriteLine($" {points} / {quiz.questions[i].CountCorrectAnswers()}");
                }

                if (points == quiz.questions[i].CountCorrectAnswers())
                {
                    Console.WriteLine(" Все ответы правильные + 1 балл ");
                    Console.WriteLine($" {points} / {quiz.questions[i].CountCorrectAnswers()}");
                    scoresThisQuiz++;
                }
                else
                {
                    Console.WriteLine("хзхзхз");
                    Console.WriteLine($" {points} / {quiz.questions[i].CountCorrectAnswers()}");
                }
            }

            CompletedQuiz(quiz, scoresThisQuiz);
        }
    }
}