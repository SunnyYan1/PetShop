using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Customer customer = new Customer
            {
                CustomerName = CustomerName,
                Phone = PhoneNumber
            };

            using (DatabaseDataContext db =
                new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                db.Customers.InsertOnSubmit(customer);
                db.SubmitChanges();
            }
            CloseAction();

        }
    }
    
}
