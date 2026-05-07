using System;
using EasySave.Models;
using EasySave.Services;
using EasyLog; 

// --- V1.1 : Initial configuration ---
ILogFormatter selectedFormatter = SelectLogFormat();
Logger.GetInstance().SetFormatter(selectedFormatter);

if (args.Length > 0)
{
    BackupManager.GetInstance().ExecuteRange(args[0]);
    return;
}

// --- Interactive mode ---
LanguageManager lang = SelectLanguage();
BackupManager manager = BackupManager.GetInstance();
bool running = true;

while (running)
{
    ShowMenu(lang);
    string? choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1": AddJob(manager, lang);                                           break;
        case "2": ListJobs(manager, lang);                                         break;
        case "3": ExecuteJob(manager, lang);                                       break;
        case "4": manager.RunAll(); Console.WriteLine(lang.Get("all_done"));       break;
        case "5": RemoveJob(manager, lang);                                        break;
        case "6": running = false;                                                 break;
        default:  Console.WriteLine(lang.Get("invalid_choice"));                   break;
    }

    if (running)
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}

// --- Logic for V1.1 Log Format Selection ---
static ILogFormatter SelectLogFormat()
{
    Console.WriteLine("Select Log Format / Choisissez le format des logs:");
    Console.WriteLine("1. JSON");
    Console.WriteLine("2. XML");
    Console.Write("> ");
    string? choice = Console.ReadLine();
    Console.Clear();
    
    if (choice == "2")
    {
        return new XmlFormatter(); 
    }
    return new JsonFormatter(); 
}

static LanguageManager SelectLanguage()
{
    Console.WriteLine("Select language / Choisissez la langue:");
    Console.WriteLine("1. English");
    Console.WriteLine("2. Français");
    Console.Write("> ");
    string? choice = Console.ReadLine();
    Language locale = choice == "2" ? Language.French : Language.English;
    Console.Clear();
    return new LanguageManager(locale);
}

static void ShowMenu(LanguageManager lang)
{
    Console.WriteLine(lang.Get("menu_title"));
    Console.WriteLine(lang.Get("menu_add"));
    Console.WriteLine(lang.Get("menu_list"));
    Console.WriteLine(lang.Get("menu_execute"));
    Console.WriteLine(lang.Get("menu_execute_all"));
    Console.WriteLine(lang.Get("menu_remove"));
    Console.WriteLine(lang.Get("menu_quit"));
    Console.Write(lang.Get("menu_choice"));
}

static void AddJob(BackupManager manager, LanguageManager lang)
{
    Console.Write(lang.Get("job_name"));
    string name = Console.ReadLine() ?? string.Empty;
    Console.Write(lang.Get("job_source"));
    string source = Console.ReadLine() ?? string.Empty;
    Console.Write(lang.Get("job_target"));
    string target = Console.ReadLine() ?? string.Empty;
    Console.Write(lang.Get("job_type"));
    BackupType type = Console.ReadLine() == "2" ? BackupType.Differential : BackupType.Complete;
    bool added = manager.AddJob(new BackupJob(name, source, target, type));
    Console.WriteLine(added ? lang.Get("job_added") : lang.Get("job_max"));
}

static void ListJobs(BackupManager manager, LanguageManager lang)
{
    if (manager.Jobs.Count == 0) { Console.WriteLine(lang.Get("job_none")); return; }
    for (int i = 0; i < manager.Jobs.Count; i++)
    {
        BackupJob job = manager.Jobs[i];
        Console.WriteLine($"{i + 1}. [{job.Type}] {job.Name} | {job.SourcePath} -> {job.TargetPath}");
    }
}

static void ExecuteJob(BackupManager manager, LanguageManager lang)
{
    ListJobs(manager, lang);
    if (manager.Jobs.Count == 0) return;
    Console.Write(lang.Get("job_number"));
    if (int.TryParse(Console.ReadLine(), out int index))
    {
        manager.RunJob(index - 1);
        Console.WriteLine(lang.Get("backup_done"));
    }
    else Console.WriteLine(lang.Get("invalid_number"));
}

static void RemoveJob(BackupManager manager, LanguageManager lang)
{
    ListJobs(manager, lang);
    if (manager.Jobs.Count == 0) return;
    Console.Write(lang.Get("job_number"));
    if (int.TryParse(Console.ReadLine(), out int index))
    {
        bool removed = manager.RemoveJob(index - 1);
        Console.WriteLine(removed ? lang.Get("job_removed") : lang.Get("invalid_number"));
    }
    else Console.WriteLine(lang.Get("invalid_number"));
}
