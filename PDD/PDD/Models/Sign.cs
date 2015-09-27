using SQLite;

namespace PDD.Models
{
    internal class Sign : BaseSign
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
