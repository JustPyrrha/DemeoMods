using Boardgame;
using HarmonyLib;

namespace PyrrhasUtils.Patches
{
    [HarmonyPatch]
    public static class ContextPatch
    {
        public static GameContext Context;

        [HarmonyPatch(typeof(GameStartup), MethodType.Constructor)]
        public static void Postfix(ref GameContext ___gameContext)
        {
            Context = ___gameContext;
        }
    }
}