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
    internal class DonorsDeleteViewModel :ViewModelBase
    {
        private DonorModel donor;
        public DonorModel Donor
        {
            get { return donor; }
            set { donor = value;
                OnPropertyChanged();
            }
        }

        public DonorsDeleteViewModel(DonorModel donorModelInput)
        {
            Donor = donorModelInput;

            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DonorsViewModel;
                MainMenuViewModel.Instance.DonorsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
            DeleteCommand = new DelegateCommand(x =>
            {
                if (Donor.Photo != null)
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                    System.IO.File.Delete(strWorkPath + Donor.Photo);
                }               

                Connector.SqlConnector.DeleteDonor(Donor);

                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DonorsViewModel;
                MainMenuViewModel.Instance.DonorsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
    }
}
