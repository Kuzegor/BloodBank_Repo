using BloodBank.Commands;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BloodBank.ViewModels
{
    internal class RecipientsCreateViewModel : ViewModelBase
    {
        private List<BloodGroupModel> BloodGroupsList { get; set; }

        private ObservableCollection<BloodGroupModel> bloodGroups;
        public ObservableCollection<BloodGroupModel> BloodGroups
        {
            get { return bloodGroups; }
            set
            {
                bloodGroups = value;
                OnPropertyChanged();
            }
        }

        private RecipientModel recipient;
        public RecipientModel Recipient
        {
            get { return recipient; }
            set
            {
                recipient = value;
                OnPropertyChanged();
            }
        }

        private BloodGroupModel selectedBloodGroup;
        public BloodGroupModel SelectedBloodGroup
        {
            get { return selectedBloodGroup; }
            set
            {
                selectedBloodGroup = value;
                Recipient.BloodGroupModel = selectedBloodGroup;
                Recipient.BloodGroup = selectedBloodGroup.Id;
                OnPropertyChanged();
            }
        }

        private string DialogFileName { get; set; }
        private string Destination { get; set; }
        private Uri photoUri;
        public Uri PhotoUri
        {
            get { return photoUri; }
            set
            {
                photoUri = value;
                OnPropertyChanged();
            }
        }
        private Uri DefaultUri
        {
            get
            {
                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                Uri defaultUri = new Uri(strWorkPath + @"\Images\ph.jpg");
                return defaultUri;
            }
        }

        public RecipientsCreateViewModel(RecipientModel selectedRecipient, List<BloodGroupModel> bloodGroupsList)
        {
            BloodGroupsList = bloodGroupsList;
            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            Recipient = selectedRecipient;
            SelectedBloodGroup = BloodGroups.Where(x => x.Id == selectedRecipient.BloodGroup).FirstOrDefault();

            if (Recipient.PhotoUri != DefaultUri)
            {
                PhotoUri = Recipient.PhotoUri;
            }
            else
            {
                PhotoUri = null;
            }

            InitializeCommands();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Recipient.BloodGroup > 0 && Recipient.Name != null)
                {
                    ExecuteImageActions();
                    Connector.SqlConnector.UpdateRecipient(Recipient);

                    MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RecipientsViewModel;
                    MainMenuViewModel.Instance.RecipientsViewModel.SelectedEntity = null;
                    MainMenuViewModel.Instance.AllowSelectedItemNull = false;
                }
                else
                {
                    MessageBox.Show("Поля 'Имя' и 'Группа крови' обязательны для заполнения.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
        public RecipientsCreateViewModel(List<BloodGroupModel> bloodGroupsList)
        {
            BloodGroupsList = bloodGroupsList;
            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            Recipient = new RecipientModel();

            InitializeCommands();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Recipient.BloodGroup > 0 && Recipient.Name != null)
                {
                    ExecuteImageActions();
                    Connector.SqlConnector.CreateRecipient(Recipient);

                    MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RecipientsViewModel;
                    MainMenuViewModel.Instance.RecipientsViewModel.SelectedEntity = null;
                    MainMenuViewModel.Instance.AllowSelectedItemNull = false;
                }
                else
                {
                    MessageBox.Show("Поля 'Имя' и 'Группа крови' обязательны для заполнения.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
        public DelegateCommand UploadImageCommand { get; set; }
        public DelegateCommand DeleteImageCommand { get; set; }

        private void InitializeCommands()
        {
            GoBackCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.RecipientsViewModel;
                MainMenuViewModel.Instance.RecipientsViewModel.SelectedEntity = null;
                MainMenuViewModel.Instance.AllowSelectedItemNull = false;
            });
            UploadImageCommand = new DelegateCommand(x =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image files|*.bpm;*jpg;*.png;*jfif";
                dialog.FilterIndex = 1;
                if (dialog.ShowDialog() == true)
                {
                    FileInfo fi = new FileInfo(dialog.FileName);
                    //fi.Length; //The size of the current file in bytes.file 

                    if (fi.Length <= 25000)
                    {
                        DialogFileName = dialog.FileName;
                        string name = System.IO.Path.GetFileName(DialogFileName);
                        Destination = @"BloodBankImages\Recipients\" + Guid.NewGuid().ToString() + name;
                        PhotoUri = new Uri(DialogFileName);
                    }
                    else
                    {
                        MessageBox.Show("Размер файла не должен быть более 25 KB.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            });
            DeleteImageCommand = new DelegateCommand(x =>
            {
                DialogFileName = null;
                Destination = null;
                PhotoUri = null;
            });
        }

        private void ExecuteImageActions()
        {
            if (PhotoUri == null && Recipient.PhotoUri != DefaultUri)
            {
                DeleteOldImage();
            }
            else if (PhotoUri != null && PhotoUri != Recipient.PhotoUri && Recipient.PhotoUri != DefaultUri)
            {
                DeleteOldImage();
                SaveNewImage();
            }
            else if (PhotoUri != null && Recipient.PhotoUri == DefaultUri)
            {
                SaveNewImage();
            }
        }
        private void DeleteOldImage()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            System.IO.File.Delete(strWorkPath + Recipient.Photo);
            Recipient.Photo = null;
        }
        private void SaveNewImage()
        {
            System.IO.FileInfo file = new System.IO.FileInfo(Destination);
            file.Directory.Create();
            System.IO.File.Copy(DialogFileName, Destination, true);
            Recipient.Photo = @"\" + Destination;
        }
    }
}
