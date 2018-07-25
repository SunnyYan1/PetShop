using System.Data.Linq;

namespace ShopConsole.DataAccess
{
    class DatabaseDataContext : DataContext
    {
        private Table<Customer> _customers;
        private Table<Pet> _pets;
        private Table<Deal> _deals;

        public ITable<Pet> Pets
        {
            get
            {
                if (_pets == null)
                {
                    return _pets = GetTable<Pet>();
                }
                return _pets;
            }
        }

        public ITable<Customer> Customers
        {
            get
            {
                if (_customers == null)
                {
                    return _customers = GetTable<Customer>();
                }
                return _customers;
            }
        }

        public ITable<Deal> Deals
        {
            get
            {
                if (_deals == null)
                {
                    return _deals = GetTable<Deal>();
                }

                return _deals;
            }
        }

        public DatabaseDataContext(string connection)
            : base(connection)
        {

        }
    }
}
