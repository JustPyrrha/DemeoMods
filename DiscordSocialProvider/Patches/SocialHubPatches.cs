using System;
using Boardgame.Social;
using HarmonyLib;

namespace DiscordSocialProvider.Patches
{
    [HarmonyPatch]
    public class SocialHubPatches
    {
        private static Action<ISocialProvider, JoinParameters> _onJoinReceived;
        public static DiscordSocialProviderImpl DiscordSocialProvider;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SocialHub), MethodType.Constructor, typeof(StringToRoomCode), typeof(Action<ISocialProvider, JoinParameters>))]
        public static void Constructor(SocialHub __instance, Action<ISocialProvider, JoinParameters> onJoinReceived)
        {
            _onJoinReceived = onJoinReceived;
            DiscordSocialProvider = new DiscordSocialProviderImpl(_onJoinReceived);
            AddProvider(__instance, DiscordSocialProvider);
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(SocialHub), "AddProvider")]
        public static void AddProvider(SocialHub instance, ISocialProvider provider)
        {
            throw new NotImplementedException("[PATCH] SocialHubAddProviderPatch.AddProvider -> SocialHub.AddProvider");
        }
    }
}