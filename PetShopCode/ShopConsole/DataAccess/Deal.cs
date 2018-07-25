using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace ShopConsole.DataAccess
{
    [Table(Name = "Deal")]
    public class Deal
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public int PetId { get; set; }

        [Column]
        public int CustomerId { get; set; }

        [Column]
        public DateTime DealData { get; set; }

        [Column]
        public int Amount { get; set; }

    }
}
