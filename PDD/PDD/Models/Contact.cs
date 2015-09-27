using SQLite;

namespace PDD.Models
{
    internal class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
    }
}
