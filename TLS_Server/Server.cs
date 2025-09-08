//	<copyright file="Server.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Server.
//	</summary>
namespace TLS_Server
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using SocketDataSecurity;

    /// <summary>
    /// Server class to demonstrate sending/receiving encrypted messages to and from a client.
    /// </summary>
    internal class Server
    {
        /// <summary>The TCP port number to connect to.</summary>
        private const int Port = 5000;

        /// <summary>The host name.</summary>
        private const string HostEntry = "localhost";

        /// <summary>
        /// The secret message to send to the client.
        /// </summary>
        private const string SecretMessage = "This is a secret message from the server.";

        /// <summary>IO exception message to print when message sending fails.</summary>
        private const string IoExMsg = "Unable to send data.";

        /// <summary>The Socket used for connections.</summary>
        private readonly Socket server;

        /// <summary>Host entry object.</summary>
        private readonly IPHostEntry host;

        /// <summary>IP address object.</summary>
        private readonly IPAddress ip;

        /// <summary>The local end point object.</summary>
        private readonly IPEndPoint localEndPoint;

        /// <summary>Object used to encrypt data using the client's public key. Cannot decrypt data encrypted using said key.</summary>
        private DataCryptography? clientEncryptorOnly;

        /// <summary>Object used to encrypt and decrypt data using it's private key. Can only decrypt data encrypted using it's public key.
        /// Send it's public key to the server for them to encrypt data with.
        /// </summary>
        private DataCryptography? serverEncryptDecryptor;

        /// <summary>Handler object for the connection.</summary>
        private Socket? handler;

        /// <summary>The buffer used for data transmission.</summary>
        private readonly byte[] buffer;

        /// <summary>Initializes a new instance of the <see cref="Server"/> class.</summary>
        public Server()
        {
            host = Dns.GetHostEntry(HostEntry);
            ip = host.AddressList[0];
            localEndPoint = new(ip, Port);
            server = new(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverEncryptDecryptor = new();
            buffer = new byte[1024];
        }

        /// <summary>Runs the server.</summary>
        /// <exception cref="IOException">Exception thrown when data is unable to be sent to the client.</exception>
        public void Run()
        {
            server.Bind(localEndPoint);
            server.Listen();

            Console.WriteLine("Server\n\nWaiting for a connection...");
            handler = server.Accept();

            Console.WriteLine($"Connected to {handler.RemoteEndPoint}!");

            // Get the public key from the client to create our clientEncryptorOnly and encrypt data for the client.
            byte[] externalPublicKey = ReceiveMessage(buffer);
            clientEncryptorOnly = new DataCryptography(externalPublicKey);

            // Send the encrypted message
            SendEncryptedMessage(SecretMessage, true);

            // Decrypt data from the sender using clientEncryptorOnly. This is expected to fail since it was encrypted with the client's public key.
            byte[] externalEncryptedDataFail = ReceiveMessage(buffer);

            try
            {
                byte[] decryptedData = clientEncryptorOnly.DecryptData(externalEncryptedDataFail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                // Print data just to see what it looks like.
                Console.WriteLine($"Could not decrypt message: \n{Encoding.UTF8.GetString(externalEncryptedDataFail)}\n");
            }

            // Send our public key to the client to encrypt the data with so it can be decrypted on our side.
            byte[] internalPublicKey = serverEncryptDecryptor!.ExportPublicKey();

            if (!SendBytes(internalPublicKey, 0, internalPublicKey.Length))
            {
                throw new IOException(IoExMsg);
            }

            // Get data encrypted with our public key from the client. Decryption should work now with our serverEncryptDecryptor.
            byte[] externalEncryptedDataPass = ReceiveMessage(buffer);

            try
            {
                byte[] decryptedDataPass = serverEncryptDecryptor.DecryptData(externalEncryptedDataPass);

                // Print the result.
                Console.WriteLine($"Decrypted message: \n{Encoding.UTF8.GetString(decryptedDataPass)}\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                // Print data just to see what it looks like.
                Console.WriteLine($"Could not decrypt message: \n{Encoding.UTF8.GetString(externalEncryptedDataFail)}\n");
            }

            Console.ReadLine();
        }

        /// <summary>Stops the server and frees resources.</summary>
        public void Stop()
        {
            handler!.Disconnect(false);
            handler!.Dispose();
            server.Dispose();
        }

        /// <summary>Sends the bytes to the server.</summary>
        /// <param name="buffer">The buffer to send.</param>
        /// <param name="startingOffset">The starting offset to send the data from in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if all bytes were sent, false otherwise.</returns>
        private bool SendBytes(byte[] buffer, int startingOffset, int length)
        {
            int totalSent = 0;

            while (totalSent != length)
            {
                int sentBytes;
                try
                {
                    sentBytes = handler!.Send(buffer, startingOffset + totalSent, length - totalSent, SocketFlags.None);
                }
                catch (SocketException)
                {
                    return false;
                }

                totalSent += sentBytes;
            }

            return true;
        }

        /// <summary>Encrypts the <see cref="message"/> and then sends it to the client.</summary>
        /// <param name="message">The string to send.</param>
        /// <param name="useClientEncryptorOnly">Whether or not to encrypt the data using the client encryptor. Uses this server's encrypt/decryptor if available if false (default).</param>
        /// <exception cref="IOException"></exception>
        private void SendEncryptedMessage(string message, bool useClientEncryptorOnly)
        {
            // Encrypt the secret message.
            byte[] secretMessageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encryptedMessage = EncryptSecretMessage(secretMessageBytes, useClientEncryptorOnly);

            // Display secret message and encrypted message.
            Console.WriteLine($"Sending secret message: {message}");
            Console.WriteLine($"Encrypted secret message: \n{Encoding.UTF8.GetString(encryptedMessage)}\n");

            if (!SendBytes(encryptedMessage, 0, encryptedMessage.Length))
            {
                throw new IOException(IoExMsg);
            }
        }

        /// <summary>Encrypts the secret message using the chosen encryptor object.</summary>
        /// <param name="message">The message.</param>
        /// <param name="useClientEncryptorOnly">True to use <see cref="clientEncryptorOnly"/>, false to use <see cref="serverEncryptDecryptor"/> (false by default).
        /// <returns>A byte array containing the encrypted message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided message is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided message is empty.</exception>
        private byte[] EncryptSecretMessage(byte[] message, bool useClientEncryptorOnly = false)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (message.Length == 0)
            {
                throw new ArgumentException($"Message content is empty.", nameof(message));
            }

            return (useClientEncryptorOnly && clientEncryptorOnly is not null) ? clientEncryptorOnly!.EncryptData(message) : serverEncryptDecryptor!.EncryptData(message);
        }

        /// <summary>Receives a message from the client.</summary>
        /// <param name="buffer">The buffer to use for receiving messages.</param>
        /// <returns>A byte array containing the message received.</returns>
        private byte[] ReceiveMessage(byte[] buffer)
        {
            Array.Clear(buffer);
            ushort bytesRead = 0;

            while (bytesRead == 0)
            {
                bytesRead += (ushort)handler!.Receive(buffer);
            }

            byte[] message = new byte[bytesRead];
            Array.Copy(buffer, message, bytesRead);

            return message;
        }
    }
}
