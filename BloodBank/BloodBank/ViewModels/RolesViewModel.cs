using BloodBank.Commands;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BloodBank.ViewModels
{
    class RolesViewModel : ViewModelBase
    {
        private List<RoleModel> RolesList { get; set; }

        private ObservableCollection<RoleModel> roles;
        public ObservableCollection<RoleModel> Roles
        {
            get { return roles; }
            set
            {
                roles = value;
                OnPropertyChanged();
            }
        }

        private RoleModel selectedRole;
        public RoleModel SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged();
            }
        }

        public RolesViewModel()
        {
            RolesList = new List<RoleModel>();
            RolesList = Connector.SqlConnector.GetAllRoles();
            Roles = new ObservableCollection<RoleModel>(RolesList);

            OpenDeleteViewCommand = new DelegateCommand(x =>
            {
                if (SelectedRole != null)
                {
                    MainMenuViewModel.Instance.CurrentViewModelTwo = new RolesDeleteViewModel(SelectedRole);
                }
                else
                {
                    MessageBox.Show("Выберите роль.","Внимание!",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            });
            OpenCreateViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new RolesCreateViewModel();
            });
            OpenEditViewCommand = new DelegateCommand(x =>
            {
                if (SelectedRole != null)
                {
                    MainMenuViewModel.Instance.CurrentViewModelTwo = new RolesCreateViewModel(SelectedRole);
                }
                else
                {
                    MessageBox.Show("Выберите роль.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Connector.SqlConnector.OnRoleDeleted += SqlConnector_OnRoleDeleted;
            Connector.SqlConnector.OnRoleCreated += SqlConnector_OnRoleCreated;
        }

        private void SqlConnector_OnRoleDeleted(object? sender, RoleModel e)
        {
            RolesList.Remove(e);
            Roles = new ObservableCollection<RoleModel>(RolesList);
        }

        private void SqlConnector_OnRoleCreated(object? sender, RoleModel e)
        {
            RolesList.Add(e);
            Roles = new ObservableCollection<RoleModel>(RolesList);
        }

        public DelegateCommand OpenCreateViewCommand { get; set; }
        public DelegateCommand OpenEditViewCommand { get; set; }
        public DelegateCommand OpenDeleteViewCommand { get; set; }
    }
}
