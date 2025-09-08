//	<copyright file="DataCryptography.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for DataCryptography.
//	</summary>
namespace SocketDataSecurity
{
    using System.Security.Cryptography;

    /// <summary>
    /// Handles encryption and decryption of data using RSA.
    /// </summary>
    public sealed class DataCryptography
    {
        /// <summary>The AES key length in bytes.</summary>
        private const int KeyLength = 4;
        /// <summary>The AES IV (Initialization Vector) length in bytes</summary>
        private const int IVLength = 4;

        /// <summary>The RSA object for this class.</summary>
        private RSA? _rsa;

        /// <summary>Initializes a new instance of the <see cref="DataCryptography"/> class. Creates an RSA public/private key pair.</summary>
        public DataCryptography()
        {
            _rsa = RSA.Create(2048);
        }

        /// <summary>Initializes a new instance of the <see cref="DataCryptography"/> class. Creates keys for this object using the provided RSA public key.</summary>
        /// <param name="externalPublicKey">The external RSA public key.</param>
        /// <exception cref="ArgumentNullException">externalPublicKey</exception>
        public DataCryptography(byte[] externalPublicKey)
        {
            ArgumentNullException.ThrowIfNull(externalPublicKey, nameof(externalPublicKey));
            this.ImportPublicKey(externalPublicKey);
        }

        /// <summary>
        /// Encrypts the provided message. Prepends AES key and IV data that can only be deciphered by the receiver.
        /// </summary>
        /// <param name="rawData">The raw data to encrypt.</param>
        /// <returns>A byte array containing the encrypted message.</returns>
        public byte[] EncryptData(byte[] rawData)
        {
            Aes aes = Aes.Create();
            ICryptoTransform transform = aes.CreateEncryptor();

            // Encrypt the AES key.
            byte[] keyEncrypted = _rsa!.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);

            // Create byte arrays to contain the length and values of the key and iv.
            int lKey = keyEncrypted.Length;
            int lIV = aes.IV.Length;
            byte[] LenK = BitConverter.GetBytes(lKey);
            byte[] LenIV = BitConverter.GetBytes(lIV);

            // Build encrypted key segment.
            byte[] keySegment = new byte[LenK.Length + LenIV.Length + lKey + lIV];

            Array.Copy(LenK, 0, keySegment, 0, LenK.Length);
            Array.Copy(LenIV, 0, keySegment, LenK.Length, LenIV.Length);
            Array.Copy(keyEncrypted, 0, keySegment, (LenK.Length + LenIV.Length), lKey);
            Array.Copy(aes.IV, 0, keySegment, (LenK.Length + LenIV.Length + lKey), lIV);

            byte[] encryptedData;
            using (MemoryStream ms = new())
            {
                using (CryptoStream outStreamEncrypted = new(ms, transform, CryptoStreamMode.Write))
                {
                    outStreamEncrypted.Write(rawData, 0, rawData.Length);
                    outStreamEncrypted.FlushFinalBlock();

                    encryptedData = ms.ToArray();
                }
            }

            byte[] keyAndData = new byte[keySegment.Length + encryptedData.Length];
            Array.Copy(keySegment, 0, keyAndData, 0, keySegment.Length);
            Array.Copy(encryptedData, 0, keyAndData, keySegment.Length, encryptedData.Length);

            return keyAndData;
        }

        /// <summary>
        /// Decrypts the provided message. Deciphers key data that's been prepended and encrypted with this instance's public key.
        /// Naturally, this instance can only decrypt the data if it owns the corresponding private key due to the nature of asymmetric keys.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns>A byte array containing the decrypted data</returns>
        public byte[] DecryptData(byte[] encryptedData)
        {
            Aes aes = Aes.Create();

            byte[] LenK = new byte[KeyLength];
            byte[] LenIV = new byte[IVLength];

            // Get the key segment.
            Array.Copy(encryptedData, 0, LenK, 0, KeyLength);
            Array.Copy(encryptedData, KeyLength, LenIV, 0, IVLength);

            int lenK = BitConverter.ToInt32(LenK);
            int lenIV = BitConverter.ToInt32(LenIV);

            // Get start position of cipher.
            int startC = lenK + lenIV + sizeof(long);
            int lenC = (int)encryptedData.Length - startC;

            // Extract key and IV.
            byte[] keyEncrypted = new byte[lenK];
            byte[] iv = new byte[lenIV];

            Array.Copy(encryptedData, (KeyLength + IVLength), keyEncrypted, 0, lenK);
            Array.Copy(encryptedData, (KeyLength + IVLength + lenK), iv, 0, lenIV);

            // Use RSACryptoProvider to decrypt the AES key.
            byte[] keyDecrypted = _rsa!.Decrypt(keyEncrypted, RSAEncryptionPadding.Pkcs1);
            ICryptoTransform transform = aes.CreateDecryptor(keyDecrypted, iv);

            // Byte array to hold the decrypted data.
            byte[] decryptedData = new byte[lenC];

            using (MemoryStream ms = new())
            {
                using (CryptoStream outStreamDecrypted = new(ms, transform, CryptoStreamMode.Write))
                {
                    outStreamDecrypted.Write(encryptedData, startC, lenC);

                    outStreamDecrypted.FlushFinalBlock();
                    decryptedData = ms.ToArray();
                }
            }

            return decryptedData;
        }

        /// <summary>
        /// Exports the public key. This method simulates Alice giving Bob her public key so he can encrypt messages for her. He and others
        /// who have that public key will not be able to decrypt them because they do not have the full key pair with private parameters.
        /// </summary>
        /// <returns>A byte array containing the RSA public key for this instance.</returns>
        public byte[] ExportPublicKey()
        {
            return _rsa!.ExportRSAPublicKey();
        }

        /// <summary>
        /// This loads the key with only public parameters, as created with the ExportPublicKey, and sets it as the key container name.
        /// This simulates the scenario of Bob loading Alice's key with only public parameters so he can encrypt the files for her.
        /// </summary>
        public void ImportPublicKey(byte[] externalPublicKey)
        {
            _rsa = RSA.Create(2048);
            _rsa.ImportRSAPublicKey(externalPublicKey, out _);
        }
    }
}
