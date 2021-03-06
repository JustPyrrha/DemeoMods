using System.IO;
using DiscordSocialProvider.Patches;
using MelonLoader;


namespace DiscordSocialProvider
{
    public static class ModInfo
    {
        public const string Name = "Discord Social Provider";
        public const string Version = "1.3.0";
    }
    
    public class DiscordSocialProviderMod : MelonMod
    {
        internal static MelonLogger.Instance Logger = new MelonLogger.Instance(ModInfo.Name);
        
        public override void OnApplicationStart()
        {
            var discordSdkPath = ExtractDiscordSdk();
            NativeLibrary.Load(discordSdkPath);
        }

        private static string ExtractDiscordSdk()
        {
            var userLibsDir = MelonUtils.UserLibsDirectory;
            
            var path = Path.Combine(userLibsDir, "discord_game_sdk.dll");
            if (!File.Exists(path))
            {
                if (!Directory.Exists(userLibsDir))
                    Directory.CreateDirectory(userLibsDir);
                
                File.WriteAllBytes(path, Properties.Resources.discord_game_sdk);
            }
            
            return path;
        }

        public override void OnApplicationQuit()
        {
            SocialHubPatches.DiscordSocialProvider?.Dispose();
        }
    }
}