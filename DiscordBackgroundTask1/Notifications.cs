﻿using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace DiscordBackgroundTask1
{
    public class SendToast
    {
        public static void Default(string message)
        {
            //build toast
            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName("text");
            var text = xml.CreateTextNode(message);
            elements[0].AppendChild(text);
            var toast = new ToastNotification(xml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        public static void UnreadDM(ReadyStructure.PrivateChannel channel, int count, string lastmessage)
        {
            string imageurl = "";
            string text = "";
            if(channel.type == 1)
            {
                text = channel.recipients.FirstOrDefault().username;
                imageurl = "https://cdn.discordapp.com/avatars/" + channel.recipients.FirstOrDefault().id + "/" + channel.recipients.FirstOrDefault().avatar + ".png";
            }     
            else if(channel.type == 3)
            {
                if (string.IsNullOrEmpty(channel.name))
                    text = String.Join(",", channel.recipients.Select(x => x.username));
                else text = channel.name;
                if (channel.icon == null)
                    imageurl = "https://discordapp.com/assets/f046e2247d730629309457e902d5c5b3.svg";
                else
                    imageurl = "https://cdn.discordapp.com/channel-icons/" + channel.id + "/" + channel.icon + ".png";
            }

            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                        new AdaptiveText()
                        {
                            Text = text
                        },
                        new AdaptiveText()
                        {
                            Text = count.ToString() + " missed messages"
                        }
                    },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = imageurl,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                },
                DisplayTimestamp = SnowflakeToTime(lastmessage)
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public static void FriendRequest(ReadyStructure.User2 user)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children = {
                        new AdaptiveText() {
                            Text = user.username + " sent you a friend request"
                        }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = "https://cdn.discordapp.com/avatars/" + user.id + "/" + user.avatar + ".png",
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons = {
                         new ToastButton("Accept", "relationship/accept/"+user.id) {
                            ActivationType = ToastActivationType.Background
                         },
                         new ToastButton("Decline", "relationship/decline/="+user.id) {
                            ActivationType = ToastActivationType.Background
                         }
                    }
                }
                
            };
            var toastNotif = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
        public static DateTimeOffset SnowflakeToTime(string id)
        {
            //returns unix time in ms
            if (String.IsNullOrEmpty(id)) return new DateTimeOffset();
            return DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64((double)((Convert.ToInt64(id) / (4194304)) + 1420070400000) / 10));
        }
    }
}