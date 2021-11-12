using System;

namespace QuizGame
{
    [Serializable]
    public class Answer
    {
        public string answer { get; set; }
        public bool IsRight { get; set; }
    }
}