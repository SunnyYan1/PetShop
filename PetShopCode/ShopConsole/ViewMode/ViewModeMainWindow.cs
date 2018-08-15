using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using log4net;
using ShopConsole.Annotations;
using ShopConsole.DataAccess;
using MessageBox = System.Windows.Forms.MessageBox;
using log4net.Config;

//[assembly:log4net.Config.XmlConfigurator(Watch = true)]
namespace ShopConsole.ViewMode
{
    public class ViewModeMainWindow : DependencyObject, INotifyPropertyChanged
    {
        public DelegateCommand PurchaseCommand { get; set; }
        public DelegateCommand VipCommand { get; set; }
        public DelegateCommand SaleCommand { get; set; }
        public DelegateCommand DeleteCusCommand { get; set; }
        public DelegateCommand DeletePetCommand { get; set; }
        private ObservableCollection<Pet> _petlist;
        private ObservableCollection<Customer> _customerlist;
        private ObservableCollection<DealList> _dealsList;
        private readonly ILog _log = log4net.LogManager.GetLogger("Test1");

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
            DeleteCusCommand = new DelegateCommand(DeleteCustomer);
            DeletePetCommand = new DelegateCommand(DeletePet);
            BasicConfigurator.Configure();

      
            RefreshData();
        }

        private void DeletePet(object obj)
        {
            const string message = "Are you sure you want to delete this pet";
            const string caption = "Delete Pet";
            DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (DatabaseDataContext db =
                    new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
                {
                    var pet = from pe in db.Pets where pe.Id == SelectItem.Id select pe;
                    Pet p = pet.FirstOrDefault();
                    if (p != null)
                    {
                        p.Status = StatusType.Deleted;
                        p.Amount = 0;
                    }
                    db.SubmitChanges();
                }
                RefreshData();                
            }
        }

        private void DeleteCustomer(object obj)
        {
            const string message = "Are you sure you want to delete this customer";
            const string caption = "Delete Customer";
            DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (DatabaseDataContext db =
                    new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
                {
                    var customers = from cus in db.Customers where cus.Id == SelectCustomer.Id select cus;
                    Customer customer = customers.FirstOrDefault();
                    if (customer != null)
                    {
                        customer.CusStatus = StatusType.Deleted;
                        customer.Phone = "";
                    }

                    db.SubmitChanges();
                }

                RefreshData();
            }
        }

        private void RefreshData()
        {
            using (DatabaseDataContext db =
                new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                PetsList = new ObservableCollection<Pet>((from pe in db.Pets where pe.Status == StatusType.None select pe).ToList());
                CustomerList = new ObservableCollection<Customer>((from cus in db.Customers where cus.CusStatus == StatusType.None select cus).ToList());

                DealsList = new ObservableCollection<DealList>((from deal in db.Deals
                    join pe in db.Pets on deal.PetId equals pe.Id
                    join cus in db.Customers on deal.CustomerId equals cus.Id
                    select new DealList(){CustomerName = cus.CustomerName,PetName = pe.PetName, Amount = deal.Amount,DealDate = deal.DealData}).ToList());
            } 
        }

        public static readonly DependencyProperty SelectPetProperty = DependencyProperty.Register(
            "SelectItem", typeof(Pet), typeof(ViewModeMainWindow), new PropertyMetadata(null));

        public Pet SelectItem
        {
            get { return (Pet)GetValue(SelectPetProperty); }
            set { SetValue(SelectPetProperty, value); }
        }

        public static readonly DependencyProperty SelectCusromerProperty = DependencyProperty.Register(
            "SelectCusItem", typeof(Customer), typeof(ViewModeMainWindow), new PropertyMetadata(null));

        public Customer SelectCustomer
        {
            get { return (Customer)GetValue(SelectCusromerProperty); }
            set { SetValue(SelectCusromerProperty, value); }
        }

        private void PurchusePets(object parameter)
        {   
            _log.Info("Where is this log");
            ViewModePurchaseDialogView model = new ViewModePurchaseDialogView();
            var purchaseDialog = new PurchaseDialogView{DataContext = model};
            model.CloseAction = () => purchaseDialog.Close();
            purchaseDialog.ShowDialog();
            RefreshData();
        }

        private void AddCustomers(object parameter)
        {
            ViewModeVipDialogView model = new ViewModeVipDialogView();
            var customerDialog = new VIPDialogView{DataContext = model};
            model.CloseAction = () => customerDialog.Close();
            customerDialog.ShowDialog();
            RefreshData();
        }

        private void SalePets(object parameter)
        {
            ViewModeSaleDialogView model = new ViewModeSaleDialogView(SelectItem);
            var saleDialog = new SaleDialogView{DataContext = model};
            model.CloseAction = () => saleDialog.Close();
            saleDialog.ShowDialog();
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
