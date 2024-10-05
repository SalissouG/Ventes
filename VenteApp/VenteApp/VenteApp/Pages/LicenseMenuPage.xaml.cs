using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace VenteApp
{
    public partial class LicenseMenuPage : ContentPage
    {
        public LicenseMenuPage()
        {
            InitializeComponent();
            PreloadLicence();
        }

        // Précharger la licence si elle existe déjà
        private void PreloadLicence()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "licence.json");

            // Vérifier si le fichier de licence existe
            if (File.Exists(path))
            {
                // Lire la licence à partir du fichier JSON
                var json = File.ReadAllText(path);
                var licenceData = JsonConvert.DeserializeObject<dynamic>(json);
                var licenceKey = (string)licenceData.LicenceKey;

                // Charger la licence dans le champ d'entrée
                LicenceEntry.Text = licenceKey;

                // Valider la licence chargée automatiquement
                if (ValidateLicence(licenceKey))
                {
                    ValidationMessageLabel.Text = "Licence valide.";
                    ValidationMessageLabel.TextColor = Colors.Green;
                }
                else
                {
                    ValidationMessageLabel.Text = "Licence expirée ou invalide.";
                    ValidationMessageLabel.TextColor = Colors.Red;
                }
            }
        }


        // Event handler for the "Valider" button click
        public async void OnValidateLicence(object sender, EventArgs e)
        {
            var licenceKey = LicenceEntry.Text; // Get the inputted licence key

            if (string.IsNullOrEmpty(licenceKey))
            {
                // Display message if the licence key is empty
                ValidationMessageLabel.Text = "Veuillez entrer une clé de licence.";
                return;
            }

            // Validate the licence key
            if (ValidateLicence(licenceKey))
            {
                // If valid, save it and display success message
                SaveLicence(licenceKey);
                ValidationMessageLabel.Text = "Licence valide.";
                ValidationMessageLabel.TextColor = Colors.Green;

            }
            else
            {
                SaveLicence(licenceKey);
                // If invalid, display error message
                ValidationMessageLabel.Text = "Licence invalide ou expirée.";
                ValidationMessageLabel.TextColor = Colors.Red;
            }
        }

        // Method to validate the licence
        private bool ValidateLicence(string licence)
        {
            try
            {
                var decodedLicence = Encoding.UTF8.GetString(Convert.FromBase64String(licence));
                var parts = decodedLicence.Split('.');
                if (parts.Length != 2) return false;

                var expirationString = parts[0];
                var signature = parts[1];

                // Check the signature with the secret key
                if (signature != SignData(expirationString, "MySuperSecretKey")) return false;

                // Check if the licence has expired
                DateTime expirationDate;
                if (!DateTime.TryParse(expirationString, out expirationDate)) return false;

                return expirationDate >= DateTime.Now;
            }
            catch
            {
                return false;
            }
        }

        // Save the valid licence in a JSON file
        private void SaveLicence(string licence)
        {
            var licenceData = new { LicenceKey = licence };
            var json = JsonConvert.SerializeObject(licenceData);

            var path = Path.Combine(FileSystem.AppDataDirectory, "licence.json");
            File.WriteAllText(path, json); // Save the licence in the app's data directory
        }

        // Sign data using HMAC SHA256 with a secret key
        private string SignData(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
