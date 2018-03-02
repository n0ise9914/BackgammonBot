using System;
using System.Diagnostics;
using MoonSharp.Interpreter;

namespace BackgammonBot
{
    public class App
    {
        private readonly Api _api = Api.GetInstance();

        private static App _instance;

        public static App GetInstance()
        {
            return _instance ?? (_instance = new App());
        }

        private App()
        {
        }

        public void Init()
        {            
            Console.WriteLine("Verify: " + _api.Verify(4199819896).message);
            Console.WriteLine("GetProfile: " + _api.GetProfile().profile.name);
            BackgammonBot bot = new BackgammonBot(_api);
            
            try
            {
                bot.Start();
            }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Doh! An error occured! {0}", ex.DecoratedMessage);
            }
        }      
    }
}