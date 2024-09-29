using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VenteApp
{
    public class ClientViewModel
    {
        public ObservableCollection<Client> Clients { get; set; }

        // Commands for editing and deleting clients
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ClientViewModel()
        {
            // Initialize with sample data
            Clients = new ObservableCollection<Client>
        {
            new Client { Nom = "Doe", Prenom = "John", Telephone = "1234567890", Email = "john.doe@example.com", Adresse = "123 Main St" },
            new Client { Nom = "Smith", Prenom = "Jane", Telephone = "9876543210", Email = "jane.smith@example.com", Adresse = "456 Oak St" },
        };

            // Commands
            EditCommand = new Command<Client>(OnEditClient);
            DeleteCommand = new Command<Client>(OnDeleteClient);
        }

        private void OnEditClient(Client client)
        {
            // Handle the edit action (e.g., navigate to an edit page)
            App.Current.MainPage.DisplayAlert("Edit", $"Edit Client: {client.Nom} {client.Prenom}", "OK");
        }

        private void OnDeleteClient(Client client)
        {
            // Remove the client from the collection
            Clients.Remove(client);
            App.Current.MainPage.DisplayAlert("Deleted", $"Deleted Client: {client.Nom} {client.Prenom}", "OK");
        }
    }
}
