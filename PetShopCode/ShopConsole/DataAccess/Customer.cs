using System.Data.Linq.Mapping;


namespace ShopConsole.DataAccess
{
    [Table(Name = "Customer")]
    public class Customer
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string CustomerName { get; set; }

        [Column]
        public  string Phone { get; set; }

        [Column] 
        public StatusType CusStatus { get; set; }
    }
}
