using BloodBank.Commands;
using BloodBankLibrary.Models;
using BloodBankLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BloodBank.ViewModels
{
    internal class RecipientsViewModel : ViewModelBase
    {
        private List<BloodGroupModel> BloodGroupsList { get; set; }

        private List<RecipientModel> EntitiesList { get; set; }

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

        private List<ObservableCollection<RecipientModel>> Pages { get; set; }

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

        private ObservableCollection<RecipientModel> currentPage;
        public ObservableCollection<RecipientModel> CurrentPage
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

        private RecipientModel selectedEntity;
        public RecipientModel SelectedEntity
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
                        RecipientModel lastEntity = CurrentPage[CurrentPage.Count / 2];
                        Pages = PopulatePages(EntitiesList);
                        foreach (ObservableCollection<RecipientModel> page in Pages)
                        {
                            foreach (RecipientModel entity in page)
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

        public RecipientsViewModel()
        {
            BloodGroupsList = new List<BloodGroupModel>();
            EntitiesList = new List<RecipientModel>();
            EntitiesList = Connector.SqlConnector.GetAllRecipients();
            BloodGroupsList = GetBloodGroups();

            foreach (RecipientModel item in EntitiesList)
            {
                item.BloodGroupModel = BloodGroupsList.Where(x => x.Id == item.BloodGroup).FirstOrDefault();
            }

            BloodGroups = new ObservableCollection<BloodGroupModel>(BloodGroupsList);
            BloodGroups.Insert(0, new BloodGroupModel { Id = -1, Name = "Все группы крови" });
            SelectedBloodGroup = BloodGroups[0];

            ItemsPerPage = new ObservableCollection<int> { 10, 30, 50, 100};
            SelectedItemsPerPage = ItemsPerPage[0];


            if (EntitiesList.Count > 0)
            {
                CurrentPage = Pages[0];
                CurrentPageNumber = 1;                
            }

            OpenDeleteViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new RecipientsDeleteViewModel(SelectedEntity);
            });
            OpenEditViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new RecipientsCreateViewModel(SelectedEntity, BloodGroupsList);
            });
            OpenCreateViewCommand = new DelegateCommand(x =>
            {
                MainMenuViewModel.Instance.CurrentViewModelTwo = new RecipientsCreateViewModel(BloodGroupsList);
            });
            SearchCommand = new DelegateCommand(x =>
            {
                if (EntitiesList.Count > 0)
                {
                    SelectedBloodGroup = BloodGroups.Where(x => x.Id == -1).FirstOrDefault();
                    List<RecipientModel> EntitiesToShow = SearchForEntities(SearchBoxText);
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
            Connector.SqlConnector.OnRecipientCreated += SqlConnector_OnRecipientCreated;
            Connector.SqlConnector.OnRecipientDeleted += SqlConnector_OnRecipientDeleted;
        }
        private void SqlConnector_OnRecipientDeleted(object? sender, RecipientModel e)
        {
            EntitiesList.Remove(e);
            if (EntitiesList.Count > 0)
            {
                Pages = PopulatePages(EntitiesList);
                CurrentPage = Pages[0];
                CurrentPageNumber = 1; 
            }
        }
        private void SqlConnector_OnRecipientCreated(object? sender, RecipientModel e)
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
        private List<ObservableCollection<RecipientModel>> PopulatePages(List<RecipientModel> entitiesInput)
        {
            List<ObservableCollection<RecipientModel>> pages = new List<ObservableCollection<RecipientModel>>();
            List<RecipientModel> entities;

            if (SelectedBloodGroup != null && SelectedBloodGroup != BloodGroups.Where(x => x.Id == -1).FirstOrDefault())
            {
                entities = entitiesInput.Where(x => x.BloodGroup == SelectedBloodGroup.Id).ToList();
            }
            else
            {
                entities = entitiesInput;
            }

            int index = 0;
            pages.Add(new ObservableCollection<RecipientModel>());
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
                            pages.Add(new ObservableCollection<RecipientModel>());
                            pages[index].Add(entities[i]);
                        }
                    }
                }
            }

            return pages;
        }
        private List<RecipientModel> SearchForEntities(string searchBoxText)
        {
            List<RecipientModel> enitiesToShow = EntitiesList;
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
