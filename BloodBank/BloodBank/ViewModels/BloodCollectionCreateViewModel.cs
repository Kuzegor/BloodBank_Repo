using BloodBank.Commands;
using BloodBank.Interfaces;
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
    internal class BloodCollectionCreateViewModel : ViewModelBase, IDonorsCaller, IDoctorsCaller
    {
        private BloodModel blood;
        public BloodModel Blood
        {
            get { return blood; }
            set { blood = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DonationTypeModel> donationTypes;
        public ObservableCollection<DonationTypeModel> DonationTypes
        {
            get { return donationTypes; }
            set { donationTypes = value;
                OnPropertyChanged();
            }
        }

        private DonationTypeModel selectedDonationType;
        public DonationTypeModel SelectedDonationType
        {
            get { return selectedDonationType; }
            set { selectedDonationType = value;
                Blood.DonationTypeModel = selectedDonationType;
                Blood.DonationType = selectedDonationType.Id;
                OnPropertyChanged();
            }
        }

        private DonorModel selectedDonor;
        public DonorModel SelectedDonor
        {
            get { return selectedDonor; }
            set { selectedDonor = value;
                Blood.Donor = selectedDonor;
                if (selectedDonor != null && selectedDonor.BloodGroupModel != null)
                {
                    SelectedBloodGroup = selectedDonor.BloodGroupModel;
                }
                OnPropertyChanged();
            }
        }

        private BloodGroupModel selectedBloodGroup;
        public BloodGroupModel SelectedBloodGroup
        {
            get { return selectedBloodGroup; }
            set { selectedBloodGroup = value;
                Blood.BloodGroupModel = selectedBloodGroup;
                blood.BloodGroup = selectedBloodGroup.Id;
                OnPropertyChanged();
            }
        }

        private DoctorModel selectedDoctor;
        public DoctorModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set { selectedDoctor = value;
                Blood.DoctorInCharge = selectedDoctor;
                OnPropertyChanged();
            }
        }


        public BloodCollectionCreateViewModel(BloodModel bloodModel, List<DonationTypeModel> donationTypeModels)
        {
            Blood = bloodModel;
            SelectedBloodGroup = bloodModel.BloodGroupModel;
            SelectedDonor = bloodModel.Donor;
            SelectedDoctor = bloodModel.DoctorInCharge;
            DonationTypes = new ObservableCollection<DonationTypeModel>(donationTypeModels);
            SelectedDonationType = DonationTypes.Where(x => x.Id == bloodModel.DonationType).FirstOrDefault();

            InitializeCommands();
            SubmitCommand = new DelegateCommand(x =>
            {
                if (Blood.Amount > 0 && Blood.Unit != null && Blood.DonationType > 0 && Blood.DateOfCollection != null && Blood.BloodGroup > 0 && Blood.DoctorInCharge != null && Blood.Donor != null)
                {
                    Connector.SqlConnector.UpdateBlood(Blood);
                    GoBack();
                }
                else
                {
                    MessageBox.Show("Пожалуйста заполните все поля.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
        public BloodCollectionCreateViewModel( List<DonationTypeModel> donationTypeModels)
        {
            Blood = new BloodModel();
            DonationTypes = new ObservableCollection<DonationTypeModel>(donationTypeModels);

            InitializeCommands();
            SubmitCommand = new DelegateCommand(x =>
            {
                if (Blood.Amount > 0 && Blood.Unit != null && Blood.DonationType > 0 && Blood.DateOfCollection != null && Blood.BloodGroup > 0 && Blood.DoctorInCharge != null && Blood.Donor != null)
                {
                    Connector.SqlConnector.CreateBlood(Blood);
                    GoBack();
                }
                else
                {
                    MessageBox.Show("Пожалуйста заполните все поля.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }

        public DelegateCommand SelectDoctorCommand { get; set; }
        public DelegateCommand SelectDonorCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }

        private void GoBack()
        {
            MainMenuViewModel.Instance.AllowSelectedItemNull = true;
            MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.BloodCollectionViewModel;
            MainMenuViewModel.Instance.BloodCollectionViewModel.SelectedEntity = null;
            MainMenuViewModel.Instance.AllowSelectedItemNull = false;
        }
        private void InitializeCommands()
        {
            SelectDoctorCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DoctorsViewModel(this);
            });
            SelectDonorCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DonorsViewModel(this);
            });
            GoBackCommand = new DelegateCommand(x =>
            {
                GoBack();
            });
        }

        void IDonorsCaller.SelectDonor(DonorModel selectedDonor)
        {
            SelectedDonor = selectedDonor;
        }
        public void SelectDoctor(DoctorModel selectedDoctor)
        {
            SelectedDoctor = selectedDoctor;
        }
    }
}
