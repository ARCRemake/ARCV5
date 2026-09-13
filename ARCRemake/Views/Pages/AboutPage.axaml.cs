using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Threading.Tasks;

namespace ARCRemake;

public partial class AboutPage : UserControl
{
    public AboutPage()
    {
        InitializeComponent();
        foreach (var animation in RootPanel.Children)
        {

            animation.IsVisible = false;
        }

    }

    private async void Page_Loaded(object s, RoutedEventArgs e)
    {
        await StartSPAnimation(RootPanel);
    }

    private void Page_UnLoaded(object s, RoutedEventArgs e)
    {
        foreach (var animation in RootPanel.Children)
        {

            animation.IsVisible = false;
        }
    }



    public async Task StartSPAnimation(StackPanel sp)
    {


        foreach (var animation in sp.Children)
        {

            animation.IsVisible = false;
            var transform = new TranslateTransform();
            animation.RenderTransform = transform;
            transform.X = -1500;
            animation.IsVisible = true;
            transform.Transitions = new Transitions
            {
                new DoubleTransition
                {
                    Property = TranslateTransform.XProperty,
                    Duration = TimeSpan.FromMilliseconds(500),
                    Easing = new ExponentialEaseInOut()
                }
            };
            transform.X = 0;
            await Task.Delay(50);


        }

    }

    private async void CheckUpdate_Click(object s, RoutedEventArgs e)
    {
        CheckUpdate.IsEnabled = false;
        await UpdateServices.CheckUpdateAsync(false);
        CheckUpdate.IsEnabled = true;
    }
}