using System.Collections.Generic;

namespace QuizGame
{
    public class USERS_DATABASE
    {
        private USERS_DATABASE() { }

        private static List<User> users;

        public static List<User> GetUsers()
        {
            if (users == null)
                users = new List<User>();
            return users;
        }

        public static void AddUsers(User user)
        {
            users.Add(user);
        }

        //public static void RemoveUsers(User user)
        //{
        //    users.Remove(user);
        //}


    }
}