using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizGame;
using QuizEditor;

namespace QuizProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            Console.Title = "МЕНЮ";

            GlobalMenu menu = new GlobalMenu();
            menu.Start();
        }
    }
}
