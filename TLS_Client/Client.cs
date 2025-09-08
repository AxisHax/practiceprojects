//	<copyright file="Client.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Client.
//	</summary>
namespace TLS_Client
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using SocketDataSecurity;

    /// <summary>
    /// Server class to demonstrate sending/receiving encrypted messages to and from a client.
    /// </summary>
    internal class Client
    {
        /// <summary>The TCP port number to connect to.</summary>
        private const int Port = 5000;

        /// <summary>The host name.</summary>
        private const string HostEntry = "localhost";

        /// <summary>IO exception message to print when message sending fails.</summary>
        private const string IoExMsg = "Unable to send data.";

        /// <summary>The Socket used for connections.</summary>
        private readonly Socket client;

        /// <summary>Host entry object.</summary>
        private readonly IPHostEntry host;

        /// <summary>IP address object.</summary>
        private readonly IPAddress ip;

        /// <summary>The local end point object.</summary>
        private readonly IPEndPoint localEndPoint;

        /// <summary>Object used to encrypt data using the server's public key. Cannot decrypt data encrypted using said key.</summary>
        private DataCryptography? serverEncryptorOnly;

        /// <summary>
        /// Object used to encrypt and decrypt data using it's private key. Can only decrypt data encrypted using it's public key.
        /// Send it's public key to the server for them to encrypt data with.
        /// </summary>
        private DataCryptography? clientEncryptDecryptor;

        /// <summary>The buffer used for data transmission.</summary>
        private readonly byte[] buffer;

        /// <summary>Initializes a new instance of the <see cref="Client"/> class.</summary>
        /// <param name="targetHost">The target host.</param>
        public Client(string targetHost = HostEntry)
        {
            host = Dns.GetHostEntry(targetHost);
            ip = host.AddressList[0];
            localEndPoint = new(ip, Port);
            client = new(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            clientEncryptDecryptor = new DataCryptography();
            buffer = new byte[1024];
        }

        /// <summary>Runs the client.</summary>
        /// <exception cref="IOException">Exception thrown when data is unable to be sent to the client.</exception>
        public void Run()
        {
            // Connect to the server.
            client.Connect(localEndPoint);
            Console.WriteLine($"Client\n\nConnected to {client.RemoteEndPoint}!");

            // Send our public key to the server.
            byte[] internalPublicKey = clientEncryptDecryptor!.ExportPublicKey();

            if (!SendBytes(internalPublicKey, 0, internalPublicKey.Length))
            {
                throw new IOException(IoExMsg);
            }

            // Get encrypted message from the server which was encrypted using our public key.
            byte[] externalEncryptedData = ReceiveMessage(buffer);
            Console.WriteLine($"Received encrypted message: \n{Encoding.UTF8.GetString(externalEncryptedData)}\n");

            string s = "";

            // Decrypt the message.
            try
            {
                byte[] decryptedData = clientEncryptDecryptor.DecryptData(externalEncryptedData);
                s = Encoding.UTF8.GetString(decryptedData);

                // Print the result.
                Console.WriteLine($"Decrypted message: {s}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                // Print data just to see what it looks like.
                Console.WriteLine($"Could not decrypt message: \n{Encoding.UTF8.GetString(externalEncryptedData)}\n");
            }

            // Edit then send the message encrypted using our public key. This should fail on the server side.
            s += " Client touched this message.";
            SendEncryptedMessage(s, false);

            // Get the server public key and assign it to our serverEncryptorOnly object.
            byte[] externalPublicKey = ReceiveMessage(buffer);
            serverEncryptorOnly = new DataCryptography(externalPublicKey);

            // Send the message again but encrypted with the server's public key this time. It should work.
            SendEncryptedMessage(s, true);

        }

        /// <summary>Stops the <see cref="Client"/>.</summary>
        public void Stop()
        {
            // Disconnect.
            client.Disconnect(false);
            client.Dispose();
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
                    sentBytes = client!.Send(buffer, startingOffset + totalSent, length - totalSent, SocketFlags.None);
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
        /// <param name="useServerEncryptorOnly">Whether or not to encrypt the data using the server encryptor. Uses this client's encrypt/decryptor if available if false (default).</param>
        /// <exception cref="IOException"></exception>
        private void SendEncryptedMessage(string message, bool useServerEncryptorOnly)
        {
            // Encrypt the secret message.
            byte[] secretMessageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encryptedMessage = EncryptSecretMessage(secretMessageBytes, useServerEncryptorOnly);

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
        /// <param name="useServerEncryptorOnly">True to use <see cref="serverEncryptorOnly"/>, false to use <see cref="clientEncryptDecryptor"/> (false by default).</param>
        /// <returns>A byte array containing the encrypted message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided message is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided message is empty.</exception>
        private byte[] EncryptSecretMessage(byte[] message, bool useServerEncryptorOnly = false)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (message.Length == 0)
            {
                throw new ArgumentException($"Message content is empty.", nameof(message));
            }

            return (useServerEncryptorOnly && serverEncryptorOnly is not null) ? serverEncryptorOnly!.EncryptData(message) : clientEncryptDecryptor!.EncryptData(message);
        }

        /// <summary>Receives a message from the server.</summary>
        /// <param name="buffer">The buffer to use for receiving messages.</param>
        /// <returns>A byte array containing the message received.</returns>
        private byte[] ReceiveMessage(byte[] buffer)
        {
            Array.Clear(buffer);
            ushort bytesRead = 0;

            while (bytesRead == 0)
            {
                bytesRead += (ushort)client!.Receive(buffer);
            }

            byte[] message = new byte[bytesRead];
            Array.Copy(buffer, message, bytesRead);

            return message;
        }
    }
}
