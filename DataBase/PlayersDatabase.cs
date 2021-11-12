using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace QuizGame
{
    public class PlayersDatabase
    {
        private PlayersDatabase() { }

        private static List<QuizPlayer> Players = new List<QuizPlayer>();

        public static List<QuizPlayer> GetQuizPlayer()
        {
            if (Players == null)
                Players = new List<QuizPlayer>();
            return Players;
        }

        public static void AddPlayer(QuizPlayer user)
        {
            Players.Add(user);
        }

        public static void RemovePlayer(QuizPlayer user)
        {
            Players.Remove(user);
        }


        public static void LoadUsers()
        {
            string[] path = Directory.GetFiles("../../../Save/Users/");

            for (int i = 0; i < path.Length; i++)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(QuizPlayer));
                using (FileStream fs = new FileStream(path[i], FileMode.OpenOrCreate))
                {
                    QuizPlayer players = (QuizPlayer)formatter.Deserialize(fs);

                    Players.Add(players);
                }
            }
        }

        public static void SaveUsers()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(QuizPlayer));

            foreach (var player in Players)
            {
                string path = "../../../Save/Users/" + player.Login + ".xml";

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, player);
                }
            }
        }
    }
}