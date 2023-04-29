using BloodBank.Commands;
using BloodBank.Interfaces;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace BloodBank.ViewModels
{
    internal class IssueCreateViewModel : ViewModelBase, IRecipientsCaller, IBloodCollectionCaller, IDoctorsCaller
    {
		private IssueModel issue;
		public IssueModel Issue
		{
			get { return issue; }
			set { issue = value;
                OnPropertyChanged();
            }
		}

        private RecipientModel selectedRecipient;
        public RecipientModel SelectedRecipient
        {
            get { return selectedRecipient; }
            set { selectedRecipient = value;
                Issue.Recipient = selectedRecipient;
                OnPropertyChanged();
            }
        }

        private double InitialMaxBlood;
        private double InitiallySelectedIssueBloodAmount;
        private BloodModel InitiallySelectedBlood;

        private BloodModel selectedBlood;
        public BloodModel SelectedBlood
        {
            get { return selectedBlood; }
            set
            {
                MaxBlood = default(Double);
                selectedBlood = value;                
                Issue.Blood = selectedBlood;
                Issue.Unit = selectedBlood.Unit;
                Issue.BloodGroupModel = selectedBlood.BloodGroupModel;
                Issue.DonationTypeModel = selectedBlood.DonationTypeModel;
                OnPropertyChanged();
            }
        }

        private DoctorModel selectedDoctor;
        public DoctorModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                Issue.DoctorInCharge = selectedDoctor;
                OnPropertyChanged();
            }
        }

        private double bloodAmount;
        public double BloodAmount
        {
            get { return bloodAmount; }
            set
            {
                if (SelectedBlood != null && value > MaxBlood)
                {
                    bloodAmount = MaxBlood;
                }
                else if (value >= 0)
                {
                    bloodAmount = value;
                }
                Issue.BloodAmount = bloodAmount;
                OnPropertyChanged();
            }
        }

        private double maxBlood;
        public double MaxBlood
        {
            get 
           {
                if (SelectedBlood != null)
                {
                    return maxBlood + SelectedBlood.Amount;
                }
                else
                {
                    return maxBlood;
                }
            }
            set { maxBlood = value;
                OnPropertyChanged();
            }
        }


        public IssueCreateViewModel(IssueModel selectedIssue)
		{
			Issue = selectedIssue;
            SelectedRecipient = selectedIssue.Recipient;

            selectedIssue.Blood.BloodGroupModel = selectedIssue.BloodGroupModel;
            selectedIssue.Blood.DonationTypeModel = selectedIssue.DonationTypeModel;
            SelectedBlood = selectedIssue.Blood;
            InitiallySelectedBlood = selectedIssue.Blood;

            MaxBlood = selectedIssue.BloodAmount;
            InitialMaxBlood = MaxBlood;
            InitiallySelectedIssueBloodAmount = selectedIssue.BloodAmount;

            SelectedDoctor = selectedIssue.DoctorInCharge;
            BloodAmount = selectedIssue.BloodAmount;

            InitializeCommands();
            SubmitCommand = new DelegateCommand(x =>
            {
                if (Issue.Recipient != null && Issue.Blood != null && Issue.BloodAmount > 0 && Issue.DateOfIssue != null && Issue.DoctorInCharge != null && Issue.BloodAmount <= MaxBlood)
                {
                    Connector.SqlConnector.UpdateIssue(Issue);
                    if (SelectedBlood.Id != InitiallySelectedBlood.Id)
                    {
                        Connector.SqlConnector.UpdateBloodAmount(InitialMaxBlood, InitiallySelectedBlood.Id);
                    }
                    Connector.SqlConnector.UpdateBloodAmount(MaxBlood - Issue.BloodAmount, SelectedBlood.Id);
                    GoBack();
                }
                else
                {
                    MessageBox.Show("Пожалуйста заполните все поля.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }

        public IssueCreateViewModel()
		{
			Issue = new IssueModel();

            InitializeCommands();
            SubmitCommand = new DelegateCommand(x =>
            {
                if (Issue.Recipient != null && Issue.Blood != null && Issue.BloodAmount > 0 && Issue.DateOfIssue != null && Issue.DoctorInCharge != null && Issue.BloodAmount <= MaxBlood)
                {
                    Connector.SqlConnector.CreateIssue(Issue);
                    Connector.SqlConnector.UpdateBloodAmount(MaxBlood - Issue.BloodAmount, SelectedBlood.Id);
                    GoBack();
                }
                else
                {
                    MessageBox.Show("Пожалуйста заполните все поля.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }

        public DelegateCommand SelectRecipientCommand { get; set; }
        public DelegateCommand SelectDoctorCommand { get; set; }
        public DelegateCommand SelectBloodCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }

        private void GoBack()
        {
            MainMenuViewModel.Instance.AllowSelectedItemNull = true;
            MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.IssueViewModel;
            MainMenuViewModel.Instance.IssueViewModel.SelectedEntity = null;
            MainMenuViewModel.Instance.AllowSelectedItemNull = false;
        }
        private void InitializeCommands()
        {
            GoBackCommand = new DelegateCommand(x =>
            {
                SelectedBlood = InitiallySelectedBlood;
                MaxBlood = InitiallySelectedIssueBloodAmount;
                BloodAmount = InitiallySelectedIssueBloodAmount;
                GoBack();
            });
            SelectDoctorCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DoctorsViewModel(this);
            });
            SelectBloodCommand = new DelegateCommand(x =>
            {
                if (SelectedRecipient != null)
                {
                    if (SelectedBlood != null)
                    {
                        MainMenuViewModel.Instance.CurrentViewModelTwo = new BloodCollectionViewModel(this, SelectedRecipient.BloodGroup, SelectedBlood.Id);
                    }
                    else
                    {
                        MainMenuViewModel.Instance.CurrentViewModelTwo = new BloodCollectionViewModel(this, SelectedRecipient.BloodGroup);
                    }               
                }
                else
                {
                    MainMenuViewModel.Instance.CurrentViewModelTwo = new BloodCollectionViewModel(this, 0);
                }
            });
            SelectRecipientCommand = new DelegateCommand(x =>
            {
                if (SelectedBlood != null)
                {
                    MainMenuViewModel.Instance.CurrentViewModelTwo = new RecipientsViewModel(this, SelectedBlood.BloodGroup);
                }
                else
                {
                    MainMenuViewModel.Instance.CurrentViewModelTwo = new RecipientsViewModel(this, 0);
                }
            });
        }

        public void SelectRecipient(RecipientModel selectedRecipient)
        {
            SelectedRecipient = selectedRecipient;
        }
        public void SelectBlood(BloodModel selectedBlood)
        {
            SelectedBlood = selectedBlood;
        }
        public void SelectDoctor(DoctorModel selectedDoctor)
        {
            SelectedDoctor = selectedDoctor;
        }
    }
}
