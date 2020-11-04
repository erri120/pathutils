using System;
using PathUtils.Verbs;

namespace PathUtils
{
    public static class OptionsDefinition
    {
        public static readonly Type[] AllOptions =
        {
            typeof(Clean),
            typeof(Restore)
        };
    }
}
