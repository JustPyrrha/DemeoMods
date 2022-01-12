namespace PyrrhasUtils.Utilities
{
    public static class SceneUtilities
    {
        public static bool IsLobbyScene(string sceneName) =>
            sceneName.Equals("LobbySteamVR") || sceneName.Equals("Lobby");
    }
}