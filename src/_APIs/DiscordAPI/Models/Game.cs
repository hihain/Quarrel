﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DiscordAPI.Models
{
    public class GameBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
    }

    public class Game : GameBase
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("timestamps")]
        public timestamps TimeStamps{get;set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("party")]
        public party Party { get; set; }

        [JsonProperty("assets")]
        public assets Assets { get; set; }

        [JsonProperty("secrets")]
        public secrets Secrets { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("instance")]
        public bool Instance { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        public bool IsXboxGame { get => ApplicationId != null && ApplicationId == "438122941302046720"; }

        public string SmallImageUrl => Assets != null ? GetImageUrl(Assets.SmallImage, ApplicationId) : "";
        public Uri SmallImageUri => Assets != null ? new Uri(GetImageUrl(Assets.SmallImage, ApplicationId)) : null;

        public string LargeImageUrl => Assets != null ? GetImageUrl(Assets.LargeImage, ApplicationId) : "";
        public Uri LargeImageUri => Assets != null ? new Uri(GetImageUrl(Assets.LargeImage, ApplicationId)) : null;

        public string GetImageUrl(string id, string gameid, bool game = false, string append = "?size=512")
        {
            // Set type in query 
            string type = "app";
            if (game) type = "game";

            // Query URL
            return "https://cdn.discordapp.com/" + type + "-assets/" + gameid + "/" + id + ".png" + append;
        }
    }
    public class timestamps
    {
        [JsonProperty("start")]
        public long? Start;
        [JsonProperty("end")]
        public long? End;
    }
    public class party
    {
        [JsonProperty("size")]
        public int?[] Size { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    public class assets
    {
        [JsonProperty("small_image")]
        public string SmallImage { get; set; }
        [JsonProperty("large_image")]
        public string LargeImage { get; set; }
        [JsonProperty("small_text")]
        public string SmallText { get; set; }
        [JsonProperty("large_text")]
        public string LargeText { get; set; }
    }
    public class secrets
    {
        [JsonProperty("join")]
        public string Join { get; set; }
        [JsonProperty("spectate")]
        public string Spectate { get; set; }
        [JsonProperty("match")]
        public string Match { get; set; }
    }
}