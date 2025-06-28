using System;
using System.IO;
using System.Security.Cryptography;

namespace WpfAppForDiascan
{
    public static class HashCalculator
    {
        public static string Calculate(string filePath)
        {
            try
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hash = sha256.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}";
            }
        }
    }
}
