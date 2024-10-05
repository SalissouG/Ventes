using System.Security.Cryptography;
using System.Text;

namespace LicenceSystem
{
    class Program
    {
        // Clé secrète utilisée pour signer et valider les licences (doit être gardée secrète).
        private static readonly string SecretKey = "MySuperSecretKey";

        static void Main(string[] args)
        {
            Console.WriteLine("Génération d'une licence.");
            Console.Write("Entrez la date d'expiration de la licence (yyyy-MM-dd) : ");
            var inputDate = Console.ReadLine();
            DateTime expirationDate;
            if (DateTime.TryParse(inputDate, out expirationDate))
            {
                var licenceKey = GenerateLicence(expirationDate);
                Console.WriteLine("Licence générée : " + licenceKey);
            }
            else
            {
                Console.WriteLine("Date invalide.");
            }

            Console.WriteLine("Vérification d'une licence.");
            Console.Write("Entrez la clé de licence : ");
            var licenceToValidate = Console.ReadLine();
            var isValid = ValidateLicence(licenceToValidate);

            Console.WriteLine(isValid ? "Licence valide." : "Licence invalide ou expirée.");
        }

        // Générer une licence avec une date d'expiration
        public static string GenerateLicence(DateTime expirationDate)
        {
            var expirationString = expirationDate.ToString("yyyy-MM-dd");
            var signature = SignData(expirationString, SecretKey);
            var licence = $"{expirationString}.{signature}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(licence));
        }

        // Valider une licence
        public static bool ValidateLicence(string licence)
        {
            try
            {
                var decodedLicence = Encoding.UTF8.GetString(Convert.FromBase64String(licence));
                var parts = decodedLicence.Split('.');
                if (parts.Length != 2) return false;

                var expirationString = parts[0];
                var signature = parts[1];

                // Vérifier la signature
                if (signature != SignData(expirationString, SecretKey)) return false;

                // Vérifier la date d'expiration
                DateTime expirationDate;
                if (!DateTime.TryParse(expirationString, out expirationDate)) return false;

                return expirationDate >= DateTime.Now; // Valide si non expirée
            }
            catch
            {
                return false;
            }
        }

        // Signer les données avec la clé secrète (HMAC SHA256)
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
