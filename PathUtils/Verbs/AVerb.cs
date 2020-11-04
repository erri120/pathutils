using System.Threading.Tasks;

namespace PathUtils.Verbs
{
    public abstract class AVerb
    {
        public int Execute()
        {
            return (int) Run().Result;
        }

        protected abstract Task<ExitCode> Run();
    }
}
