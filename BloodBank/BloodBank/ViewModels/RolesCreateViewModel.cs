using BloodBank.Commands;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BloodBank.ViewModels
{
    class RolesCreateViewModel : ViewModelBase
    {
        private RoleModel role;
        public RoleModel Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged();
            }
        }

        public RolesCreateViewModel()
        {
            Role = new RoleModel();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Role.Name != null)
                {
                    Connector.SqlConnector.CreateRole(Role);

                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RolesViewModel;
                }
                else
                {
                    MessageBox.Show("Полe 'Имя' обязательнo для заполнения.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RolesViewModel;
            });
        }

        public RolesCreateViewModel(RoleModel roleModel)
        {
            Role = roleModel;

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Role.Name != null)
                {
                    Connector.SqlConnector.UpdateRole(Role);

                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RolesViewModel;
                }
                else
                {
                    MessageBox.Show("Полe 'Имя' обязательнo для заполнения.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RolesViewModel;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
    }
}
