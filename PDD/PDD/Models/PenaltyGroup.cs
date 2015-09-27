using SQLite;

namespace PDD.Models
{
    internal class PenaltyGroup : BaseGroup
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }
    }
}
