using MoonSharp.Interpreter;

namespace BackgammonBot.Utils
{
    public class LuaWrapper
    {
        public Script Script { get; }


        public LuaWrapper(string name)
        {
            var res = ResourceManager.GetInstance();
                Script = res.LoadScript(name);
        }

        public DynValue CallModule(string module, string method, params object[] args)
        {
            return Script.Call(((Table) Script.Globals[module]).Get(method), args);
        }

        public DynValue Call(string module, string method)
        {
            return Script.Call(((Table) Script.Globals[module]).Get(method));
        }

        public DynValue Call(string method)
        {
            return Script.Call(Script.Globals[method]);
        }

        public DynValue Call(string method, params object[] args)
        {
            return Script.Call(Script.Globals[method], args);
        }
    }
}