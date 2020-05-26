﻿// Copyright (c) Quarrel. All rights reserved.

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Quarrel.Controls;
using Quarrel.Controls.Markdown;
using Quarrel.Controls.Members;
using Quarrel.ViewModels.Messages.Navigation;
using Quarrel.ViewModels.Models.Bindables.Messages.Embeds;
using Quarrel.ViewModels.Models.Bindables.Users;
using Quarrel.ViewModels.Services.Analytics;
using Quarrel.ViewModels.Services.Discord.Guilds;
using Quarrel.ViewModels.Services.Navigation;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Quarrel.DataTemplates.Messages
{
    /// <summary>
    /// A collection of Data Templates for Message displaying.
    /// </summary>
    public partial class MessageTemplate
    {
        private IAnalyticsService _analyticsService = null;
        private ISubFrameNavigationService _subFrameNavigationService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTemplate"/> class.
        /// </summary>
        public MessageTemplate()
        {
            this.InitializeComponent();
        }

        private IAnalyticsService AnalyticsService => _analyticsService ?? (_analyticsService = SimpleIoc.Default.GetInstance<IAnalyticsService>());

        private ISubFrameNavigationService SubFrameNavigationService => _subFrameNavigationService ?? (_subFrameNavigationService = SimpleIoc.Default.GetInstance<ISubFrameNavigationService>());

        private void Expand(object sender, TappedRoutedEventArgs e)
        {
            var image = (e.OriginalSource as FrameworkElement).DataContext;
            if (image is BindableAttachment bAttachment)
            {
                image = bAttachment.Model;
            }
            else if (image is BindableEmbed bEmbed)
            {
                image = bEmbed.Model;
            }

            SubFrameNavigationService.NavigateTo("AttachmentPage", image);
        }

        private async void Markdown_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.User != null)
            {
                var guildsService = SimpleIoc.Default.GetInstance<IGuildsService>();
                BindableGuildMember member = guildsService.GetGuildMember(e.User.Id, guildsService.CurrentGuild.Model.Id);
                if (member != null)
                {
                    Flyout flyout = new Flyout()
                    {
                        Content = new MemberFlyoutTemplate() { DataContext = member },
                        FlyoutPresenterStyle = App.Current.Resources["GenericFlyoutStyle"] as Style,
                    };
                    flyout.ShowAt(sender as FrameworkElement);
                }
            }
            else if (e.Channel != null)
            {
                Messenger.Default.Send(new ChannelNavigateMessage(e.Channel));
            }
            else
            {
                Uri uri;
                if (Uri.TryCreate(e.Link, UriKind.Absolute, out uri))
                {
                    await Launcher.LaunchUriAsync(uri);
                }
            }
        }

        private void LoadEmojis(object sender, RoutedEventArgs e)
        {
            var picker = ((sender as Button).Flyout as Flyout).Content.FindChild<EmojiPicker>();
            if (!picker.IsDataLoaded)
            {
                picker.Load();
            }
        }
    }
}
