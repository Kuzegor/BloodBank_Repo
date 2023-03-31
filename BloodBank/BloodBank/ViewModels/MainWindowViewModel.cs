using BloodBank.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BloodBank.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

        private object _currentViewModelOne;
        public object CurrentViewModelOne
        {
            get { return _currentViewModelOne; }
            set
            {
                _currentViewModelOne = value;
                OnPropertyChanged();
            }
        }

        //ViewModels
        public AuthorizationViewModel AuthorizationViewModel { get; set;}
        public MainMenuViewModel MainMenuViewModel { get; set;}

        //Commands
        public DelegateCommand ShutdownCommand { get; set; }
        public DelegateCommand MinimizeCommand { get; set; }
        public DelegateCommand MaximizeCommand { get; set; }
        public DelegateCommand DragMoveCommand { get; set; }

        public MainWindowViewModel()
        {
            AuthorizationViewModel = new AuthorizationViewModel();
            MainMenuViewModel = new MainMenuViewModel();

            CurrentViewModelOne = AuthorizationViewModel;

            ShutdownCommand = new DelegateCommand(x => Application.Current.Shutdown());
            MinimizeCommand = new DelegateCommand(x => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new DelegateCommand(x =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    Application.Current.MainWindow.BorderThickness = new Thickness(0);
                }
                else
                {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    Application.Current.MainWindow.BorderThickness = new Thickness(6); // this line doesn't let the window get bigger than the screen
                }
            });
            DragMoveCommand = new DelegateCommand(x => Application.Current.MainWindow.DragMove());
        }
    }
}
