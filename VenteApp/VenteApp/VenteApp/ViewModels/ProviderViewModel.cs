using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class ProviderViewModel : BindableObject
    {
        private const int PageSize = 5;  // Number of providers per page
        private int _currentPage = 1;     // Current page number
        private int _totalPages;          // Total number of pages

        public ObservableCollection<Provider> Providers { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private readonly Func<Provider, Task<bool>> _confirmDelete;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged();
            }
        }

        public ProviderViewModel(Func<Provider, Task<bool>> confirmDelete)
        {
            Providers = new ObservableCollection<Provider>();
            _confirmDelete = confirmDelete;

            DeleteCommand = new Command<Provider>(OnDeleteProvider);
            SearchCommand = new Command<string>(OnSearchProviders);
            NextPageCommand = new Command(OnNextPage);
            PreviousPageCommand = new Command(OnPreviousPage);

            LoadProviders(); // Load providers for the first page
        }

        private void LoadProviders()
        {
            using (var db = new AppDbContext())
            {
                int totalProviders = db.Providers.Count();
                TotalPages = (int)Math.Ceiling(totalProviders / (double)PageSize);
                LoadPage(CurrentPage);
            }
        }

        private void LoadPage(int pageNumber)
        {
            using (var db = new AppDbContext())
            {
                int skip = (pageNumber - 1) * PageSize;
                var pagedProviders = db.Providers
                                       .OrderBy(p => p.Nom)
                                       .Skip(skip)
                                       .Take(PageSize)
                                       .ToList();

                Providers.Clear();
                foreach (var provider in pagedProviders)
                {
                    Providers.Add(provider);
                }
            }
        }

        private void OnNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadPage(CurrentPage);
            }
        }

        private void OnPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPage(CurrentPage);
            }
        }

        private async void OnDeleteProvider(Provider provider)
        {
            if (provider == null)
                return;

            bool confirm = await _confirmDelete(provider);
            if (!confirm)
                return;

            using (var db = new AppDbContext())
            {
                var providerToDelete = db.Providers.Find(provider.Id);
                if (providerToDelete != null)
                {
                    db.Providers.Remove(providerToDelete);
                    db.SaveChanges();
                }

                int totalProviders = db.Providers.Count();
                TotalPages = (int)Math.Ceiling(totalProviders / (double)PageSize);

                if (TotalPages == 0)
                {
                    TotalPages = 1;
                    CurrentPage = 1;
                }
                else if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages;
                }
            }

            LoadPage(CurrentPage);
        }

        private void OnSearchProviders(string query)
        {
            using (var db = new AppDbContext())
            {
                var filteredProviders = db.Providers
                                          .Where(p => p.Nom.ToLower().Contains(query.ToLower()) ||
                                                      p.Prenom.ToLower().Contains(query.ToLower()) ||
                                                      p.Email.ToLower().Contains(query.ToLower()))
                                          .OrderBy(p => p.Nom)
                                          .Skip((CurrentPage - 1) * PageSize)
                                          .Take(PageSize)
                                          .ToList();

                Providers.Clear();
                foreach (var provider in filteredProviders)
                {
                    Providers.Add(provider);
                }

                int totalFilteredProviders = db.Providers
                                               .Count(p => p.Nom.ToLower().Contains(query.ToLower()) ||
                                                           p.Prenom.ToLower().Contains(query.ToLower()) ||
                                                           p.Email.ToLower().Contains(query.ToLower()));
                TotalPages = (int)Math.Ceiling(totalFilteredProviders / (double)PageSize);
            }
        }

        public List<Provider> GetAllProvidersForPdf()
        {
            using (var db = new AppDbContext())
            {
                return db.Providers.OrderBy(p => p.Nom).ToList();
            }
        }
    }
}
