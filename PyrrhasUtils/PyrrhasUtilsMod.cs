using System;
using MelonLoader;

namespace PyrrhasUtils
{
    public static class ModInfo
    {
        public const string Name = "Pyrrha's Utils";
        public const string Version = "1.3.0";
    }

    public class PyrrhasUtilsMod : MelonMod
    {
        internal static MelonLogger.Instance Logger = new MelonLogger.Instance(ModInfo.Name);
        
        public override void OnApplicationStart()
        {
            Logger.Msg("Trans Rights are Human Rights");
            Logger.Msg(ConsoleColor.Cyan,    "===============");
            Logger.Msg(ConsoleColor.Magenta, "===============");
            Logger.Msg(ConsoleColor.White,   "===============");
            Logger.Msg(ConsoleColor.Magenta, "===============");
            Logger.Msg(ConsoleColor.Cyan,    "===============");
        }
    }
}