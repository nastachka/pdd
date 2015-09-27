using SQLite;

namespace PDD.Models
{
    internal class TrafficLightGroup : BaseGroup
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }
    }
}
