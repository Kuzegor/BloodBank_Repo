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
    class RolesDeleteViewModel
    {
        public RoleModel Role { get; set; }
        public RolesDeleteViewModel(RoleModel roleModel)
        {
            Role = roleModel;

            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RolesViewModel;
            });
            DeleteCommand = new DelegateCommand(x =>
            {
                Connector.SqlConnector.DeleteRole(Role);

                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RolesViewModel;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
    }
}
