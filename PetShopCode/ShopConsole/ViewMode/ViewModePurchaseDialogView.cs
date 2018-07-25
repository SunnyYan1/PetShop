using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ShopConsole.DataAccess;

namespace ShopConsole.ViewMode
{
    class ViewModePurchaseDialogView : Window
    {
        public Action CloseAction  { get; set;}
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand PurchasePetCommand { get; set; }


        public string PetName { get; set; }
        public int PetAmount { get; set; }


        public ViewModePurchaseDialogView()
        {
            CancelCommand = new DelegateCommand(CloseWindow);
            PurchasePetCommand = new DelegateCommand(PurchasePet);
        }

        void CloseWindow(object parameter)
        {
            CloseAction();
        }

        private void PurchasePet(object parameter)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(@"Server=10.30.169.46;Database=PetShop;User Id=sunny;Password=~123qwerty;"))
            {
                var pets = from pe in db.Pets where pe.PetName == PetName select pe;
                if (pets.Count() > 1)
                {
                    throw new Exception("Too many pets");
                }

                var matchedPet = pets.FirstOrDefault();
                if (matchedPet == null)
                {
                    Pet pet = new Pet
                    {
                        PetName = PetName,
                        Amount = PetAmount
                    };
                    db.Pets.InsertOnSubmit(pet);
                }
                else
                {
                    matchedPet.Amount += PetAmount;
                }
                db.SubmitChanges();
            }
            CloseAction();
            
        }
    }    
}
