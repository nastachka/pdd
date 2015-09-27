using SQLite;

namespace PDD.Models
{
    internal class MarkingGroup : BaseGroup
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }
    }
}
