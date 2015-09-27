using SQLite;

namespace PDD.Models
{
    internal class Phone
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ContactId { get; set; }
        public int Type { get; set; }
        public string DisplayName { get; set; }
        public string FormattedPhone { get; set; }
        public string PhoneNumber { get; set; }
    }
}
