using System;
using System.Linq;
using ShopConsole.DataAccess;

namespace ShopConsole.ViewMode
{
    class ViewModeVipDialogView
    {
        public Action CloseAction  { get; set;}
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }

        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }

        public ViewModeVipDialogView()
        {
            CancelCommand = new DelegateCommand(CloseWindow);
            AddCommand = new DelegateCommand(AddCustomer);
        }

        void CloseWindow(object parameter)
        {
            CloseAction();
        }

        void AddCustomer(object parameter)
        {
            using (DatabaseDataContext db =
                new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                var customers = from cus in db.Customers where cus.CustomerName == CustomerName select cus;
                if (customers.Count() > 1)
                {
                    throw new Exception("Too many Customers");
                }

                var matchedCustomer = customers.FirstOrDefault();
                if (matchedCustomer == null)
                {
                    Customer customer = new Customer
                    {
                        CustomerName = CustomerName,
                        Phone = PhoneNumber,
                        CusStatus = StatusType.None
                    };
                    db.Customers.InsertOnSubmit(customer);
                }
                else
                {
                    matchedCustomer.CusStatus = StatusType.None;
                }
                db.SubmitChanges();
            }
            CloseAction();
        }
    } 
}
