﻿using System;
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

            Console.ReadKey();
        }
    }
}
