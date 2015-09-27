using SQLite;

namespace PDD.Models
{
    internal class PenaltyArticle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PenaltyGroupId { get; set; }
        public string Text { get; set; }
    }
}
