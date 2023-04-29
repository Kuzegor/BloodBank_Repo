using BloodBank.Commands;
using BloodBankLibrary;
using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.ViewModels
{
    class IssueViewModel : ViewModelBase
    {

        private List<BloodGroupModel> BloodGroupsList { get; set; }
        private List<DonationTypeModel> DonationTypesList { get; set; }

        private List<IssueModel> EntitiesList { get; set; }

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

        private List<ObservableCollection<IssueModel>> Pages { get; set; }

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

        private ObservableCollection<IssueModel> currentPage;
        public ObservableCollection<IssueModel> CurrentPage
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

        private IssueModel selectedEntity;
        public IssueModel SelectedEntity
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
                        IssueModel lastEntity = CurrentPage[CurrentPage.Count / 2];
                        Pages = PopulatePages(EntitiesList);
                        foreach (ObservableCollection<IssueModel> page in Pages)
                        {
                            foreach (IssueModel entity in page)
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
        public DelegateCommand OpenPrintViewCommand { get; set; }

        public IssueViewModel()
        {
            DonationTypesList = new List<DonationTypeModel>();
            BloodGroupsList = new List<BloodGroupModel>();
            EntitiesList = new List<IssueModel>();
            EntitiesList = Connector.SqlConnector.GetAllIssues();
            BloodGroupsList = GetBloodGroups();
            DonationTypesList = GetDonationTypes();

            foreach (IssueModel item in EntitiesList)
            {
                item.BloodGroupModel = BloodGroupsList.Where(x => x.Id == item.Blood.BloodGroup).FirstOrDefault();
                item.DonationTypeModel = DonationTypesList.Where(x => x.Id == item.Blood.DonationType).FirstOrDefault();
                if (item.Recipient != null)
                {
                    item.Recipient.BloodGroupModel = BloodGroupsList.Where(x => x.Id == item.Recipient.BloodGroup).FirstOrDefault(); 
                }
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

            OpenPrintViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new IssuePrintViewModel(SelectedEntity);
            });
            OpenDeleteViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new IssueDeleteViewModel(SelectedEntity);
            });
            OpenEditViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new IssueCreateViewModel(SelectedEntity);
            });
            OpenCreateViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new IssueCreateViewModel();
            });
            SearchCommand = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    SelectedBloodGroup = BloodGroups.Where(x => x.Id == -1).FirstOrDefault();
                    List<IssueModel> EntitiesToShow = SearchForEntities(SearchBoxText);
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
            Connector.SqlConnector.OnIssueCreated += SqlConnector_OnIssueCreated;
            Connector.SqlConnector.OnIssueDeleted += SqlConnector_OnIssueDeleted;
            Connector.SqlConnector.OnDoctorUpdated += SqlConnector_OnDoctorUpdated;
            Connector.SqlConnector.OnRecipientUpdated += SqlConnector_OnRecipientUpdated;
            Connector.SqlConnector.OnBloodAmountUpdated += SqlConnector_OnBloodAmountUpdated;
            Connector.SqlConnector.OnBloodUpdated += SqlConnector_OnBloodUpdated;
        }

        private void SqlConnector_OnBloodUpdated(object? sender, BloodModel e)
        {
            List<IssueModel> issues = EntitiesList.Where(x => x.Blood.Id == e.Id).ToList();
            if (issues != null)
            {
                foreach (var item in issues)
                {
                    item.Blood = e;
                }
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }
        }
        private void SqlConnector_OnBloodAmountUpdated(object? sender, Dictionary<int, double> e)
        {
            List<IssueModel> issues = EntitiesList.Where(x => x.Blood.Id == e.Keys.FirstOrDefault()).ToList();
            if (issues != null)
            {
                foreach (IssueModel item in issues)
                {
                    item.Blood.Amount = e.Values.FirstOrDefault();
                }
            }
        }
        private void SqlConnector_OnRecipientUpdated(object? sender, RecipientModel e)
        {
            List<IssueModel> issues = EntitiesList.Where(x => x.Recipient.Id == e.Id).ToList();
            if (issues != null)
            {
                foreach (var item in issues)
                {
                    item.Recipient = e;
                }
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }
        }
        private void SqlConnector_OnDoctorUpdated(object? sender, DoctorModel e)
        {
            List<IssueModel> issues = EntitiesList.Where(x => x.DoctorInCharge.Id == e.Id).ToList();
            if (issues != null)
            {
                foreach (var item in issues)
                {
                    item.DoctorInCharge = e;
                }             
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;
            }
        }
        private void SqlConnector_OnIssueDeleted(object? sender, IssueModel e)
        {
            EntitiesList.Remove(e);
            Pages = PopulatePages(EntitiesList);
            CurrentPage = Pages[0];
            CurrentPageNumber = 1;
        }
        private void SqlConnector_OnIssueCreated(object? sender, IssueModel e)
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
        private List<ObservableCollection<IssueModel>> PopulatePages(List<IssueModel> entitiesInput)
        {
            List<ObservableCollection<IssueModel>> pages = new List<ObservableCollection<IssueModel>>();
            List<IssueModel> entities;

            if (SelectedBloodGroup != null && SelectedBloodGroup != BloodGroups.Where(x => x.Id == -1).FirstOrDefault())
            {
                entities = entitiesInput.Where(x => x.Blood.BloodGroup == SelectedBloodGroup.Id).ToList();
            }
            else
            {
                entities = entitiesInput;
            }

            if (SelectedDonationType != null && SelectedDonationType != DonationTypes.Where(x => x.Id == -1).FirstOrDefault())
            {
                entities = entities.Where(x => x.Blood.DonationType == SelectedDonationType.Id).ToList();
            }
            else
            {
                entities = entities;
            }

            int index = 0;
            pages.Add(new ObservableCollection<IssueModel>());
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
                            pages.Add(new ObservableCollection<IssueModel>());
                            pages[index].Add(entities[i]);
                        }
                    }
                }
            }

            return pages;
        }
        private List<IssueModel> SearchForEntities(string searchBoxText)
        {
            List<IssueModel> enitiesToShow = EntitiesList;
            string clippedText = searchBoxText.Trim();
            clippedText += " ";
            string word;
            for (int i = 0; i < clippedText.Length; i++)
            {
                if (clippedText[i] == ' ')
                {
                    word = clippedText.Substring(0, i);
                    enitiesToShow = enitiesToShow.Where(x => x.Recipient.Name.Contains(word, StringComparison.OrdinalIgnoreCase)).ToList();

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
