using SQLite;

namespace PDD.Models
{
    internal class Law
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }
    }
}
