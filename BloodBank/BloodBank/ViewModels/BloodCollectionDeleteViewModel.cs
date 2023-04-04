using BloodBank.Commands;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.ViewModels
{
    internal class BloodCollectionDeleteViewModel : ViewModelBase
    {
        private BloodModel blood;
        public BloodModel Blood
        {
            get { return blood; }
            set { blood = value;
                OnPropertyChanged();
            }
        }

        public BloodCollectionDeleteViewModel(BloodModel bloodModel)
        {
            Blood = bloodModel;

            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.BloodCollectionViewModel;
                MainMenuViewModel.Instance.BloodCollectionViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
            DeleteCommand = new DelegateCommand(x =>
            {
                Connector.SqlConnector.DeleteBlood(Blood);

                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.BloodCollectionViewModel;
                MainMenuViewModel.Instance.BloodCollectionViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
    }
}
