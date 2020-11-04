using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using CommandLine;

namespace PathUtils.Verbs
{
    [Verb("clean", HelpText = "Clean the PATH variable of non-existing and duplicate paths.")]
    public class Clean : AVerb
    {
        [Option('d', HelpText = "Remove duplicates.", Default = true)]
        public bool RemoveDuplicates { get; set; } = true;

        [Option('n', HelpText = "Remove paths that do not exist.", Default = true)]
        public bool RemoveNotExisting { get; set; } = true;

        protected override async Task<ExitCode> Run()
        {
            string? current;
            try
            {
                current = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            }
            catch (Exception e)
            {
                CLIUtils.LogException(e, "Unable to read PATH variable!");
                return ExitCode.Error;
            }

            if (current == null)
            {
                CLIUtils.Log("PATH variable is null!");
                return ExitCode.Error;
            }
            
            CLIUtils.Log("Creating Backup...");
            BackupsHandler.SaveBackup(current);
            
            CLIUtils.Log("Cleaning variable...");
            List<string> values = current.Split(";")
                .Select(x =>
                {
                    var r = x.Replace("/", "\\");
                    if (r.EndsWith("\\"))
                        r = r.Substring(0, r.Length - 1);
                    return r;
                })
                .ToList();
            
            if (RemoveDuplicates)
            {
                var count = values.Count;
                values = values.Distinct().ToList();
                CLIUtils.Log($"Removed {count-values.Count} duplicates");
            }

            if (RemoveNotExisting)
            {
                var removed = values.RemoveAll(x => !Directory.Exists(x));
                CLIUtils.Log($"Removed {removed} not existing paths");
            }
            
            CLIUtils.Log($"Finished cleaning, before: {current.Split(";").Length}, after: {values.Count}");

            try
            {
                Environment.SetEnvironmentVariable("Path", values.Aggregate((x, y) => $"{x};{y}"),
                    EnvironmentVariableTarget.Machine);
                return ExitCode.Ok;
            }
            catch (SecurityException securityException)
            {
                CLIUtils.LogException(securityException, "Unable to set PATH due to Security Exception, try running the program as Admin");
                return ExitCode.Error;
            }
            catch (Exception e)
            {
                CLIUtils.LogException(e, "Unable to set PATH variable!");
                return ExitCode.Error;
            }
        }
    }
}
