﻿using System;
using Windows.Storage;
using Windows.Security.Credentials;

using Discord_UWP.LocalModels;
using Discord_UWP.SharedModels;

namespace Discord_UWP
{
    public static class Storage
    {
        public static Settings Settings = new Settings(); //this just represents the storage
        public static ApplicationDataContainer SavedSettings = ApplicationData.Current.LocalSettings;
        public static LocalState State = new LocalState();
        public static PasswordVault PasswordVault = new PasswordVault();
    }

    public enum Theme { Dark, Light, Windows, Discord }
    public class Settings
    {
        Tuple<string, User> ActiveUser; public bool LockChannels = false;
        public bool AutoHideChannels = true;
        public bool AutoHidePeople = false;
        public bool HighlightEveryone = true;
        public bool FriendsNotifyDMs = true;
        public bool FriendsNotifyFriendRequest = false;
        //public bool FriendsNotifyIncoming = true;
        //public bool FriendsNotifyOutgoing = false;
        public bool Toasts = false;
        public double RespUiM = 569;
        public double RespUiL = 768;
        public double RespUiXl = 1024;
        public bool AppBarAtBottom = false;
        public bool ExpensiveRender = false;
        public bool ShowOfflineMembers = false;
        public bool DevMode = false;
        public bool DiscordLightTheme = false;
        public bool CompactMode = false;
        public Theme Theme = Theme.Dark;
        public bool AccentBrush = false; //If false use Blurple if true use System.AccentColor
        public bool Vibrate = false;
    }
}
