using BloodBank.Commands;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.ViewModels
{
    internal class IssuePrintViewModel : ViewModelBase
    {
        private IssueModel issue;
        public IssueModel Issue
        {
            get { return issue; }
            set { issue = value;
                OnPropertyChanged();
            }
        }

        public IssuePrintViewModel(IssueModel selectedIssue)
        {
            Issue = selectedIssue;
            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.IssueViewModel;
                MainMenuViewModel.Instance.IssueViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
    }
}
