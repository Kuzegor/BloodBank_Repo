using BloodBank.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.ViewModels
{
    internal class MainMenuViewModel : ViewModelBase
    {
        public static MainMenuViewModel Instance { get; } = new MainMenuViewModel();

        private object currentViewModelTwo;
        public object CurrentViewModelTwo
        {
            get { return currentViewModelTwo; }
            set
            {
                currentViewModelTwo = value;
                OnPropertyChanged();
            }
        }

        //ViewModels
        public DonorsViewModel DonorsViewModel { get; set; }
        public RecipientsViewModel RecipientsViewModel { get; set; }
        public DoctorsViewModel DoctorsViewModel { get; set; }
        public RolesViewModel RolesViewModel { get; set; }
        public BloodCollectionViewModel BloodCollectionViewModel { get; set; }
        public IssueViewModel IssueViewModel { get; set; }

        //Commands
        public DelegateCommand OpenAuthorizationCommand { get; set; }
        public DelegateCommand ShowDonorsCommand { get; set; }
        public DelegateCommand ShowRecipientsCommand { get; set; }
        public DelegateCommand ShowDoctorsCommand { get; set; }
        public DelegateCommand ShowRolesCommand { get; set; }
        public DelegateCommand ShowBloodCollectionCommand { get; set; }
        public DelegateCommand ShowIssueCommand { get; set; }


        public MainMenuViewModel()
        {
            DonorsViewModel = new DonorsViewModel();
            RecipientsViewModel = new RecipientsViewModel();
            DoctorsViewModel = new DoctorsViewModel();
            RolesViewModel = new RolesViewModel();
            BloodCollectionViewModel = new BloodCollectionViewModel();
            IssueViewModel = new IssueViewModel();

            CurrentViewModelTwo = BloodCollectionViewModel;

            OpenAuthorizationCommand = new DelegateCommand(x =>
            {
                MainWindowViewModel.Instance.CurrentViewModelOne = MainWindowViewModel.Instance.AuthorizationViewModel;
            });
            ShowDonorsCommand = new DelegateCommand(x =>
            {
                Instance.AllowSelectedItemNull = true;
                Instance.CurrentViewModelTwo = Instance.DonorsViewModel;
                Instance.DonorsViewModel.SelectedEntity = null;
                Instance.AllowSelectedItemNull = false;
            });
            ShowRecipientsCommand = new DelegateCommand(x =>
            {
                Instance.AllowSelectedItemNull = true;
                Instance.CurrentViewModelTwo = Instance.RecipientsViewModel;
                Instance.RecipientsViewModel.SelectedEntity = null;
                Instance.AllowSelectedItemNull = false;
            });
            ShowDoctorsCommand = new DelegateCommand(x =>
            {
                Instance.AllowSelectedItemNull = true;
                Instance.CurrentViewModelTwo = Instance.DoctorsViewModel;
                Instance.DoctorsViewModel.SelectedEntity = null;
                Instance.AllowSelectedItemNull = false;
            });
            ShowRolesCommand = new DelegateCommand(x =>
            {
                Instance.CurrentViewModelTwo = Instance.RolesViewModel;
            });
            ShowBloodCollectionCommand = new DelegateCommand(x =>
            {
                Instance.AllowSelectedItemNull = true;
                Instance.CurrentViewModelTwo = Instance.BloodCollectionViewModel;
                Instance.BloodCollectionViewModel.SelectedEntity = null;
                Instance.AllowSelectedItemNull = false;
            });
            ShowIssueCommand = new DelegateCommand(x =>
            {
                Instance.AllowSelectedItemNull = true;
                Instance.CurrentViewModelTwo = Instance.IssueViewModel;
                Instance.IssueViewModel.SelectedEntity = null; 
                Instance.AllowSelectedItemNull = false;
            });

            AllowSelectedItemNull = false;
        }

        public bool AllowSelectedItemNull { get; set; }
    }
}
