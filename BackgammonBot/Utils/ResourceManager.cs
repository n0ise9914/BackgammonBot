using System;
using System.IO;
using System.Reflection;
using MoonSharp.Interpreter;

namespace BackgammonBot.Utils
{
    public class ResourceManager
    {
        private static ResourceManager _instance;

        private ResourceManager()
        {       
        }

        public static ResourceManager GetInstance()
        {
            return _instance ?? (_instance = new ResourceManager());
        }

        public Script LoadScript(string name)
        {
            name = "BackgammonBot." + name + ".lua";
            Script script = new Script();
            string raw = LoadFile(name);
            
            try
            {
                script.DoString(raw);
            }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Doh! An error occured! {0}", ex.DecoratedMessage);
            }
            
       
            return script;
        }

        public string LoadFile(string name)
        {
            var resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(name);

            using (var reader = new StreamReader(resourceStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}