using System.Data.Linq.Mapping;


namespace ShopConsole.DataAccess
{
    [Table(Name = "Pet")]
    public class Pet
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string PetName { get; set; }

        [Column]
        public int Amount { get; set; }

        [Column]
        public StatusType Status { get; set; }

    }

    public enum StatusType
    {
        None = 0,
        Deleted = 1
    }
}
