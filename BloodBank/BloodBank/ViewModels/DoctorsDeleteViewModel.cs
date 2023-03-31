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
    internal class DoctorsDeleteViewModel : ViewModelBase
    {
        private DoctorModel doctor;
        public DoctorModel Doctor
        {
            get { return doctor; }
            set { doctor = value;
                OnPropertyChanged();
            }
        }

        public DoctorsDeleteViewModel(DoctorModel selectedDoctor)
        {
            Doctor = selectedDoctor;

            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DoctorsViewModel;
                MainMenuViewModel.Instance.DoctorsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
            DeleteCommand = new DelegateCommand(x =>
            {
                if (Doctor.Photo != null)
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                    System.IO.File.Delete(strWorkPath + Doctor.Photo);
                }

                Connector.SqlConnector.DeleteDoctor(Doctor);

                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DoctorsViewModel;
                MainMenuViewModel.Instance.DoctorsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
    }
}
