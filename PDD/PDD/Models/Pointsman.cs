using SQLite;

namespace PDD.Models
{
    internal class Pointsman : BaseSign
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
