using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopConsole.DataAccess
{
    public class DealList
    {
        public string PetName { get; set; }
        public string CustomerName { get; set; }
        public int Amount { get; set; }
        public DateTime DealDate { get; set; }
    }
}
