using ARCRemake.Utils;
using Avalonia.Animation;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

namespace ARCRemake
{
    public partial class MainWindow : AppWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RootNavi.SelectedItem = NaviItem0;
        }

        public void RootNavi_SelectionChanged(object s,NavigationViewSelectionChangedEventArgs e)
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