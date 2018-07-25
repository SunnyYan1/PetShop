using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using ShopConsole.Annotations;
using ShopConsole.DataAccess;

namespace ShopConsole.ViewMode
{
    public class ViewModeMainWindow : DependencyObject, INotifyPropertyChanged
    {
        public DelegateCommand PurchaseCommand { get; set; }
        public DelegateCommand VipCommand { get; set; }
        public DelegateCommand SaleCommand { get; set; }
        private ObservableCollection<Pet> _petlist;
        private ObservableCollection<Customer> _customerlist;
        private ObservableCollection<DealList> _dealsList;


        public ObservableCollection<Pet> PetsList
        {
            get { return _petlist; }
            set
            {
                _petlist = value;
                OnPropertyChanged(() => PetsList);
            }
        }

        public ObservableCollection<Customer> CustomerList
        {
            get { return _customerlist; }
            set
            {
                _customerlist = value;
                OnPropertyChanged(() =>CustomerList);
            }
        }

        public ObservableCollection<DealList> DealsList
        {
            get { return _dealsList; }
            set
            {
                _dealsList = value;
                OnPropertyChanged(()=>DealsList);
            }
        }

        public ViewModeMainWindow()
        {

            PurchaseCommand = new DelegateCommand(PurchusePets);
            VipCommand = new DelegateCommand(AddCustomers);
            SaleCommand = new DelegateCommand(SalePets);

      
            RefreshData();
        }

        private void RefreshData()
        {
            using (DatabaseDataContext db =
                new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                PetsList = new ObservableCollection<Pet>((from pe in db.Pets select pe).ToList());
                CustomerList = new ObservableCollection<Customer>((from cus in db.Customers select cus).ToList());

                DealsList = new ObservableCollection<DealList>((from deal in db.Deals
                    join pe in db.Pets on deal.PetId equals pe.Id
                    join cus in db.Customers on deal.CustomerId equals cus.Id
                    select new DealList(){CustomerName = cus.CustomerName,PetName = pe.PetName, Amount = deal.Amount,DealDate = deal.DealData}).ToList());
            } 
        }

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "SelectItem", typeof(Pet), typeof(ViewModeMainWindow), new PropertyMetadata(null));

        public Pet SelectItem
        {
            get { return (Pet)GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        private void PurchusePets(object parameter)
        {
            var win = new PurchaseDialogView();
            win.ShowDialog();
            RefreshData();
        }

        private void AddCustomers(object parameter)
        {
            var win = new VIPDialogView();
            win.ShowDialog();
            RefreshData();
        }

        private void SalePets(object parameter)
        {
            using (DatabaseDataContext db =
                new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                var pet = from pe in db.Pets where pe.Id == SelectItem.Id select pe;
                Pet p = pet.FirstOrDefault();
                db.Pets.DeleteOnSubmit(p);
                db.SubmitChanges();
            }
           // var win = new SaleDialogView(SelectItem);
            //win.ShowDialog();
            RefreshData();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged<T>(Expression<Func<T>> path)
        {
            var propertyName = path.Name;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
    
}
