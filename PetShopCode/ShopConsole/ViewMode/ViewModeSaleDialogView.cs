using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;
using System.Windows;
using ShopConsole.DataAccess;
using MessageBox = System.Windows.MessageBox;

namespace ShopConsole.ViewMode
{
    class ViewModeSaleDialogView : DependencyObject
    {
        public Action CloseAction  { get; set;}
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaleCommand { get; set; }
        public ObservableCollection<Customer> CustomerList { get; set; }

        public Pet SelectPet;
        public string PetName { get; set; }
        public int PetAmount  { get; set; }

        public ViewModeSaleDialogView(Pet pet)
        {
            CancelCommand = new DelegateCommand(CloseWindow);
            SaleCommand = new DelegateCommand(SalePet);
            PetName = pet.PetName;
            SelectPet = pet;
            using (DatabaseDataContext db =
                new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                CustomerList = new ObservableCollection<Customer>((from cus in db.Customers where cus.CusStatus == StatusType.None select cus).ToList());

            } 
        }

        public static readonly DependencyProperty SelectCustomerProperty = DependencyProperty.Register(
            "SelectCustomer", typeof(Customer), typeof(ViewModeSaleDialogView), new PropertyMetadata(null));

        public Customer SelectCustomer
        {
            get { return (Customer) GetValue(SelectCustomerProperty); }
            set { SetValue(SelectCustomerProperty, value); }
        }


        private void CloseWindow(object parameter)
        {
            CloseAction();
        }

        private void SalePet(object paremeter)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (DatabaseDataContext db =
                        new DatabaseDataContext(
                            @"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
                    {
                        var pets = from pe in db.Pets where pe.PetName == PetName select pe;
                        if (pets.Count() > 1)
                        {
                            MessageBox.Show("Too many pets in the list", "Confirm", MessageBoxButton.YesNo);
                            return;
                        }

                        var matchedPet = pets.FirstOrDefault();
                        if (matchedPet != null)
                        {
                            matchedPet.Amount -= PetAmount;
                        }

                        if (matchedPet != null && PetAmount > matchedPet.Amount)
                        {
                            MessageBox.Show("No much pets", "Confirm", MessageBoxButton.OK);
                            return;
                        }

                        if (matchedPet != null && PetAmount <= 0)
                        {
                            MessageBox.Show("Cannot sale pet amount less than 0", "Confirm", MessageBoxButton.OK);
                            return;
                        }

                        var dealRecord = new Deal
                        {
                            Amount = PetAmount,
                            CustomerId = SelectCustomer.Id,
                            PetId = SelectPet.Id,
                            DealData = DateTime.Now
                        };

                        db.Deals.InsertOnSubmit(dealRecord);
                        db.SubmitChanges();
                    }
                    ts.Complete();
                }

                CloseAction();

            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine(@"TransactionAbortedException Message: {0}", ex.Message);
            }
        }
        
    }
}
