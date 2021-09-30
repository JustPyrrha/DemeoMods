using MelonLoader;
using UnityEngine;

namespace Clock
{
    
    public static class ModInfo
    {
        public const string Name = "Clock";
        public const string Version = "1.0.0";
    }
    
    public class ClockMod : MelonMod
    {
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "LobbySteamVR" || sceneName == "Lobby")
            {
                new GameObject("ClockMod").AddComponent<ClockBehaviour>();
            }
        }
    }
}