using System;
using System.Security.Cryptography;

namespace CSharpTutorial
{
    internal class PasswordHash
    {
        private string _storedPasswordHash;
        
        public static void Main()
        {
            var passHash = new PasswordHash();
            passHash.SavePassword("Akshay");
            passHash.CheckPassword("Akshay");
        }

        private  void SavePassword(string storedPassword)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            
            var pbkdf2 = new Rfc2898DeriveBytes(storedPassword, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            _storedPasswordHash = savedPasswordHash;
            Console.WriteLine($"Original Password Hash: {_storedPasswordHash}");
        }

        private  void CheckPassword(string userEnteredPassword)
        {

            string savedPasswordHash = _storedPasswordHash;

            var hashBytes = Convert.FromBase64String(savedPasswordHash);

            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(userEnteredPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] == hash[i]) continue;
                Console.WriteLine("Password NOT Matched !!!!!!!!!!!!!!!!!!!");
                return;
            }

            Console.WriteLine("Password Matched ..........");
        }
    }
}