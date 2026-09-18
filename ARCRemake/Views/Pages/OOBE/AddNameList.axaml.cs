using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ARCRemake;

public partial class AddNameList : UserControl
{
    private NameListConfig config;
    public AddNameList()
    {
        config = new NameListConfig
        {
            ListName = "",
            Names = new System.Collections.Generic.List<string>()
        };
        InitializeComponent();
        
    }

    private void Button_Click(object s,RoutedEventArgs e)
    {
        config.Names.Clear();
        var b = NameContent.Text.Replace("\r","");
        foreach(var i in b.Split("\n"))
        {
            if(!string.IsNullOrEmpty(i.Trim()))
            {
                config.Names.Add(i.Trim());
            }
        }
        
        if (File.Exists(Path.Combine(RootClasses.NameListFolder(),$"{config.ListName}.json"))) File.Delete(Path.Combine(RootClasses.NameListFolder(),$"{config.ListName}.json"));
        JsonServices.WriteJson<NameListConfig>(Path.Combine(RootClasses.NameListFolder(),$"{config.ListName}.json"),config);
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.OOBEStatus = true;
        a.CurrentNameListPath = Path.Combine(RootClasses.NameListFolder(),$"{config.ListName}.json");
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(),a);
        RootClasses.OOBEWindow.RootFrame.Navigate(typeof(LastScreen));
    }
    private void Page_Loaded(object s, RoutedEventArgs e)
    {
        config = new NameListConfig
        {
            ListName = "",
            Names = new List<string>()
        };
        foreach(var i in RootPanel.Children)
        {
            i.IsVisible = false;
        }
        StartSPAnimation(RootPanel);
    }

    private void NameText_Changed(object s, RoutedEventArgs e)
    {
        config.ListName = NameText.Text;
        if(!string.IsNullOrEmpty( NameText.Text ) && !string.IsNullOrEmpty( NameContent.Text ))
        {
            ContinueButton.IsEnabled = true;
        }
    }

    private void NameContent_Changed(object s, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(NameText.Text) && !string.IsNullOrEmpty(NameContent.Text))
        {
            ContinueButton.IsEnabled = true;
        }
    }

    public async Task StartSPAnimation(DockPanel sp)
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