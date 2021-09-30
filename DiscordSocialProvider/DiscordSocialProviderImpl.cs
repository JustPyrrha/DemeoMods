using System;
using Boardgame.Social;
using MelonLoader;
using PyrrhasUtils.Patches;
using DiscordSdk = Discord;

namespace DiscordSocialProvider
{
    public class DiscordSocialProviderImpl : ISocialProvider
    {
        private readonly DiscordSdk.Discord _discord;
        private DiscordSdk.Activity _activity;

        public DiscordSocialProviderImpl(Action<ISocialProvider, JoinParameters> onJoinReceived)
        {
            this._discord = new DiscordSdk.Discord(844058872573722674, (ulong)DiscordSdk.CreateFlags.NoRequireDiscord);
            this._discord.SetLogHook(DiscordSdk.LogLevel.Debug,
                (level, message) => MelonLogger.Msg($"(Discord|{level.ToString().ToUpper()}) {message}"));
            this._discord.GetActivityManager().RegisterSteam(1484280);

            // Set Activity defaults
            this._activity.Assets.SmallImage = "pyrrha";
            this._activity.Assets.SmallText = $"Mod by PyrrhaDev | v{ModInfo.Version}";
            this._activity.Assets.LargeImage = "logo";
            this._activity.Assets.LargeText = $"Demeo v{RGVersion.VERSION}";

            this._discord.GetActivityManager().OnActivityJoin += secret =>
            {
                var parts = secret.Split(',');
                onJoinReceived(this, new JoinParameters
                {
                    gameId = parts[0],
                    groupId = parts[1],
                    status = parts[2]
                });
            };
        }

        public bool IsReady() => true;

        public void Tick()
        {
            this._discord?.RunCallbacks();
        }

        public void SetCurrentStatus(string statusID, bool isJoinable, string gameID, string groupID,
            Action<ISocialProvider, bool> onDone)
        {
            switch (statusID)
            {
                case "GameState_MainMenu":
                {
                    this._activity.Details = "In the Lobby";
                    this._activity.State = "";
                    this._activity.Assets.LargeImage = "logo";
                    break;
                }
                case "GameState_Tutorial":
                {
                    this._activity.Details = "In the tutorial";
                    this._activity.State = "";
                    this._activity.Assets.LargeImage = "logo";
                    break;
                }
                case "GameState_Skirmish":
                {
                    this._activity.Details = "In Adventure";
                    this._activity.State = "Playing Skirmish";
                    this._activity.Assets.LargeImage = "logo";
                    
                    break;
                }
                case "Adventure_TheBlackSarcophagus":
                {
                    this._activity.Details = "In Adventure";
                    this._activity.State = "The Black Sarcophagus";
                    this._activity.Assets.LargeImage = "book1";
                    break;
                }
                case "Adventure_RealmOfTheRatKing":
                {
                    this._activity.Details = "In Adventure";
                    this._activity.State = "Realm Of The Rat King";
                    this._activity.Assets.LargeImage = "book2";
                    break;
                }
            }
            
            if (groupID != "")
            {
                this._activity.Party.Id = groupID;
                this._activity.Party.Size = new DiscordSdk.PartySize
                {
                    CurrentSize = PhotonNetwork.playerList != null ? PhotonNetwork.playerList.Length : 1,
                    MaxSize = 4
                };
            }
            else
            {
                this._activity.Party = new DiscordSdk.ActivityParty();
            }
            
            if (isJoinable && gameID != "")
            {
                this._activity.Secrets.Join = string.Join(",", gameID, groupID, statusID);
            }
            else
            {
                this._activity.Secrets.Join = null;
            }
            this._activity.Timestamps.Start = DateTimeOffset.Now.ToUnixTimeSeconds();
            
            this._discord.GetActivityManager().UpdateActivity(this._activity, result =>
            {
                onDone(this, result == DiscordSdk.Result.Ok);
                MelonLogger.Msg($"(Discord|INFO): Updating status result: {result.ToString()}");
            });
        }

        public void Dispose()
        {
            this._discord?.Dispose();
        }
    }
}