using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;



namespace QuizGame
{
    public static class QuizzesDatabase
    {
        static QuizzesDatabase() { }

        private static List<Quiz> Quizes = new List<Quiz>();

        public static List<Quiz> GetQuizes()
        {
            if (Quizes == null)
                Quizes = new List<Quiz>();
            return Quizes;
        }

        public static void AddQuiz(Quiz quiz)
        {
            Quizes.Add(quiz);
        }

        public static void RemoveQuiz(Quiz quiz)
        {
            Quizes.Remove(quiz);
        }

        public static void LoadQuizzes()
        {
            string[] path = Directory.GetFiles("../../../Save/Quizzes/");

            for (int i = 0; i < path.Length; i++)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Quiz));
                using (FileStream fs = new FileStream(path[i], FileMode.OpenOrCreate))
                {
                    Quiz quizzes = (Quiz)formatter.Deserialize(fs);

                    Quizes.Add(quizzes);
                }
            }
        }

        public static void SaveQuizzes()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Quiz));

            foreach (var quiz in Quizes)
            {
                string path = "../../../Save/Quizzes/" + quiz.saveAs + ".xml";

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, quiz);
                }
            }
        }
    }
}