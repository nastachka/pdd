using SQLite;

namespace PDD.Models
{
    internal class RuleGroup : BaseGroup
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }
    }
}
