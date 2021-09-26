using System.IO;
using DiscordSocialProvider.Patches;
using MelonLoader;


namespace DiscordSocialProvider
{
    public static class ModInfo
    {
        public const string Name = "Discord Social Provider";
        public const string Version = "1.0.0";
    }
    
    public class DiscordSocialProviderMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            var discordSdkPath = ExtractDiscordSdk();
            NativeLibrary.Load(discordSdkPath);
        }

        private static string ExtractDiscordSdk()
        {
            // @todo: replace with MelonUtils.UserLibsDirectory with ML 0.4.4
            var userLibsDir = Path.Combine(MelonUtils.GameDirectory, "UserLibs");
            
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