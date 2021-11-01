using System.Collections.Generic;

namespace QuizGame
{
    public class QuizPlayerDatabase
    {
        private QuizPlayerDatabase() { }

        private static List<QuizPlayer> quizPlayers;

        public static List<QuizPlayer> GetQuizPlayer()
        {
            if (quizPlayers == null)
                quizPlayers = new List<QuizPlayer>();
            return quizPlayers;
        }

        public static void AddQuizPlayer(QuizPlayer user)
        {
            quizPlayers.Add(user);
        }

    }
}