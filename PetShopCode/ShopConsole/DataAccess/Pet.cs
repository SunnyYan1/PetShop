using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

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
        public int Status { get; set; }

    }

    public enum StatusType
    {
        None,
        Deleted
    }
}
