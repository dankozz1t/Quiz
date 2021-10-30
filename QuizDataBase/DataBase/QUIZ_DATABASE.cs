using System.Collections.Generic;

namespace Quiz
{
    public class QUIZ_DATABASE
    {
        private QUIZ_DATABASE() { }

        private static List<Quiz> quizes;

        public static List<Quiz> GetQuiz()
        {
            if (quizes == null)
                quizes = new List<Quiz>();
            return quizes;
        }

        public static void AddUsers(Quiz quiz)
        {
            quizes.Add(quiz);
        }
    }
}