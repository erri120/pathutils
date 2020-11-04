using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathUtils
{
    public static class BackupsHandler
    {
        private const string FileName = "backups.json";
        
        public static readonly List<Backup> Backups;
        
        static BackupsHandler()
        {
            if (!File.Exists(FileName))
                Backups = new List<Backup>();
            else
            {
                var res = JSONUtils.Deserialize<List<Backup>>(FileName);
                if (res != null)
                {
                    Backups = res;
                    return;
                }

                CLIUtils.Log($"{FileName} is corrupted, deleting file!");
                File.Delete(FileName);
                Backups = new List<Backup>();
            }
        }
        
        public static Backup? LoadBackup(int id)
        {
            var backup = Backups.FirstOrDefault(x => x.ID.Equals(id));
            return backup;
        }
        
        public static void SaveBackup(string value)
        {
            var id = Backups.Count == 0 ? 1 : Backups.Select(x => x.ID).Max() + 1;
            var backup = new Backup(id, value);
            Backups.Add(backup);
            JSONUtils.Serialize(FileName, Backups);
        }
    }

    public class Backup
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public DateTime Time { get; set; }

        public Backup(int id, string value)
        {
            ID = id;
            Value = value;
            Time = DateTime.Now;
        }
    }
}
