using SQLite;

namespace PDD.Models
{
    internal class Malfunction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int GroupId { get; set; }
        public string Text { get; set; }
    }
}
