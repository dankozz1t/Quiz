using System.Collections.Generic;

namespace QuizEditor
{
    public class QUIZZES_DATABASE
    {
        private QUIZZES_DATABASE() { }

        private static List<Quiz> quizes;

        public static List<Quiz> GetQuizes()
        {
            if (quizes == null)
                quizes = new List<Quiz>();
            return quizes;
        }

        public static void AddQuiz(Quiz quiz)
        {
            quizes.Add(quiz);
        }
    }
}