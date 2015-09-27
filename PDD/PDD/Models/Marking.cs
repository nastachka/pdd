using SQLite;

namespace PDD.Models
{
    internal class Marking : BaseSign
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
