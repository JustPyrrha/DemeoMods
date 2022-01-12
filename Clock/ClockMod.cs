using MelonLoader;
using PyrrhasUtils.Utilities;
using UnityEngine;

namespace Clock
{
    public static class ModInfo
    {
        public const string Name = "Clock";
        public const string Version = "1.2.0";
    }
    
    public class ClockMod : MelonMod
    {
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (SceneUtilities.IsLobbyScene(sceneName))
            {
                new GameObject("ClockMod").AddComponent<ClockBehaviour>();
            }
        }
    }
}