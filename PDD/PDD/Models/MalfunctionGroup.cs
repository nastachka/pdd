using SQLite;

namespace PDD.Models
{
    internal class MalfunctionGroup : BaseGroup
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }
    }
}
