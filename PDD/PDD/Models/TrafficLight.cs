using SQLite;

namespace PDD.Models
{
    internal class TrafficLight : BaseSign
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
