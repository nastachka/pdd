using SQLite;

namespace PDD.Models
{
    internal class SignGroup : BaseGroup
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }
    }
}
