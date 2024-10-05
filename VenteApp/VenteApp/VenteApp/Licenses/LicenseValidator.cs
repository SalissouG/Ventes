using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace VenteApp
{
    public static class LicenseValidator
    {
        public static bool IsLicenceValid()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "licence.json");

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var licenceData = JsonConvert.DeserializeObject<dynamic>(json);
                var licenceKey = (string)licenceData.LicenceKey;

                return ValidateLicence(licenceKey);
            }

            return false;
        }

        // Valider la licence
        private static bool ValidateLicence(string licence)
        {
            try
            {
                var decodedLicence = Encoding.UTF8.GetString(Convert.FromBase64String(licence));
                var parts = decodedLicence.Split('.');
                if (parts.Length != 2) return false;

                var expirationString = parts[0];
                var signature = parts[1];

                if (signature != SignData(expirationString, "MySuperSecretKey")) return false;

                DateTime expirationDate;
                if (!DateTime.TryParse(expirationString, out expirationDate)) return false;

                return expirationDate >= DateTime.Now;
            }
            catch
            {
                return false;
            }
        }

        // Fonction pour signer les données avec la clé secrète
        private static string SignData(string data, string key)
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
