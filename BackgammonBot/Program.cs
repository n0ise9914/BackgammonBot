using System;
using System.Text;
using BackgammonBot.Utils;
using MoonSharp.Interpreter;

namespace BackgammonBot
{
    static class Program
    {  
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Script.DefaultOptions.ScriptLoader = new CustomResourceLoader();
            App.GetInstance().Init();
            Console.ReadLine();
        }
    }
    
}