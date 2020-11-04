using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace PathUtils.Verbs
{
    [Verb("restore", HelpText = "Restore a backup.")]
    public class Restore : AVerb
    {
        [Option("id", HelpText = "ID of the backup to restore.", Required = false)]
        public int ID { get; set; } = -1;

        protected override async Task<ExitCode> Run()
        {
            if (ID == -1)
            {
                Console.WriteLine("{0,-3} {1,-21} {2,-4}", "ID", "Time", "Values");
                foreach (var x in BackupsHandler.Backups)
                {
                    Console.WriteLine("{0,-3} {1,-21} {2,-4}", $"{x.ID}", $"{x.Time:G}", $"{x.Value.Count(c => c.Equals(';'))}");
                }
                return ExitCode.Ok;
            }
            
            var backup = BackupsHandler.LoadBackup(ID);
            if (backup == null)
                return CLIUtils.Exit($"Unable to find Backup with ID {ID}!", ExitCode.Error);

            CLIUtils.Log($"Loaded Backup with ID {backup.ID} from {backup.Time:f}");

            try
            {
                Environment.SetEnvironmentVariable("Path", backup.Value, EnvironmentVariableTarget.Machine);
                return ExitCode.Ok;
            } catch (Exception e)
            {
                CLIUtils.LogException(e, "Unable to set PATH variable!");
                return ExitCode.Error;
            }
        }
    }
}
