using System.Collections.Generic;

namespace Quiz
{
    public class USERS_DATABASE
    {
        private USERS_DATABASE() { }

        private static List<User> users;
        public static List<User> Users
        {
            get
            {
                if (users == null)
                    users = new List<User>();
                return users;
            }
            set
            {
                users = value;
            }
        }




    }
}