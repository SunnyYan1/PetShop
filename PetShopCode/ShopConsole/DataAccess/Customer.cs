using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Text;
using System.Windows.Documents;

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
    }
}
