using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using System;
using System.Linq;

namespace ARCRemake
{
    public partial class MainWindow : AppWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RootClasses.MainWindow = this;
            RootNavi.SelectedItem = NaviItem0;
        }

        private void Window_Loaded(object s,RoutedEventArgs e)
        {
            if(JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json").StartUpCheckUpdate)
            {
                UpdateServices.CheckUpdateAsync(true);
            }
            if (JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json").UsingHoverBall == true)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var existing = desktop.Windows
                        .OfType<HoverWindow>()
                        .FirstOrDefault(w => w.IsVisible);

                    if (existing == null)
                    {
                        RootClasses.HoverWindow = new HoverWindow();
                        RootClasses.HoverWindow.Show();
                    }
                }


            }
        }

        private void Window_Closing(object s,WindowClosingEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var existing = desktop.Windows
                    .OfType<HoverWindow>()
                    .FirstOrDefault(w => w.IsVisible);

                if (existing != null)
                {

                    RootClasses.HoverWindow.Close();
                }
            }
        }

        private void RootNavi_SelectionChanged(object s,NavigationViewSelectionChangedEventArgs e)
        {
            if(RootNavi.SelectedItem == NaviItem0)
            {
                RootFrame.Content = null;
                RootClasses.DianMingMode = "常规点名";
                RootFrame.Navigate(typeof(DianMingPage));
                
            }
            else if (RootNavi.SelectedItem == NaviItem1)
            {
                RootFrame.Content = null;
                RootClasses.DianMingMode = "立即点名";
                RootFrame.Navigate(typeof(DianMingPage));
                
            }
            else if (RootNavi.SelectedItem == NaviItem2)
            {
                RootFrame.Content = null;
                RootClasses.DianMingMode = "延时点名";
                RootFrame.Navigate(typeof(DianMingPage));
                
            }
            else if (RootNavi.SelectedItem == NaviItem3)
            {
                RootFrame.Content = null;
                RootClasses.DianMingMode = "批量点名";
                RootFrame.Navigate(typeof(DianMingPage));
                
            }
            else if (RootNavi.SelectedItem == NaviItem4)
            {
                RootFrame.Navigate(typeof(SettingsPage));
            }
            else if (RootNavi.SelectedItem == NaviItem5)
            {
                RootFrame.Navigate(typeof(AboutPage));
            }
            

        }
    }
}