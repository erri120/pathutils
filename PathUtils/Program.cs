using CommandLine;
using PathUtils.Verbs;

namespace PathUtils
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments(args, OptionsDefinition.AllOptions)
                .MapResult(
                    (AVerb opts) => opts.Execute(),
                    err => 1);
        }
    }
}
