using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ShopConsole.DataAccess;
using ShopConsole.ViewMode;

namespace ShopConsole
{
    /// <summary>
    /// Interaction logic for SaleWindow.xaml
    /// </summary>
    public partial class SaleDialogView : Window
    {
        public SaleDialogView(Pet pet)
        {
            InitializeComponent();
            ViewModeSaleDialogView vm = new ViewModeSaleDialogView(pet);
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }
    }
}
