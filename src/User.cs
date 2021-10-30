using System;

namespace Quiz
{
    public enum Access
    {
        USER, ADMIN
    }

    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public Access Access { get; set; }


        public User(string login, string password, string name, DateTime birthDay, Access access)
        {
            Login = login;
            Password = password.GetHashCode().ToString();
            Name = name;
            BirthDay = birthDay;
            Access = access;
        }

        public override string ToString()
        {
            return $"log pas : {Login} {Password} | Name: {Name} \nДата рождения : {BirthDay.ToShortDateString()}\n Access: {Access}\n";
        }

    }
}