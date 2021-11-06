using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGame
{
    [Serializable]
    public class PlayerStats
    {
        public string PlayersLogin;
        public DateTime DateRecord;
        public int Points;

        public PlayerStats(){}
        public PlayerStats(string playerlogin, int points, DateTime dateRecord)
        {
            PlayersLogin = playerlogin;
            Points = points;
            DateRecord = dateRecord;
        }
    }

    [Serializable]
    public class LeaderBoard
    {
        public List<PlayerStats> PlayerStats = new List<PlayerStats>();

        public LeaderBoard() { }

        public void Show()
        {
            Console.CursorTop++;
            var sortedUsers = PlayerStats.OrderByDescending(u => u.Points);

            foreach (var player in sortedUsers)
            {
                ConsoleGui.WriteLineColor($" {player.PlayersLogin.PadRight(16)}     {player.Points.ToString().PadRight(10)} {player.DateRecord.ToString().PadRight(10)}", ConsoleColor.Cyan, true);
            }

            ConsoleGui.Wait();
        }

        public void AddPlayerToBoard(QuizPlayer player, int points)
        {
            bool unique = false;

            if (PlayerStats.Count <= 0)
                unique = true;

            foreach (var playerStat in PlayerStats)
            {
                if (playerStat.PlayersLogin== player.Login)
                {
                    if (points > playerStat.Points)
                    {
                        playerStat.Points = points;
                        playerStat.DateRecord = DateTime.Now;
                    }
                    unique = false;
                }
                else
                {
                    unique = true;
                }
            }

            if (unique)
            {
                PlayerStats.Add(new PlayerStats(player.Login, points, DateTime.Now));
            }
        }
    }
}