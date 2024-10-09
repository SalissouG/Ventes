using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class UserViewModel : BindableObject
    {
        private const int PageSize = 1;  // Number of users per page
        private int _currentPage = 1;     // Current page number
        private int _totalPages;          // Total number of pages

        public ObservableCollection<User> Users { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private readonly Func<User, Task<bool>> _confirmDelete;

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


        public UserViewModel(Func<User, Task<bool>> confirmDelete)
        {
            Users = new ObservableCollection<User>();
            _confirmDelete = confirmDelete;

            DeleteCommand = new Command<User>(OnDeleteUser);
            SearchCommand = new Command<string>(OnSearchUsers);
            NextPageCommand = new Command(OnNextPage);
            PreviousPageCommand = new Command(OnPreviousPage);

            LoadUsers(); // Load users for the first page
        }

        // Load total number of users and first page from the database
        private void LoadUsers()
        {
            using (var db = new AppDbContext())
            {
                // Get the total number of users
                int totalUsers = db.Users.Count();

                // Calculate total pages
                TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

                // Load the first page
                LoadPage(CurrentPage);
            }
        }

        // Load users by page, directly from the database
        private void LoadPage(int pageNumber)
        {
            using (var db = new AppDbContext())
            {
                int skip = (pageNumber - 1) * PageSize;

                var pagedUsers = db.Users
                                   .OrderBy(u => u.Nom)
                                   .Skip(skip)
                                   .Take(PageSize)
                                   .ToList();

                Users.Clear();
                foreach (var user in pagedUsers)
                {
                    Users.Add(user);
                }
            }
        }

        // Handle the next page navigation
        private void OnNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadPage(CurrentPage); // Load the next page
            }
        }

        // Handle the previous page navigation
        private void OnPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPage(CurrentPage); // Load the previous page
            }
        }

        // Handle deleting a user from the database and UI
        private async void OnDeleteUser(User user)
        {
            if (user == null)
                return;

            bool confirm = await _confirmDelete(user);
            if (!confirm)
                return;

            using (var db = new AppDbContext())
            {
                var userToDelete = db.Users.Find(user.Id);
                if (userToDelete != null)
                {
                    db.Users.Remove(userToDelete);
                    db.SaveChanges();
                }

                int totalUsers = db.Users.Count();
                TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

                // Ensure that the current page does not exceed the total pages
                if (TotalPages == 0)
                {
                    TotalPages = 1;
                    CurrentPage = 1;
                }
                else if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages; // Go back to the last available page
                }
            }

            // Reload current page after deletion
            LoadPage(CurrentPage);
        }

        // Handle searching users directly from the database
        private void OnSearchUsers(string query)
        {
            using (var db = new AppDbContext())
            {
                var filteredUsers = db.Users
                                      .Where(u => u.Nom.ToLower().Contains(query.ToLower()) ||
                                                  u.Prenom.ToLower().Contains(query.ToLower()) ||
                                                  u.Email.ToLower().Contains(query.ToLower()))
                                      .OrderBy(u => u.Nom)
                                      .Skip((CurrentPage - 1) * PageSize)
                                      .Take(PageSize)
                                      .ToList();

                Users.Clear();
                foreach (var user in filteredUsers)
                {
                    Users.Add(user);
                }

                // Update the total pages based on the filtered result count
                int totalFilteredUsers = db.Users
                                           .Count(u => u.Nom.ToLower().Contains(query.ToLower()) ||
                                                       u.Prenom.ToLower().Contains(query.ToLower()) ||
                                                       u.Email.ToLower().Contains(query.ToLower()));
                TotalPages = (int)Math.Ceiling(totalFilteredUsers / (double)PageSize);
            }
        }
    }
}
