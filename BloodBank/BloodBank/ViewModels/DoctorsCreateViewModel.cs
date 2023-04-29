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
    internal class DoctorsCreateViewModel : ViewModelBase
    {
        private List<RoleModel> RolesList { get; set; }

        private ObservableCollection<RoleModel> roles;
        public ObservableCollection<RoleModel> Roles
        {
            get { return roles; }
            set
            {
                roles = value;
                OnPropertyChanged();
            }
        }

        private DoctorModel doctor;
        public DoctorModel Doctor
        {
            get { return doctor; }
            set { doctor = value;
                OnPropertyChanged();
            }
        }

        private RoleModel selectedRole;
        public RoleModel SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                Doctor.Role = selectedRole;
                OnPropertyChanged();
            }
        }

        private string DialogFileName { get; set; }
        private string Destination { get; set; }
        private Uri photoUri;
        public Uri PhotoUri
        {
            get { return photoUri; }
            set { photoUri = value;
                OnPropertyChanged();
            }
        }
        private Uri DefaultUri
        {
            get {
                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                Uri defaultUri = new Uri(strWorkPath + @"\Images\ph.jpg");
                return defaultUri;
            }
        }


        public DoctorsCreateViewModel(List<RoleModel> rolesList)
        {
            RolesList = rolesList;
            Roles = new ObservableCollection<RoleModel>(RolesList);
            Doctor = new DoctorModel();

            InitializeCommands();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Doctor.Role != null && Doctor.Name != null)
                {
                    ExecuteImageActions();
                    Connector.SqlConnector.CreateDoctor(Doctor);

                    MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DoctorsViewModel;
                    MainMenuViewModel.Instance.DoctorsViewModel.SelectedEntity = null;
                    MainMenuViewModel.Instance.AllowSelectedItemNull = false;
                }
                else
                {
                    MessageBox.Show("Поля 'Имя' и 'Должность' обязательны для заполнения.","Внимание!",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            });
        }
        public DoctorsCreateViewModel(DoctorModel selectedDoctor, List<RoleModel> rolesList)
        {
            RolesList = rolesList;
            Roles = new ObservableCollection<RoleModel>(RolesList);
            Doctor = selectedDoctor;
            if (selectedDoctor.Role != null)
            {
                SelectedRole = Roles.Where(x => x.Id == selectedDoctor.Role.Id).FirstOrDefault();
            }

            if (Doctor.PhotoUri != DefaultUri)
            {
                PhotoUri = Doctor.PhotoUri;
            }
            else
            {
                PhotoUri = null;
            }

            InitializeCommands();

            SubmitCommand = new DelegateCommand(x =>
            {
                if (Doctor.Role != null && Doctor.Name != null)
                {
                    ExecuteImageActions();
                    Connector.SqlConnector.UpdateDoctor(Doctor);

                    MainMenuViewModel.Instance.AllowSelectedItemNull = true;
                    MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DoctorsViewModel;
                    MainMenuViewModel.Instance.DoctorsViewModel.SelectedEntity = null;
                    MainMenuViewModel.Instance.AllowSelectedItemNull = false;
                }
                else
                {
                    MessageBox.Show("Поля 'Имя' и 'Должность' обязательны для заполнения.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MainMenuViewModel.Instance.CurrentViewModelTwo = MainMenuViewModel.Instance.DoctorsViewModel;
                MainMenuViewModel.Instance.DoctorsViewModel.SelectedEntity = null;
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
                        Destination = @"BloodBankImages\Doctors\" + Guid.NewGuid().ToString() + name;
                        PhotoUri = new Uri(DialogFileName);
                    }
                    else
                    {
                        MessageBox.Show("Размер файла не должен быть более 25 KB.","Внимание!",MessageBoxButton.OK,MessageBoxImage.Warning);
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
            if (PhotoUri == null && Doctor.PhotoUri != DefaultUri)
            {
                DeleteOldImage();
            }
            else if (PhotoUri != null && PhotoUri != Doctor.PhotoUri && Doctor.PhotoUri != DefaultUri)
            {
                DeleteOldImage();
                SaveNewImage();
            }
            else if (PhotoUri != null && Doctor.PhotoUri == DefaultUri)
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
            System.IO.File.Delete(strWorkPath + Doctor.Photo);
            Doctor.Photo = null;
        }
        private void SaveNewImage()
        {
            System.IO.FileInfo file = new System.IO.FileInfo(Destination);
            file.Directory.Create();
            System.IO.File.Copy(DialogFileName, Destination, true);
            Doctor.Photo = @"\" + Destination;
        }
    }
}
