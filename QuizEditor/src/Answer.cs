using System;

namespace QuizEditor
{
    [Serializable]
    public class Answer
    {
        public string answer { get; set; }
        public bool IsRight { get; set; }

        public override string ToString()
        {
            if (IsRight)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            return $" [*] {answer}   [{IsRight}]";
        }
    }
}