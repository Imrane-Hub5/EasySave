using System;
using System.IO;

namespace EasySave.Strategies
{
    public class DiffBackupStrategy : IBackupStrategy
    {
        // Nom affiché dans les logs ou l'interface
        public string GetTypeName() => "Differential";

        /// Vérifie si le fichier source doit être copié vers la cible.
      
        public bool ShouldCopy(string sourcePath, string targetPath)
        {
            // Si le fichier n'existe pas encore à destination, on doit le copier
            if (!File.Exists(targetPath))
            {
                return true;
            }

            // On récupère la date de dernière modification des deux fichiers
            DateTime sourceDate = File.GetLastWriteTime(sourcePath);
            DateTime targetDate = File.GetLastWriteTime(targetPath);

            // On ne copie que si la source est plus récente que la destination
            return sourceDate > targetDate;
        }

        public void Execute(string src, string dst)
        {
            
        }
    }
}
