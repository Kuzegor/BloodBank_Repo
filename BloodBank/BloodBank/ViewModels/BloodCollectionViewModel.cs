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
    class BloodCollectionViewModel : ViewModelBase
    {
        private List<BloodGroupModel> BloodGroupsList { get; set; }
        private List<DonationTypeModel> DonationTypesList { get; set; }
        private List<BloodGroupModel> StatusesList { get; set; }


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

        private ObservableCollection<BloodGroupModel> statuses;
        public ObservableCollection<BloodGroupModel> Statuses
        {
            get { return statuses; }
            set
            {
                statuses = value;
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

        private BloodGroupModel selectedStatus;
        public BloodGroupModel SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
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
        public DelegateCommand SelectBloodCommand { get; set; }

        public BloodCollectionViewModel()
        {
            InitializeEverything();
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
            SubscribeToEvents();
        }

        private IBloodCollectionCaller BloodCollectionCaller { get; set; }
        public BloodCollectionViewModel(IBloodCollectionCaller bloodCollectionCaller, int bloodGroupId, int bloodId = -2)
        {
            InitializeEverything();
            BloodCollectionCaller = bloodCollectionCaller;
            SelectBloodCommand = new DelegateCommand(x =>
            {
                BloodCollectionCaller.SelectBlood(SelectedEntity);
                MainMenuViewModel.Instance.CurrentViewModelTwo = BloodCollectionCaller;
            });
            if (bloodId > -2)
            {
                EntitiesList.Remove(EntitiesList.Where(x => x.Id == bloodId).FirstOrDefault());
            }
            EntitiesList = EntitiesList.Where(x => x.Amount > 0).ToList();

            FilterBloodCollectionByRecipientsBloodGroup(bloodGroupId);

            Statuses.RemoveAt(2);
            SelectedStatus = Statuses[1];
            Statuses.RemoveAt(0);
        }

        /// <summary>
        /// Фильтрует список крови, оставляя лишь кровь совместимую с кровью реципиента
        /// </summary>
        /// <param name="bloodGroupId"></param>
        private void FilterBloodCollectionByRecipientsBloodGroup(int bloodGroupId)
        {
            switch (bloodGroupId)
            {
                case 1: //O+
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 1 || x.BloodGroup == 2).ToList();
                    BloodGroups.RemoveAt(8);
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(6);
                    BloodGroups.RemoveAt(5);
                    BloodGroups.RemoveAt(4);
                    BloodGroups.RemoveAt(3);
                    break;
                case 2: //O-
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 2).ToList();
                    BloodGroups.RemoveAt(8);
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(6);
                    BloodGroups.RemoveAt(5);
                    BloodGroups.RemoveAt(4);
                    BloodGroups.RemoveAt(3);
                    BloodGroups.RemoveAt(1);
                    break;
                case 3: //A+
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 1 || x.BloodGroup == 2 || x.BloodGroup == 3 || x.BloodGroup == 4).ToList();
                    BloodGroups.RemoveAt(8);
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(6);
                    BloodGroups.RemoveAt(5);
                    break;
                case 4: //A-
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 2 || x.BloodGroup == 4).ToList();
                    BloodGroups.RemoveAt(8);
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(6);
                    BloodGroups.RemoveAt(5);
                    BloodGroups.RemoveAt(3);
                    BloodGroups.RemoveAt(1);
                    break;
                case 5: //B+
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 1 || x.BloodGroup == 2 || x.BloodGroup == 5 || x.BloodGroup == 6).ToList();
                    BloodGroups.RemoveAt(8);
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(4);
                    BloodGroups.RemoveAt(3);
                    break;
                case 6: //B-
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 2 || x.BloodGroup == 6).ToList();
                    BloodGroups.RemoveAt(8);
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(5);
                    BloodGroups.RemoveAt(4);
                    BloodGroups.RemoveAt(3);
                    BloodGroups.RemoveAt(1);
                    break;
                case 7: //AB+
                    //do nothing
                    break;
                case 8: //AB-
                    EntitiesList = EntitiesList.Where(x => x.BloodGroup == 2 || x.BloodGroup == 4 || x.BloodGroup == 6 || x.BloodGroup == 8).ToList();
                    BloodGroups.RemoveAt(7);
                    BloodGroups.RemoveAt(5);
                    BloodGroups.RemoveAt(3);
                    BloodGroups.RemoveAt(1);
                    break;
                default:
                    //do nothing
                    break;
            }

            Pages = PopulatePages(EntitiesList);
            CurrentPage = Pages[0];
            CurrentPageNumber = 1;
        }
        private void InitializeEverything()
        {
            DonationTypesList = new List<DonationTypeModel>();
            BloodGroupsList = new List<BloodGroupModel>();
            StatusesList = new List<BloodGroupModel>();
            EntitiesList = new List<BloodModel>();
            EntitiesList = Connector.SqlConnector.GetBloodCollection();
            BloodGroupsList = GetBloodGroups();
            DonationTypesList = GetDonationTypes();
            StatusesList = GetStatuses();

            foreach (BloodModel item in EntitiesList)
            {
                item.BloodGroupModel = BloodGroupsList.Where(x => x.Id == item.BloodGroup).FirstOrDefault();
                item.DonationTypeModel = DonationTypesList.Where(x => x.Id == item.DonationType).FirstOrDefault();
            }

            Statuses = new ObservableCollection<BloodGroupModel>(StatusesList);
            SelectedStatus = Statuses[0];

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
        }
        private void SubscribeToEvents()
        {
            Connector.SqlConnector.OnBloodCreated += SqlConnector_OnBloodCreated;
            Connector.SqlConnector.OnBloodDeleted += SqlConnector_OnBloodDeleted;
            Connector.SqlConnector.OnDonorUpdated += SqlConnector_OnDonorUpdated;
            Connector.SqlConnector.OnDoctorUpdated += SqlConnector_OnDoctorUpdated;
            Connector.SqlConnector.OnBloodAmountUpdated += SqlConnector_OnBloodAmountUpdated;
        }

        private void SqlConnector_OnBloodAmountUpdated(object? sender, Dictionary<int, double> e)
        {
            BloodModel blood = EntitiesList.Where(x => x.Id == e.Keys.FirstOrDefault()).FirstOrDefault();

            if (blood != null)
            {
                blood.Amount = e.Values.FirstOrDefault();
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1; 
            }
        }
        private void SqlConnector_OnDoctorUpdated(object? sender, DoctorModel e)
        {
            List<BloodModel> blood = EntitiesList.Where(x => x.DoctorInCharge.Id == e.Id).ToList();
            if (blood != null)
            {
                foreach (var item in blood)
                {
                    item.DoctorInCharge = e;
                }                
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1; 
            }
        }
        private void SqlConnector_OnDonorUpdated(object? sender, DonorModel e)
        {
            List<BloodModel> blood = EntitiesList.Where(x => x.Donor.Id == e.Id).ToList();
            if (blood != null)
            {
                foreach (var item in blood)
                {
                    item.Donor = e; 
                }
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }
        }
        private void SqlConnector_OnBloodDeleted(object? sender, BloodModel e)
        {
            EntitiesList.Remove(e);

            Pages = PopulatePages(EntitiesList);
            CurrentPage = Pages[0];
            CurrentPageNumber = 1;
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

        private List<BloodGroupModel> GetStatuses()
        {
            List<BloodGroupModel> output = new List<BloodGroupModel>();
            output.Add(new BloodGroupModel { Id = -1, Name = "Все" });
            output.Add(new BloodGroupModel { Id = 1, Name = "В наличии" });
            output.Add(new BloodGroupModel { Id = 2, Name = "Выдано" });
            return output;
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

            if (SelectedBloodGroup != null && SelectedBloodGroup.Id != -1)
            {
                entities = entitiesInput.Where(x => x.BloodGroup == SelectedBloodGroup.Id).ToList();
            }
            else
            {
                entities = entitiesInput;
            }

            if (SelectedDonationType!= null && SelectedDonationType.Id != -1)
            {
                entities = entities.Where(x => x.DonationType == SelectedDonationType.Id).ToList();
            }

            if (SelectedStatus != null && SelectedStatus != Statuses.Where(x => x.Id == -1).FirstOrDefault())
            {
                if (SelectedStatus.Id == 1)
                {
                    entities = entities.Where(x => x.Amount > 0).ToList();
                }
                else if (SelectedStatus.Id == 2)
                {
                    entities = entities.Where(x => x.Amount <= 0).ToList();
                }
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
