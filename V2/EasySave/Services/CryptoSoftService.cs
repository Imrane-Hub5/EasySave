using System;
using System.Diagnostics;

namespace EasySave.Services
{
    /// <summary>
    /// Handles file encryption using the external CryptoSoft tool
    /// </summary>
    public class CryptoSoftService
    {
        private readonly string _cryptoSoftPath;

        public CryptoSoftService(string cryptoSoftPath)
        {
            _cryptoSoftPath = cryptoSoftPath;
        }

        public bool ShouldEncrypt(string filePath, List<string> encryptedExtensions)
        {
            if (string.IsNullOrEmpty(_cryptoSoftPath) || encryptedExtensions.Count == 0)
                return false;
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            return encryptedExtensions.Contains(ext);
        }

        /// <summary>
        /// Encrypts a file using CryptoSoft.
        /// Returns encryption time in ms, or negative value on error.
        /// </summary>
        public long EncryptFile(string filePath)
        {
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
            catch (Exception)
            {
                return -1;
            }
        }
    }
}