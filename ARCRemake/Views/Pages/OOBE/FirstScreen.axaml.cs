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

public partial class FirstScreen : UserControl
{
    
    public FirstScreen()
    {
        InitializeComponent();
        foreach(var a in RootPanel.Children)
        {
            if(a is StackPanel sp2)
            {
                foreach(var a2 in sp2.Children)
                {
                    a2.IsVisible = false;
                }
                a.IsVisible = false;
            }
        }
    }

    private async void Page_Loaded(object s,RoutedEventArgs e)
    {
        await StartSPAnimation(RootPanel,500);
    }

    private void Agree_Checked(object s, RoutedEventArgs e)
    {
        ContinueButton.IsEnabled = AgreeBox.IsChecked ?? false;
    }

    private void Continue_Clicked(object s,RoutedEventArgs e)
    {
        RootClasses.OOBEWindow.RootFrame.Navigate(typeof(Personalize));
    }

    public async Task StartSPAnimation(StackPanel sp,int ms)
    {


        foreach (var animation in sp.Children)
        {

            if(animation is StackPanel sp2)
            {
                animation.IsVisible = true;
                StartSPAnimation(sp2,50);
                await Task.Delay(50);
            }
            else
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
                    Duration = TimeSpan.FromMilliseconds(ms),
                    Easing = new ExponentialEaseInOut()
                }
            };
                transform.X = 0;
                await Task.Delay(50);
            }


        }

    }
}