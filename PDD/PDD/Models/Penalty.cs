using SQLite;

namespace PDD.Models
{
    internal class Penalty
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PenaltyArticleId { get; set; }
        public string Text { get; set; }
        public string Fine { get; set; }
    }
}
