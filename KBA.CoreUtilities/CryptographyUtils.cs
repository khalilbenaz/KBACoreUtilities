using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Provides cryptography utilities for encryption, hashing, and secure token generation
    /// </summary>
    public static class CryptographyUtils
    {
        #region Hashing

        /// <summary>
        /// Generates SHA256 hash of a string
        /// </summary>
        public static string HashSHA256(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Generates SHA512 hash of a string
        /// </summary>
        public static string HashSHA512(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha512.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Generates MD5 hash of a string (not recommended for security, use for checksums only)
        /// </summary>
        public static string HashMD5(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Hashes a password using PBKDF2 (recommended for password storage)
        /// </summary>
        public static string HashPassword(string password, int iterations = 10000)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            // Generate salt
            var salt = GenerateRandomBytes(32);

            // Generate hash
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            // Combine salt and hash
            var hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies a password against a hashed password
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword, int iterations = 10000)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                var hashBytes = Convert.FromBase64String(hashedPassword);

                // Extract salt (first 32 bytes)
                var salt = new byte[32];
                Array.Copy(hashBytes, 0, salt, 0, 32);

                // Generate hash with same salt
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                var hash = pbkdf2.GetBytes(32);

                // Compare hashes
                for (var i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 32] != hash[i])
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region AES Encryption

        /// <summary>
        /// Encrypts a string using AES-256
        /// </summary>
        public static string EncryptAES(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Plain text cannot be empty", nameof(plainText));
            
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be empty", nameof(key));

            using var aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(key, 32); // 256-bit key
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            
            // Write IV first
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Decrypts an AES-256 encrypted string
        /// </summary>
        public static string DecryptAES(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Cipher text cannot be empty", nameof(cipherText));
            
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be empty", nameof(key));

            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(key, 32);

            // Read IV
            var iv = new byte[aes.IV.Length];
            Array.Copy(buffer, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            return sr.ReadToEnd();
        }

        #endregion

        #region RSA Encryption

        /// <summary>
        /// Generates RSA key pair (public and private keys)
        /// </summary>
        public static (string publicKey, string privateKey) GenerateRSAKeyPair(int keySize = 2048)
        {
            using var rsa = RSA.Create(keySize);
            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            return (publicKey, privateKey);
        }

        /// <summary>
        /// Encrypts data using RSA public key
        /// </summary>
        public static string EncryptRSA(string plainText, string publicKeyBase64)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Plain text cannot be empty", nameof(plainText));

            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKeyBase64), out _);

            var dataBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedData = rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);
            
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts RSA encrypted data using private key
        /// </summary>
        public static string DecryptRSA(string cipherText, string privateKeyBase64)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Cipher text cannot be empty", nameof(cipherText));

            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKeyBase64), out _);

            var dataBytes = Convert.FromBase64String(cipherText);
            var decryptedData = rsa.Decrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);
            
            return Encoding.UTF8.GetString(decryptedData);
        }

        #endregion

        #region Digital Signatures

        /// <summary>
        /// Signs data using RSA private key
        /// </summary>
        public static string SignData(string data, string privateKeyBase64)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("Data cannot be empty", nameof(data));

            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKeyBase64), out _);

            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signature = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            
            return Convert.ToBase64String(signature);
        }

        /// <summary>
        /// Verifies RSA signature
        /// </summary>
        public static bool VerifySignature(string data, string signatureBase64, string publicKeyBase64)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(signatureBase64))
                return false;

            try
            {
                using var rsa = RSA.Create();
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKeyBase64), out _);

                var dataBytes = Encoding.UTF8.GetBytes(data);
                var signature = Convert.FromBase64String(signatureBase64);
                
                return rsa.VerifyData(dataBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Random Generation

        /// <summary>
        /// Generates a cryptographically secure random string
        /// </summary>
        public static string GenerateRandomString(int length, bool includeSpecialChars = false)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            
            var characterSet = includeSpecialChars ? chars + specialChars : chars;
            var result = new char[length];

            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            for (var i = 0; i < length; i++)
            {
                result[i] = characterSet[randomBytes[i] % characterSet.Length];
            }

            return new string(result);
        }

        /// <summary>
        /// Generates cryptographically secure random bytes
        /// </summary>
        public static byte[] GenerateRandomBytes(int length)
        {
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Generates a secure token (Base64 encoded random bytes)
        /// </summary>
        public static string GenerateSecureToken(int byteLength = 32)
        {
            var randomBytes = GenerateRandomBytes(byteLength);
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Generates a GUID/UUID
        /// </summary>
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Generates a numeric OTP (One-Time Password)
        /// </summary>
        public static string GenerateOTP(int length = 6)
        {
            var otp = new char[length];
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            for (var i = 0; i < length; i++)
            {
                otp[i] = (char)('0' + (randomBytes[i] % 10));
            }

            return new string(otp);
        }

        #endregion

        #region HMAC

        /// <summary>
        /// Generates HMAC-SHA256 signature
        /// </summary>
        public static string GenerateHMAC(string message, string key)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message cannot be empty", nameof(message));
            
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be empty", nameof(key));

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Verifies HMAC-SHA256 signature
        /// </summary>
        public static bool VerifyHMAC(string message, string signature, string key)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(signature))
                return false;

            try
            {
                var expectedSignature = GenerateHMAC(message, key);
                return signature == expectedSignature;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Helper Methods

        private static byte[] DeriveKeyFromPassword(string password, int keyLength)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(passwordBytes);

            if (hash.Length >= keyLength)
                return hash.Take(keyLength).ToArray();

            // If hash is too short, concatenate multiple hashes
            var key = new byte[keyLength];
            var offset = 0;
            
            while (offset < keyLength)
            {
                var copyLength = Math.Min(hash.Length, keyLength - offset);
                Array.Copy(hash, 0, key, offset, copyLength);
                offset += copyLength;
                
                if (offset < keyLength)
                    hash = sha256.ComputeHash(hash);
            }

            return key;
        }

        /// <summary>
        /// Generates a secure hash for file integrity check
        /// </summary>
        public static string ComputeFileHash(string filePath, string algorithm = "SHA256")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            using var stream = File.OpenRead(filePath);
            HashAlgorithm hashAlgorithm = algorithm.ToUpperInvariant() switch
            {
                "MD5" => MD5.Create(),
                "SHA1" => SHA1.Create(),
                "SHA256" => SHA256.Create(),
                "SHA512" => SHA512.Create(),
                _ => throw new ArgumentException($"Unsupported algorithm: {algorithm}")
            };

            using (hashAlgorithm)
            {
                var hash = hashAlgorithm.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        #endregion
    }
}
