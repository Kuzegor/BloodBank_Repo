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
    internal class DonorsCreateViewModel : ViewModelBase
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

        private DonorModel donor;
        public DonorModel Donor
        {
            get { return donor; }
            set
            {
                donor = value;
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
                Donor.BloodGroupModel = selectedBloodGroup;
                Donor.BloodGroup = selectedBloodGroup.Id;
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

        public DonorsCreateViewModel(DonorModel selectedDonor, List<BloodGroupModel> bloodGroupsList)
        {
            BloodGroupsList = bloodGroupsList;
            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            Donor = selectedDonor;
            SelectedBloodGroup = BloodGroups.Where(x => x.Id == selectedDonor.BloodGroup).FirstOrDefault();

            if (Donor.PhotoUri != DefaultUri)
            {
                PhotoUri = Donor.PhotoUri;
            }
            else
            {
                PhotoUri = null;
            }

            InitializeCommands();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Donor.BloodGroup > 0 && Donor.Name != null)
                {
                    ExecuteImageActions();
                    Connector.SqlConnector.UpdateDonor(Donor);

                    MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DonorsViewModel;
                    MainMenuViewModel.Instance.DonorsViewModel.SelectedEntity = null;
                    MainMenuViewModel.Instance.AllowSelectedItemNull = false;
                }
                else
                {
                    MessageBox.Show("Поля 'Имя' и 'Группа крови' обязательны для заполнения.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
        public DonorsCreateViewModel(List<BloodGroupModel> bloodGroupsList)
        {
            BloodGroupsList = bloodGroupsList;
            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            Donor = new DonorModel();

            InitializeCommands();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Donor.BloodGroup > 0 && Donor.Name != null)
                {
                    ExecuteImageActions();
                    Connector.SqlConnector.CreateDonor(Donor);

                    MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DonorsViewModel;
                    MainMenuViewModel.Instance.DonorsViewModel.SelectedEntity = null;
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
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DonorsViewModel;
                MainMenuViewModel.Instance.DonorsViewModel.SelectedEntity = null;
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

                    if (fi.Length <= 20000)
                    {
                        DialogFileName = dialog.FileName;
                        string name = System.IO.Path.GetFileName(DialogFileName);
                        Destination = @"BloodBankImages\Donors\" + Guid.NewGuid().ToString() + name;
                        PhotoUri = new Uri(DialogFileName);
                    }
                    else
                    {
                        MessageBox.Show("Размер файла не должен быть больше 20 KB.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (PhotoUri == null && Donor.PhotoUri != DefaultUri)
            {
                DeleteOldImage();
            }
            else if (PhotoUri != null && PhotoUri != Donor.PhotoUri && Donor.PhotoUri != DefaultUri)
            {
                DeleteOldImage();
                SaveNewImage();
            }
            else if (PhotoUri != null && Donor.PhotoUri == DefaultUri)
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
            System.IO.File.Delete(strWorkPath + Donor.Photo);
            Donor.Photo = null;
        }
        private void SaveNewImage()
        {
            System.IO.FileInfo file = new System.IO.FileInfo(Destination);
            file.Directory.Create();
            System.IO.File.Copy(DialogFileName, Destination, true);
            Donor.Photo = @"\" + Destination;
        }
    }
}
