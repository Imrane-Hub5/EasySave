using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace EasySave.Services
{
    /// <summary>
    /// Handles file encryption using the external CryptoSoft tool.
    /// CryptoSoft is mono-instance — a Mutex ensures only one instance runs at a time.
    /// </summary>
    public class CryptoSoftService
    {
        private readonly string _cryptoSoftPath;

        // Global Mutex — shared across all threads and processes
        private static readonly Mutex _cryptoSoftMutex = new Mutex(false, "Global\\CryptoSoftMutex");

        public CryptoSoftService(string cryptoSoftPath)
        {
            _cryptoSoftPath = cryptoSoftPath;
        }

        /// <summary>
        /// Returns true if the file extension matches the encrypted extensions list
        /// </summary>
        public bool ShouldEncrypt(string filePath, List<string> encryptedExtensions)
        {
            if (string.IsNullOrEmpty(_cryptoSoftPath) || encryptedExtensions.Count == 0)
                return false;
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            return encryptedExtensions.Contains(ext);
        }

        /// <summary>
        /// Encrypts a file using CryptoSoft.
        /// Waits for the Mutex before launching — ensures mono-instance.
        /// Returns encryption time in ms, or negative value on error.
        /// </summary>
        public long EncryptFile(string filePath)
        {
            try
            {
                // Wait for the Mutex — blocks if another thread is already encrypting
                _cryptoSoftMutex.WaitOne();

                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = _cryptoSoftPath,
                        Arguments = $"\"{filePath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Stopwatch sw = Stopwatch.StartNew();
                    Process? process = Process.Start(psi);
                    process?.WaitForExit();
                    sw.Stop();

                    if (process?.ExitCode != 0)
                        return -1;

                    return sw.ElapsedMilliseconds;
                }
                finally
                {
                    // Always release the Mutex even if an error occurs
                    _cryptoSoftMutex.ReleaseMutex();
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
