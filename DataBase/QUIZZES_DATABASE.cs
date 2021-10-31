using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace QuizGame
{
    public static class QUIZZES_DATABASE
    {
        static QUIZZES_DATABASE() { }

        private static List<Quiz> quizes = new List<Quiz>();

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

        public static void LoadQuizzes(bool forGame)
        {
            string[] path =null;

            if (forGame)
                path = Directory.GetFiles("../../Save/");
            else
                path = Directory.GetFiles("../../../Save/");


            for (int i = 0; i < path.Length; i++)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Quiz));
                using (FileStream fs = new FileStream(path[i], FileMode.OpenOrCreate))
                {
                    Quiz quizzes = (Quiz)formatter.Deserialize(fs);

                    quizes.Add(quizzes);
                }
            }
        }

        public static void SaveQuizzes()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Quiz));

            foreach (var quiz in quizes)
            {
                string path = "../../../Save/" + quiz.saveAs + ".xml";

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, quiz);
                }
            }
        }
    }
}