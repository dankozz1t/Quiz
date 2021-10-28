namespace Quiz
{
    public enum Access
    {
        USER, ADMIN
    }

    public class User
    {
        public string Login;
        public string Password;
        public string Name;
        public Access Access;

        public User(string login, string password, string name, Access access)
        {
            Login = login.GetHashCode().ToString();
            Password = password.GetHashCode().ToString();
            Name = name;
            Access = access;
        }
    }
}