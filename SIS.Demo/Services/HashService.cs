using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace IRunesWebApp.Services
{
    public static class HashService
    {
        public static string Hash(this string passwordToHash) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordToHash));
                string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
