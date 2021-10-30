using System.Collections.Generic;

namespace QuizEditor
{
    public class QUIZ_DATABASE
    {
        private QUIZ_DATABASE() { }

        private static List<Quiz> quizes;

        public static List<Quiz> GetQuizes()
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