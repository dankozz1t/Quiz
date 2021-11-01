using System;

namespace QuizGame
{
    public static class ConsoleGui
    {
        public static void SetPosition(int left, int top, bool clear = false)
        {
            if (clear)
                Console.Clear();

            Console.SetCursorPosition(left, top);
        }

        public static void Wait(ConsoleColor color = ConsoleColor.Red)
        {
            Console.SetCursorPosition(46, 28);
            WriteLineColor("Нажмите что бы продолжить...", color, true);
            Console.ReadKey();
        }

        public static void WriteLineColor(string str, ConsoleColor color, bool formatter = false)
        {
            if (formatter)
                Console.SetCursorPosition((120 - str.Length) / 2, Console.CursorTop);

            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ResetColor();

        }

        public static void WriteColor(string str, ConsoleColor color, bool formatter = false)
        {
            if (formatter)
                Console.SetCursorPosition((120 - str.Length) / 2, Console.CursorTop);

            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
        }

        public static string WhiteReadLine(string str, ConsoleColor color, bool formatter = false)
        {
            if (formatter)
                Console.SetCursorPosition((120 - str.Length) / 2, Console.CursorTop);

            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();

            return Console.ReadLine();
        }
    }
}