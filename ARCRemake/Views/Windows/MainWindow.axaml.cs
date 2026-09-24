using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using System.Threading;
using FluentAvalonia.UI.Windowing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ARCRemake
{
    public partial class MainWindow : AppWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RootClasses.MainWindow = this;
            RootNavi.SelectedItem = NaviItem0;
            this.AddHandler(
            InputElement.KeyDownEvent,
            OnPreviewKeyDown,
            RoutingStrategies.Tunnel
            );
        }
        
        private void OnPreviewKeyDown(object sender,KeyEventArgs e)
        {
             if (e.Key == Key.Space)
             {
                 if(RootClasses.MainWindow.RootFrame.Content is DianMingPage dp)
                 {
                     if(dp.RootButton.IsFocused != true && dp.RootButton.IsEnabled == true)
                     {
                        dp.Button_ChangeStatus(RootClasses.DMPageCTS);
                        e.Handled = true; 
                     }
                 }
            }
        }


        private void Window_Loaded(object s,RoutedEventArgs e)
        {
            if(JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath()).StartUpCheckUpdate)
            {
                 UpdateServices.CheckUpdateAsync(true);
            }
            if (JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath()).UsingHoverBall == true)
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
        
        
         
         private void NavigationView_BackRequested(object sender, NavigationViewBackRequestedEventArgs e)
        {
            RootFrame.Navigate(typeof(SettingsPage));
        }
    }
}
