using System;
using System.Security.Cryptography;
using System.Text;

namespace Tipwin.Hash
{
    public static class HashedPassword
    {

        public enum SupportedHashAlgorithms
        {
            SHA256,
            SHA384,
            SHA512
        }


        public static string Encrypt(string plainText, SupportedHashAlgorithms hash, byte[] salt)
        {
            int minSaltLength = 4;
            int maxSaltLength = 16;

            byte[] SaltBytes = null;

            if (salt != null)
            {
                SaltBytes = salt;
            }
            else
            {
                Random r = new Random();
                int SaltLength = r.Next(minSaltLength, maxSaltLength);
                SaltBytes = new byte[SaltLength];

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                rng.GetNonZeroBytes(SaltBytes);
                rng.Dispose();
            }

            byte[] plainData = ASCIIEncoding.UTF8.GetBytes(plainText);
            byte[] plainDataAndSalt = new byte[plainData.Length + SaltBytes.Length];

            for (int i = 0; i < plainData.Length; i++)
                plainDataAndSalt[i] = plainData[i];

            for (int j = 0; j < SaltBytes.Length; j++)
                plainDataAndSalt[plainData.Length + j] = SaltBytes[j];

            byte[] hashValue = null;

            switch (hash)
            {
                case SupportedHashAlgorithms.SHA256:
                    SHA256Managed sha256 = new SHA256Managed();
                    hashValue = sha256.ComputeHash(plainDataAndSalt);
                    break;

                case SupportedHashAlgorithms.SHA384:
                    SHA384Managed sha384 = new SHA384Managed();
                    hashValue = sha384.ComputeHash(plainDataAndSalt);
                    break;

                case SupportedHashAlgorithms.SHA512:
                    SHA512Managed sha512 = new SHA512Managed();
                    hashValue = sha512.ComputeHash(plainDataAndSalt);
                    break;
            }

            byte[] result = new byte[hashValue.Length + SaltBytes.Length];

            for (int i = 0; i < hashValue.Length; i++)
                result[i] = hashValue[i];

            for (int j = 0; j < SaltBytes.Length; j++)
                result[hashValue.Length + j] = SaltBytes[j];

            return Convert.ToBase64String(result);
        }

        public static bool Confirm(string plainText, string hashValue, SupportedHashAlgorithms hash)
        {
            byte[] hashBytes = Convert.FromBase64String(hashValue);

            int hashSize = 0;

            switch (hash)
            {
                case SupportedHashAlgorithms.SHA256:
                    hashSize = 32;
                    break;
                case SupportedHashAlgorithms.SHA384:
                    hashSize = 48;
                    break;
                case SupportedHashAlgorithms.SHA512:
                    hashSize = 64;
                    break;
            }

            byte[] saltBytes = new byte[hashBytes.Length - hashSize];

            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashBytes[hashSize + i];

            string newHash = Encrypt(plainText, hash, saltBytes);

            return (hashValue == newHash);
        }

    }

}
