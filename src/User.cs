using System;

namespace QuizGame
{
    public enum Access
    {
        USER, ADMIN
    }

    [Serializable]
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public Access Access { get; set; }

        public User() {}

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
            return $"* Логин: {Login} | Пароль: ***** | Имя: {Name} | Дата рождения : {BirthDay.ToShortDateString()} | Доступ: {Access}";
        }
    }
}