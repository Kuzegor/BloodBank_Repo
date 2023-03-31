using BloodBank.Commands;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace BloodBank.ViewModels
{
    internal class DoctorsViewModel : ViewModelBase
    {
        private List<RoleModel> RolesList { get; set; }

        private List<DoctorModel> EntitiesList { get; set; }

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

        private List<ObservableCollection<DoctorModel>> Pages { get; set; }

        private int currentPageNumber;
        public int CurrentPageNumber
        {
            get { return currentPageNumber; }
            set
            {
                if (EntitiesList.Count > 0)
                {
                    if (value < 1)
                    {
                        currentPageNumber = 1;
                    }
                    else if (value > Pages.Count)
                    {
                        currentPageNumber = Pages.Count;
                    }
                    else
                    {
                        currentPageNumber = value;
                    }
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DoctorModel> currentPage;
        public ObservableCollection<DoctorModel> CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
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
                if (EntitiesList.Count > 0)
                {
                    Pages = PopulatePages(EntitiesList);
                    CurrentPage = Pages[0];
                    CurrentPageNumber = 1; 
                }
                OnPropertyChanged();
            }
        }

        private DoctorModel selectedEntity;
        public DoctorModel SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                if (MainMenuViewModel.Instance.AllowSelectedItemNull)
                {
                    selectedEntity = value;
                }
                else
                {
                    if (value != null)
                    {
                        selectedEntity = value;
                    }
                }
                OnPropertyChanged();
            }
        }

        private string searchBoxText;
        public string SearchBoxText
        {
            get { return searchBoxText; }
            set
            {
                searchBoxText = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> itemsPerPage;
        public ObservableCollection<int> ItemsPerPage
        {
            get { return itemsPerPage; }
            set
            {
                itemsPerPage = value;
                OnPropertyChanged();
            }
        }

        private int selectedItemsPerPage;
        public int SelectedItemsPerPage
        {
            get { return selectedItemsPerPage; }
            set
            {
                selectedItemsPerPage = value;

                if (EntitiesList.Count > 0)
                {
                    //this transfers user to the page that contains items from the page that was selected before SelectedItemsPerPage was changed
                    if (CurrentPage.Count > 0)
                    {
                        DoctorModel lastEntity = CurrentPage[CurrentPage.Count / 2];
                        Pages = PopulatePages(EntitiesList);
                        foreach (ObservableCollection<DoctorModel> page in Pages)
                        {
                            foreach (DoctorModel entity in page)
                            {
                                if (entity.Id == lastEntity.Id)
                                {
                                    CurrentPage = page;
                                    CurrentPageNumber = (Pages.FindIndex(x => x.Contains(entity))) + 1;
                                    break;
                                }
                            }
                            if (CurrentPage == page)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Pages = PopulatePages(EntitiesList);
                        CurrentPage = Pages[0];
                        CurrentPageNumber = 1;
                    } 
                }
                OnPropertyChanged();
            }
        }


        public DelegateCommand GoToLastPage { get; set; }
        public DelegateCommand GoToFirstPage { get; set; }
        public DelegateCommand GoToNextPage { get; set; }
        public DelegateCommand GoToPreviousPage { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand GoToSpecificPageCommand { get; set; }
        public DelegateCommand OpenCreateViewCommand { get; set; }
        public DelegateCommand OpenEditViewCommand { get; set; }
        public DelegateCommand OpenDeleteViewCommand { get; set; }

        public DoctorsViewModel()
        {
            RolesList = new List<RoleModel>();
            EntitiesList = new List<DoctorModel>();
            EntitiesList = Connector.SqlConnector.GetAllDoctors(); // TODO: if EntitiesList.Count < 0
            RolesList = Connector.SqlConnector.GetAllRoles();

            Roles = new ObservableCollection<RoleModel>(RolesList);
            Roles.Insert(0, new RoleModel { Id = -1, Name = "Все врачи" });
            SelectedRole = Roles[0];

            ItemsPerPage = new ObservableCollection<int> { 10, 30, 50, 100};
            SelectedItemsPerPage = ItemsPerPage[0];

            if (EntitiesList.Count > 0)
            {
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;                             
            }

            OpenDeleteViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DoctorsDeleteViewModel(SelectedEntity);
            });
            OpenEditViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DoctorsCreateViewModel(SelectedEntity, RolesList);
            });
            OpenCreateViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DoctorsCreateViewModel(RolesList);
            });
            SearchCommand = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    SelectedRole = Roles.Where(x => x.Id == -1).FirstOrDefault();
                    List<DoctorModel> EntitiesToShow = SearchForEntities(SearchBoxText);
                    Pages = PopulatePages(EntitiesToShow);
                    CurrentPage = Pages[0];
                    CurrentPageNumber = 1;
                }
            });
            GoToLastPage = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    CurrentPage = Pages[Pages.Count - 1];
                    CurrentPageNumber = Pages.Count;
                }
            });
            GoToFirstPage = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    CurrentPage = Pages[0];
                    CurrentPageNumber = 1;
                }             
            });
            GoToNextPage = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    if (CurrentPageNumber < Pages.Count)
                    {
                        CurrentPageNumber++;
                        CurrentPage = Pages[CurrentPageNumber - 1];
                    }
                }             
            });
            GoToPreviousPage = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    if (CurrentPageNumber > 1)
                    {
                        CurrentPageNumber--;
                        CurrentPage = Pages[CurrentPageNumber - 1];
                    }
                }             
            });
            GoToSpecificPageCommand = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    CurrentPage = Pages[CurrentPageNumber - 1];
                }
            });
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Connector.SqlConnector.OnDoctorCreated += SqlConnector_OnDoctorCreated;
            Connector.SqlConnector.OnDoctorDeleted += SqlConnector_OnDoctorDeleted;
            Connector.SqlConnector.OnRoleDeleted += SqlConnector_OnRoleDeleted;
            Connector.SqlConnector.OnRoleCreated += SqlConnector_OnRoleCreated;
            Connector.SqlConnector.OnRoleUpdated += SqlConnector_OnRoleUpdated;
        }

        private void SqlConnector_OnDoctorDeleted(object? sender, DoctorModel e)
        {
            EntitiesList.Remove(EntitiesList.Where(x => x.Id == e.Id).FirstOrDefault());
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1; 
            }
        }
        private void SqlConnector_OnDoctorCreated(object? sender, DoctorModel e)
        {
            EntitiesList.Add(e);
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[Pages.Count -1];
                CurrentPageNumber = Pages.Count;
            }
        }
        private void SqlConnector_OnRoleDeleted(object? sender, RoleModel e)
        {
            RolesList.Remove(RolesList.Where(x => x.Id == e.Id).FirstOrDefault());
            Roles = new ObservableCollection<RoleModel>(RolesList);
            Roles.Insert(0, new RoleModel { Id = -1, Name = "Все врачи" });
            SelectedRole = Roles[0];
        }
        private void SqlConnector_OnRoleCreated(object? sender, RoleModel e)
        {
            RolesList.Add(e);
            Roles = new ObservableCollection<RoleModel>(RolesList);
            Roles.Insert(0, new RoleModel { Id = -1, Name = "Все врачи" });
            SelectedRole = Roles[0];
        }
        private void SqlConnector_OnRoleUpdated(object? sender, RoleModel e)
        {
            RolesList.Where(x => x.Id == e.Id).FirstOrDefault().Name = e.Name;
            Roles = new ObservableCollection<RoleModel>(RolesList);
            Roles.Insert(0, new RoleModel { Id = -1, Name = "Все врачи" });
            SelectedRole = Roles[0];

            if (EntitiesList.Count > 0)
            {
                List<DoctorModel> rolesNotNull = EntitiesList.Where(x => x.Role != null).ToList();
                List<DoctorModel> matchingEntities = rolesNotNull.Where(x => x.Role.Id == e.Id).ToList();
                foreach (var item in matchingEntities)
                {
                    item.Role.Name = e.Name;
                }
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1; 
            }
        }

        private List<ObservableCollection<DoctorModel>> PopulatePages(List<DoctorModel> entitiesInput)
        {
            List<ObservableCollection<DoctorModel>> pages = new List<ObservableCollection<DoctorModel>>();
            List<DoctorModel> entities;

            if (SelectedRole != null && SelectedRole != Roles.Where(x => x.Id == -1).FirstOrDefault())
            {
                List<DoctorModel> rolesNotNull = entitiesInput.Where(x => x.Role != null).ToList();
                entities = rolesNotNull.Where(x => x.Role.Id == SelectedRole.Id).ToList();
            }
            else
            {
                entities = entitiesInput;
            }

            int index = 0;
            pages.Add(new ObservableCollection<DoctorModel>());
            if (SelectedItemsPerPage != 0)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (pages[index].Count < SelectedItemsPerPage)
                    {
                        pages[index].Add(entities[i]);
                    }
                    else
                    {
                        index++;
                        if (i != entities.Count)
                        {
                            pages.Add(new ObservableCollection<DoctorModel>());
                            pages[index].Add(entities[i]);
                        }
                    }
                }
            }

            return pages;
        }
        private List<DoctorModel> SearchForEntities(string searchBoxText)
        {
            List<DoctorModel> enitiesToShow = EntitiesList;
            string clippedText = searchBoxText.Trim();
            clippedText += " ";
            string word;
            for (int i = 0; i < clippedText.Length; i++)
            {
                if (clippedText[i] == ' ')
                {
                    word = clippedText.Substring(0, i);
                    enitiesToShow = enitiesToShow.Where(x => x.Name.Contains(word, StringComparison.OrdinalIgnoreCase)).ToList();

                    clippedText = clippedText.Substring(i, clippedText.Length - i);
                    if (clippedText != " ")
                    {
                        clippedText = clippedText.Trim();
                        clippedText += " ";
                    }
                    i = 0;
                }
            }
            return enitiesToShow;
        }
    }
}
