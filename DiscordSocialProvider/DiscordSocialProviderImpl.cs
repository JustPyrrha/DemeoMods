using System;
using Boardgame.Social;
using MelonLoader;
using Photon.Pun;
using PyrrhasUtils.Patches;
using DiscordSdk = Discord;

namespace DiscordSocialProvider
{
    public class DiscordSocialProviderImpl : ISocialProvider
    {
        private readonly DiscordSdk.Discord Discord;
        private DiscordSdk.Activity Activity;

        public DiscordSocialProviderImpl(Action<ISocialProvider, JoinParameters> onJoinReceived)
        {
            this.Discord = new DiscordSdk.Discord(844058872573722674, (ulong)DiscordSdk.CreateFlags.NoRequireDiscord);
            this.Discord.SetLogHook(DiscordSdk.LogLevel.Debug,
                (level, message) => DiscordSocialProviderMod.Logger.Msg($"(Discord|{level.ToString().ToUpper()}) {message}"));
            this.Discord.GetActivityManager().RegisterSteam(1484280);

            // Set Activity defaults
            this.Activity.Assets.SmallImage = "pyrrha";
            this.Activity.Assets.SmallText = $"Mod by PyrrhaDev | v{ModInfo.Version}";
            this.Activity.Assets.LargeImage = "logo";
            this.Activity.Assets.LargeText = $"Demeo v{RGVersion.VERSION}";

            this.Discord.GetActivityManager().OnActivityJoin += secret =>
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
            this.Discord?.RunCallbacks();
        }

        public void SetCurrentStatus(string statusID, bool isJoinable, string gameID, string groupID,
            Action<ISocialProvider, bool> onDone)
        {
            switch (statusID)
            {
                case "GameState_MainMenu":
                {
                    this.Activity.Details = "In the Lobby";
                    this.Activity.State = "";
                    this.Activity.Assets.LargeImage = "logo";
                    break;
                }
                case PlayWithFriendsController.destinationTutorial:
                {
                    this.Activity.Details = "In the tutorial";
                    this.Activity.State = "";
                    this.Activity.Assets.LargeImage = "logo";
                    break;
                }
                case "GameState_Tutorial":
                {
                    this.Activity.Details = "In Adventure";
                    this.Activity.State = "Playing Skirmish";
                    this.Activity.Assets.LargeImage = "logo";
                    
                    break;
                }
                case "GameState_Skirmish":
                {
                    this.Activity.Details = "In Adventure";
                    this.Activity.State = "The Black Sarcophagus";
                    this.Activity.Assets.LargeImage = "book1";
                    break;
                }
                case "Adventure_TheBlackSarcophagus":
                {
                    this.Activity.Details = "In Adventure";
                    this.Activity.State = "Realm Of The Rat King";
                    this.Activity.Assets.LargeImage = "book2";
                    break;
                }
                case "Adventure_RealmOfTheRatKing":
                {
                    this.Activity.Details = "In Adventure";
                    this.Activity.State = "Roots of Evil";
                    this.Activity.Assets.LargeImage = "book3";
                    break;
                }
                case "Adventure_Custom": // custom adventures from Custom Adventures mod
                {
                    this.Activity.Details = "In Custom Adventure";
                    this.Activity.State = "<adventure name>"; // @todo: pull custom adventure name
                    this.Activity.Assets.LargeImage = "logo";
                    break;
                }
            }
            
            if (groupID != "")
            {
                this.Activity.Party.Id = groupID;
                this.Activity.Party.Size = new DiscordSdk.PartySize
                {
                    CurrentSize = PhotonNetwork.PlayerList != null ? PhotonNetwork.PlayerList.Length : 1,
                    MaxSize = 4
                };
            }
            else
            {
                this.Activity.Party = new DiscordSdk.ActivityParty();
            }
            
            if (isJoinable && gameID != "")
            {
                this.Activity.Secrets.Join = string.Join(",", gameID, groupID, statusID);
            }
            else
            {
                this.Activity.Secrets.Join = null;
            }
            this.Activity.Timestamps.Start = DateTimeOffset.Now.ToUnixTimeSeconds();
            
            this.Discord.GetActivityManager().UpdateActivity(this.Activity, result =>
            {
                onDone(this, result == DiscordSdk.Result.Ok);
                DiscordSocialProviderMod.Logger.Msg($"(Discord|INFO): Updating status result: {result.ToString()}");
            });
        }

        public void Dispose()
        {
            this.Discord?.Dispose();
        }
    }
}