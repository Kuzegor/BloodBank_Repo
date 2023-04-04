using BloodBank.Commands;
using BloodBankLibrary.Models;
using BloodBankLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodBank.Interfaces;

namespace BloodBank.ViewModels
{
    internal class DonorsViewModel : ViewModelBase
    {
        private List<BloodGroupModel> BloodGroupsList { get; set; }

        private List<DonorModel> EntitiesList { get; set; }

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

        private List<ObservableCollection<DonorModel>> Pages { get; set; }

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

        private ObservableCollection<DonorModel> currentPage;
        public ObservableCollection<DonorModel> CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
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
                if (EntitiesList.Count > 0)
                {
                    Pages = PopulatePages(EntitiesList);
                    CurrentPage = Pages[0];
                    CurrentPageNumber = 1; 
                }
                OnPropertyChanged();
            }
        }

        private DonorModel selectedEntity;
        public DonorModel SelectedEntity
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
                        DonorModel lastEntity = CurrentPage[CurrentPage.Count / 2];
                        Pages = PopulatePages(EntitiesList);
                        foreach (ObservableCollection<DonorModel> page in Pages)
                        {
                            foreach (DonorModel entity in page)
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
        public DelegateCommand SelectDonorCommand { get; set; }

        public DonorsViewModel()
        {
            InitializeEverything();
            OpenCreateViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DonorsCreateViewModel(BloodGroupsList);
            });
            OpenDeleteViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DonorsDeleteViewModel(SelectedEntity);
            });
            OpenEditViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new DonorsCreateViewModel(SelectedEntity, BloodGroupsList);
            });
            SubscribeToEvents();
        }

        private IDonorsCaller DonorsCaller { get; set; }
        public DonorsViewModel(IDonorsCaller donorsCaller)
        {
            InitializeEverything();
            DonorsCaller = donorsCaller;
            SelectDonorCommand = new DelegateCommand(x =>
            {
                DonorsCaller.SelectDonor(SelectedEntity);
                MainMenuViewModel.Instance.CurrentViewModelTwo = DonorsCaller;
            });
        }

        private void InitializeEverything()
        {
            BloodGroupsList = new List<BloodGroupModel>();
            EntitiesList = new List<DonorModel>();
            EntitiesList = Connector.SqlConnector.GetAllDonors();
            BloodGroupsList = GetBloodGroups();

            foreach (DonorModel item in EntitiesList)
            {
                item.BloodGroupModel = BloodGroupsList.Where(x => x.Id == item.BloodGroup).FirstOrDefault();
            }

            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            BloodGroups.Insert(0, new BloodGroupModel { Id = -1, Name = "Все группы крови" });
            SelectedBloodGroup = BloodGroups[0];

            ItemsPerPage = new ObservableCollection<int> { 10, 30, 50, 100 };
            SelectedItemsPerPage = ItemsPerPage[0];


            if (EntitiesList.Count > 0)
            {
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }

            SearchCommand = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    SelectedBloodGroup = BloodGroups.Where(x => x.Id == -1).FirstOrDefault();
                    List<DonorModel> EntitiesToShow = SearchForEntities(SearchBoxText);
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
        }
        private void SubscribeToEvents()
        {
            Connector.SqlConnector.OnDonorCreated += SqlConnector_OnDonorCreated;
            Connector.SqlConnector.OnDonorDeleted += SqlConnector_OnDonorDeleted;
        }
        private void SqlConnector_OnDonorDeleted(object? sender, DonorModel e)
        {
            EntitiesList.Remove(e);
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1; 
            }
        }
        private void SqlConnector_OnDonorCreated(object? sender, DonorModel e)
        {
            EntitiesList.Add(e);
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[Pages.Count - 1];
                CurrentPageNumber = Pages.Count;
            }
        }

        private List<BloodGroupModel> GetBloodGroups()
        {
            List<BloodGroupModel> output = new List<BloodGroupModel>();

            output.Add(new BloodGroupModel { Id = 1, Name = "O+" });
            output.Add(new BloodGroupModel { Id = 2, Name = "O-" });
            output.Add(new BloodGroupModel { Id = 3, Name = "A+" });
            output.Add(new BloodGroupModel { Id = 4, Name = "A-" });
            output.Add(new BloodGroupModel { Id = 5, Name = "B+" });
            output.Add(new BloodGroupModel { Id = 6, Name = "B-" });
            output.Add(new BloodGroupModel { Id = 7, Name = "AB+" });
            output.Add(new BloodGroupModel { Id = 8, Name = "AB-" });

            return output;
        }
        private List<ObservableCollection<DonorModel>> PopulatePages(List<DonorModel> entitiesInput)
        {
            List<ObservableCollection<DonorModel>> pages = new List<ObservableCollection<DonorModel>>();
            List<DonorModel> entities;

            if (SelectedBloodGroup != null && SelectedBloodGroup != BloodGroups.Where(x => x.Id == -1).FirstOrDefault())
            {
                entities = entitiesInput.Where(x => x.BloodGroup == SelectedBloodGroup.Id).ToList();
            }
            else
            {
                entities = entitiesInput;
            }

            int index = 0;
            pages.Add(new ObservableCollection<DonorModel>());
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
                            pages.Add(new ObservableCollection<DonorModel>());
                            pages[index].Add(entities[i]);
                        }
                    }
                }
            }

            return pages;
        }
        private List<DonorModel> SearchForEntities(string searchBoxText)
        {
            List<DonorModel> enitiesToShow = EntitiesList;
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
