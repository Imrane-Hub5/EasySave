using System;
using System.IO;
using System.Threading;

namespace CryptoSoft
{
    /// <summary>
    /// CryptoSoft — File encryption tool using XOR algorithm
    /// Mono-instance — only one instance can run at a time on the same machine
    /// Usage: CryptoSoft.exe "path/to/file"
    /// Exit codes: 0 = success, -1 = error
    /// </summary>
    class Program
    {
        private static readonly byte[] Key = { 0x4B, 0x65, 0x79, 0x21, 0x40, 0x23, 0x24, 0x25 };
        private const string MutexName = "Global\\CryptoSoftMutex";

        static int Main(string[] args)
        {
            // Try to acquire the global Mutex
            bool createdNew;
            using Mutex mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                Console.WriteLine("CryptoSoft is already running. Please wait.");
                return -1;
            }

            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Usage: CryptoSoft.exe \"path/to/file\"");
                    return -1;
                }

                string filePath = args[0];

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: File not found — {filePath}");
                    return -1;
                }

                byte[] fileBytes = File.ReadAllBytes(filePath);
                byte[] encryptedBytes = XorEncrypt(fileBytes);
                File.WriteAllBytes(filePath, encryptedBytes);

                Console.WriteLine($"File encrypted successfully: {filePath}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during encryption: {ex.Message}");
                return -1;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private static byte[] XorEncrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = (byte)(data[i] ^ Key[i % Key.Length]);
            return result;
        }
    }
}
