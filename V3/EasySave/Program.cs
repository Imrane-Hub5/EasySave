using EasySave.Services;

namespace EasySave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                BackupManager.GetInstance().ExecuteRange(args[0]);
        }
    }
}
