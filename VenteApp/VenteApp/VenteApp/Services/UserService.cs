using System;

namespace VenteApp
{
    public class UserService
    {
        private static UserService _instance;
        public static UserService Instance => _instance ??= new UserService();

        public User ConnectedUser { get; private set; }

        // Private constructor to prevent instantiation from outside
        private UserService() { }

        // Method to set the connected user
        public void SetConnectedUser(User user)
        {
            ConnectedUser = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        // Method to check if the connected user is an admin
        public bool IsAdmin()
        {
            return ConnectedUser?.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        // Method to clear the connected user (for logout)
        public void ClearConnectedUser()
        {
            ConnectedUser = null;
        }
    }
}
