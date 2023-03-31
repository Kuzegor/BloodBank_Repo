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
    internal class RecipientsDeleteViewModel :ViewModelBase
    {
        private RecipientModel recipient;

        public RecipientModel Recipient
        {
            get { return recipient; }
            set { recipient = value;
                OnPropertyChanged();
            }
        }

        public RecipientsDeleteViewModel(RecipientModel selectedRecipinent)
        {
            Recipient = selectedRecipinent;

            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RecipientsViewModel;
                MainMenuViewModel.Instance.RecipientsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
            DeleteCommand = new DelegateCommand(x =>
            {
                if (Recipient.Photo != null)
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                    System.IO.File.Delete(strWorkPath + Recipient.Photo);
                }       

                Connector.SqlConnector.DeleteRecipient(Recipient);

                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RecipientsViewModel;
                MainMenuViewModel.Instance.RecipientsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
    }
}
