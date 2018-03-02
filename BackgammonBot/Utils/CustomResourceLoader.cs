using System.Reflection;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace BackgammonBot.Utils
{
    public class CustomResourceLoader : ScriptLoaderBase
    {
        public override bool ScriptFileExists(string name)
        {
            return true;
        }

        public override object LoadFile(string file, Table globalContext)
        {
            return Assembly.GetEntryAssembly().GetManifestResourceStream(file);
        }

        protected override string ResolveModuleName(string modname, string[] paths)
        {
            return "BackgammonBot." + modname + ".lua";
        }
    }
}