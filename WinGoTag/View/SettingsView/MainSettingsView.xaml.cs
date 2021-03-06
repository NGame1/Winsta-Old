﻿using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinGoTag.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SettingsView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainSettingsView : Page
    {
        public MainSettingsView() => InitializeComponent();

        void LVI_Tapped(object sender, TappedRoutedEventArgs e)
        {
            switch ((sender as ListViewItem).Content.ToString())
            {
                case "Story Settings":
                    Frame.Navigate(typeof(StorySettingsView));
                    break;
                default:
                    break;
            }
        }

        async void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var content1 = $"Do you want to save your credential and session before you logging out?";
                var md1 = new MessageDialog(content1);
                md1.Commands.Add(new UICommand("Yes"));
                md1.Commands.Add(new UICommand("No"));
                md1.Commands.Add(new UICommand("Cancel"));
                md1.CancelCommandIndex = 2;
                md1.DefaultCommandIndex = 2;
                var label1 = await md1.ShowAsync();
                if (label1.Label == "Yes" || label1.Label == "No")
                {
                    if (label1.Label == "Yes")
                        await SessionHelper.BackupCurrentSession();

                    var content2 = $"Are you sure you want to log out?";
                    var md2 = new MessageDialog(content2);
                    md2.Commands.Add(new UICommand("Yes"));
                    md2.Commands.Add(new UICommand("No"));
                    md2.CancelCommandIndex = 1;
                    md2.DefaultCommandIndex = 1;
                    var label2 = await md2.ShowAsync();
                    if (label2.Label == "Yes")
                    {
                        await SessionHelper.DeleteCurrentSession();
                        if (label1.Label == "No")
                            await SessionHelper.DeleteBackupSession(AppCore.InstaApi.GetLoggedUser().UserName);

                        AppCore.InstaApi = null;
                        var frame = new Frame();
                        Window.Current.Content = frame;
                        frame.Navigate(typeof(MainPage));
                        Window.Current.Activate();
                    }
                }
                // else do nothing
            }
            catch (Exception ex)
            {
                ex.ExceptionMessage("MainSettingsView.LogoutButtonClick");
            }
        }
    }
}