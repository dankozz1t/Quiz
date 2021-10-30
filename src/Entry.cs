//using System;
//using System.Globalization;

//namespace Quiz
//{
//    public class Entry 
//    {

//        public Entry()
//        {
//        }

//        private void SingUP() //Регистрация
//        {
//            string login = "";
//            bool chek = true;

//            Console.Write("Введите логин: ");
//            login = Console.ReadLine();

//            for (int i = 0; i < UsersDatabase.Count; i++)
//            {
//                if (UsersDatabase[i].Login != login)
//                {
//                    continue;
//                }
//                else
//                {
//                    i = -1;
//                    Console.WriteLine("Такой логин уже существует!");
//                    Console.Write("Введите логин: ");
//                    login = Console.ReadLine();
//                }
//            }

//            Console.Write("Введите пароль: ");
//            string password = Console.ReadLine();

//            Console.Write("Введите имя: ");
//            string name = Console.ReadLine();

//            Console.Write("Введите Дату рождения [дд.мм.гггг]: ");
//            string birthDayS = Console.ReadLine();
//            DateTime birthDay = DateTime.ParseExact(birthDayS, "dd.MM.yyyy", CultureInfo.CurrentCulture);

//            Console.Write("Введите доступ: ");
//            string[] menu = { "USER", "ADMIN" };
//            Access access = (Access)Menu.VerticalMenu(menu);

//            User userReg = new User(login, password, name, birthDay, access);

//            USERS_DATABASE.AddUsers(userReg);
//            UserNOW = UsersDatabase[UsersDatabase.Count - 1];
//        }

//        private void SingIN() //Вход
//        {
//            if (UsersDatabase.Count <= 0)
//                Console.WriteLine("Еще нет пользователей");
//            else
//            {
//                bool log = false, pas = false;
//                string login = "", password = "";

//                while (!pas)
//                {
//                    if (!log)
//                    {
//                        Console.Write("Введите логин: ");
//                        login = Console.ReadLine();
//                    }

//                    if (!pas)
//                    {
//                        Console.Write("Введите пароль: ");
//                        password = Console.ReadLine();
//                    }

//                    for (int i = 0; i < UsersDatabase.Count; i++)
//                    {
//                        if (UsersDatabase[i].Login == login)
//                        {
//                            log = true;
//                            if (UsersDatabase[i].Password == password.GetHashCode().ToString())
//                            {
//                                pas = true;
//                                UserNOW = UsersDatabase[i];
//                            }
//                            else
//                            {
//                                Console.WriteLine("Неверный пароль");
//                                pas = false;
//                            }
//                        }
//                        else
//                        {
//                            log = false;
//                            Console.WriteLine("Неверный логин");
//                        }

//                    }

//                }

//                Console.WriteLine("ВХОД УСПЕШЕН");
//            }

//            Console.ReadKey();
//        }





//    }
//}