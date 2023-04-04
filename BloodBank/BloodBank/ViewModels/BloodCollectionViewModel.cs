using BloodBank.Commands;
using BloodBankLibrary.Models;
using BloodBankLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.ViewModels
{
    class BloodCollectionViewModel : ViewModelBase
    {
        private List<BloodGroupModel> BloodGroupsList { get; set; }
        private List<DonationTypeModel> DonationTypesList { get; set; }

        private List<BloodModel> EntitiesList { get; set; }

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

        private ObservableCollection<DonationTypeModel> donationTypes;
        public ObservableCollection<DonationTypeModel> DonationTypes
        {
            get { return donationTypes; }
            set
            {
                donationTypes = value;
                OnPropertyChanged();
            }
        }

        private List<ObservableCollection<BloodModel>> Pages { get; set; }

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

        private ObservableCollection<BloodModel> currentPage;
        public ObservableCollection<BloodModel> CurrentPage
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

        private DonationTypeModel selectedDonationType;
        public DonationTypeModel SelectedDonationType
        {
            get { return selectedDonationType; }
            set
            {
                selectedDonationType = value;
                if (EntitiesList.Count > 0)
                {
                    Pages = PopulatePages(EntitiesList);
                    CurrentPage = Pages[0];
                    CurrentPageNumber = 1;
                }
                OnPropertyChanged();
            }
        }

        private BloodModel selectedEntity;
        public BloodModel SelectedEntity
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
                        BloodModel lastEntity = CurrentPage[CurrentPage.Count / 2];
                        Pages = PopulatePages(EntitiesList);
                        foreach (ObservableCollection<BloodModel> page in Pages)
                        {
                            foreach (BloodModel entity in page)
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

        public BloodCollectionViewModel()
        {
            DonationTypesList = new List<DonationTypeModel>();
            BloodGroupsList = new List<BloodGroupModel>();
            EntitiesList = new List<BloodModel>();
            EntitiesList = Connector.SqlConnector.GetBloodCollection();
            BloodGroupsList = GetBloodGroups();
            DonationTypesList = GetDonationTypes();

            foreach (BloodModel item in EntitiesList)
            {
                item.BloodGroupModel = BloodGroupsList.Where(x => x.Id == item.BloodGroup).FirstOrDefault();
                item.DonationTypeModel = DonationTypesList.Where(x => x.Id == item.DonationType).FirstOrDefault();
            }

            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            BloodGroups.Insert(0, new BloodGroupModel { Id = -1, Name = "Все группы крови" });
            SelectedBloodGroup = BloodGroups[0];

            DonationTypes = new ObservableCollection<DonationTypeModel>(DonationTypesList);
            DonationTypes.Insert(0, new DonationTypeModel { Id = -1, Name = "Все виды донаций" });
            SelectedDonationType = DonationTypes[0];

            ItemsPerPage = new ObservableCollection<int> { 10, 30, 50, 100 };
            SelectedItemsPerPage = ItemsPerPage[0];


            if (EntitiesList.Count > 0)
            {
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }

            OpenDeleteViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new BloodCollectionDeleteViewModel(SelectedEntity);
            });
            OpenEditViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new BloodCollectionCreateViewModel(SelectedEntity, DonationTypesList);
            });
            OpenCreateViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new BloodCollectionCreateViewModel(DonationTypesList);
            });
            SearchCommand = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    SelectedBloodGroup = BloodGroups.Where(x => x.Id == -1).FirstOrDefault();
                    List<BloodModel> EntitiesToShow = SearchForEntities(SearchBoxText);
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
                if (EntitiesList.Count > 0 && CurrentPageNumber < Pages.Count)
                {
                    CurrentPageNumber++;
                    CurrentPage = Pages[CurrentPageNumber - 1];
                }
            });
            GoToPreviousPage = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0 && CurrentPageNumber > 1)
                {
                    CurrentPageNumber--;
                    CurrentPage = Pages[CurrentPageNumber - 1];
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
            Connector.SqlConnector.OnBloodCreated += SqlConnector_OnBloodCreated;
            Connector.SqlConnector.OnBloodDeleted += SqlConnector_OnBloodDeleted;
        }
        private void SqlConnector_OnBloodDeleted(object? sender, BloodModel e)
        {
            EntitiesList.Remove(e);
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }
        }
        private void SqlConnector_OnBloodCreated(object? sender, BloodModel e)
        {
            EntitiesList.Add(e);
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[Pages.Count - 1];
                CurrentPageNumber = Pages.Count;
            }
        }

        private List<DonationTypeModel> GetDonationTypes()
        {
            List<DonationTypeModel> output = new List<DonationTypeModel>();

            output.Add(new DonationTypeModel { Id = 1, Name = "Цельная кровь" });
            output.Add(new DonationTypeModel { Id = 2, Name = "Плазма" });
            output.Add(new DonationTypeModel { Id = 3, Name = "Тромбоциты" });
            output.Add(new DonationTypeModel { Id = 4, Name = "Эритроциты" });

            return output;
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
        private List<ObservableCollection<BloodModel>> PopulatePages(List<BloodModel> entitiesInput)
        {
            List<ObservableCollection<BloodModel>> pages = new List<ObservableCollection<BloodModel>>();
            List<BloodModel> entities;

            if (SelectedBloodGroup != null && SelectedBloodGroup != BloodGroups.Where(x => x.Id == -1).FirstOrDefault())
            {
                entities = entitiesInput.Where(x => x.BloodGroup == SelectedBloodGroup.Id).ToList();
            }
            else
            {
                entities = entitiesInput;
            }

            if (SelectedDonationType!= null && SelectedDonationType != DonationTypes.Where(x => x.Id == -1).FirstOrDefault())
            {
                entities = entities.Where(x => x.DonationType == SelectedDonationType.Id).ToList();
            }
            else
            {
                entities = entities;
            }

            int index = 0;
            pages.Add(new ObservableCollection<BloodModel>());
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
                            pages.Add(new ObservableCollection<BloodModel>());
                            pages[index].Add(entities[i]);
                        }
                    }
                }
            }

            return pages;
        }
        private List<BloodModel> SearchForEntities(string searchBoxText)
        {
            List<BloodModel> enitiesToShow = EntitiesList;
            string clippedText = searchBoxText.Trim();
            clippedText += " ";
            string word;
            for (int i = 0; i < clippedText.Length; i++)
            {
                if (clippedText[i] == ' ')
                {
                    word = clippedText.Substring(0, i);
                    enitiesToShow = enitiesToShow.Where(x => x.Donor.Name.Contains(word, StringComparison.OrdinalIgnoreCase)).ToList();

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
