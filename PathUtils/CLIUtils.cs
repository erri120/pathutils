using System;

namespace PathUtils
{
    public enum ExitCode
    {
        BadArguments = -1,
        Ok = 0,
        Error = 1
    }
    
    public static class CLIUtils
    {
        public static void Log(string message, bool newLine = true)
        {
            if (newLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
            Console.Out.Flush();
        }

        public static ExitCode Exit(string msg, ExitCode code)
        {
            Log(msg);
            return code;
        }
        
        public static void LogException(Exception e, string msg)
        {
            Log($"{msg}\n{e}");
        }
    }
}
