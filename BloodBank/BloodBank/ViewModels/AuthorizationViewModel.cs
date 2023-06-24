using BloodBank.Commands;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BloodBankLibrary;

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
                if (UserName != null && Password != null)
                {
                    SqlConnection sqlcon = new SqlConnection(Connector.GetConnectionString("BloodBank"));
                    string query = "select * from [Authorization] where UserName = '"
                        + UserName.Trim() + "' and Password = '" + Password.Trim() + "'";
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlcon);
                    DataTable dTable1 = new DataTable();
                    sqlda.Fill(dTable1);
                    if (dTable1.Rows.Count > 0)
                    {
                        if (UserName == "admin")
                        {
                            MainWindowViewModel.Instance.CurrentUserIsAdmin = true;
                            MainWindowViewModel.Instance.CurrentViewModelOne = MainWindowViewModel.Instance.MainMenuViewModel;
                        }
                        else
                        {
                            MainWindowViewModel.Instance.CurrentUserIsAdmin = false;
                            MainWindowViewModel.Instance.CurrentViewModelOne = MainWindowViewModel.Instance.MainMenuViewModel;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверно введено имя пользователя или пароль","Неверный пароль",MessageBoxButton.OK,MessageBoxImage.Warning);
                    } 
                }
                else
                {
                    MessageBox.Show("Неверно введено имя пользователя или пароль", "Неверный пароль", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
    }
}
