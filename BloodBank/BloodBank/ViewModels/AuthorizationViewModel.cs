using BloodBank.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.ViewModels
{
    internal class AuthorizationViewModel : ViewModelBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value;
                OnPropertyChanged();
            }
        }


        public DelegateCommand OpenMainMenuCommand { get; set; }

        public AuthorizationViewModel()
        {
            OpenMainMenuCommand = new DelegateCommand(x =>
            {
                MainWindowViewModel.Instance.CurrentViewModelOne = MainWindowViewModel.Instance.MainMenuViewModel;
            });
        }
    }
}
