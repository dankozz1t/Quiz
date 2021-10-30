using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGame
{
    class Program
    {
        static void Main(string[] args)
        {

            Game quiz = new Game();
            quiz.Start();

            //Console.Write("Введите pas1: ");
            //string p1 = Console.ReadLine();

            //Console.Write("Введите pas2: ");
            //string p2 = Console.ReadLine();

            //Console.WriteLine($"Pas1 {p1.GetHashCode().ToString()}");
            //Console.WriteLine($"Pas2 {p2.GetHashCode().ToString()}");

            Console.ReadKey();
        }
    }
}
