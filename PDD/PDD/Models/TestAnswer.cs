using SQLite;

namespace PDD.Models
{
    internal class TestAnswer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int IsCorrect { get; set; }
    }
}
