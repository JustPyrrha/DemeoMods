

using System;
using MelonLoader;

namespace PyrrhasUtils
{
    public static class ModInfo
    {
        public const string Name = "Pyrrha's Utils";
        public const string Version = "1.0.0";
    }
    
    public class PyrrhasUtilsMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Trans Rights are Human Rights");
            MelonLogger.Msg(ConsoleColor.Cyan,    "===============");
            MelonLogger.Msg(ConsoleColor.Magenta, "===============");
            MelonLogger.Msg(ConsoleColor.White,   "===============");
            MelonLogger.Msg(ConsoleColor.Magenta, "===============");
            MelonLogger.Msg(ConsoleColor.Cyan,    "===============");
        }
    }
}