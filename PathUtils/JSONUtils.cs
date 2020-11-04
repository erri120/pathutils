using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PathUtils
{
    public static class JSONUtils
    {
        private static readonly JsonSerializer Serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        });
        
        public static T? Deserialize<T>(string path) where T : class
        {
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist", nameof(path));

            using var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            
            var result = Serializer.Deserialize<T>(new JsonTextReader(sr));
            return result;
        }

        public static void Serialize<T>(string path, T obj) where T : class
        {
            using var fs = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            using var sw = new StreamWriter(fs, Encoding.UTF8);
            
            Serializer.Serialize(sw, obj);
        }
    }
}
