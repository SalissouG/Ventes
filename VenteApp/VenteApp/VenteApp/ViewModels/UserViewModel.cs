using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class UserViewModel
    {
        public ObservableCollection<User> Users { get; set; }

        // Commands for editing and deleting users
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public UserViewModel()
        {
            // Initialize with sample data
            Users = new ObservableCollection<User>
        {
            new User { Nom = "Doe", Prenom = "John", Telephone = "1234567890", Email = "john.doe@example.com", Adresse = "123 Main St" },
            new User { Nom = "Smith", Prenom = "Jane", Telephone = "9876543210", Email = "jane.smith@example.com", Adresse = "456 Oak St" },
        };

            // Commands
            EditCommand = new Command<User>(OnEditUser);
            DeleteCommand = new Command<User>(OnDeleteUser);
        }

        private void OnEditUser(User user)
        {
            // Handle the edit action (e.g., navigate to an edit page)
            App.Current.MainPage.DisplayAlert("Edit", $"Edit User: {user.Nom} {user.Prenom}", "OK");
        }

        private void OnDeleteUser(User user)
        {
            // Remove the user from the collection
            Users.Remove(user);
            App.Current.MainPage.DisplayAlert("Deleted", $"Deleted User: {user.Nom} {user.Prenom}", "OK");
        }
    }
}
