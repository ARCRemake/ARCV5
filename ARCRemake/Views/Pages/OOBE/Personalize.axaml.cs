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

public partial class Personalize : UserControl
{
    public Personalize()
    {
        InitializeComponent();
        
    }

    private async void Page_Loaded(object s, RoutedEventArgs e)
    {
        StartSPAnimation(RootPanel);
    }

    private void Button_Click(object s, RoutedEventArgs e)
    {
        RootClasses.OOBEWindow.RootFrame.Navigate(typeof(AddNameList));
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
}