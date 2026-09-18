using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using FluentAvalonia.Core;

namespace ARCRemake;

public partial class NameListWindow : AppWindow
{
    private NameListConfig config;
    private string? NamelistPath = null;

    public NameListWindow()
    {
        InitializeComponent();

        ModeTitle.Text = "创建名单";

        config = new NameListConfig
        {
            ListName = "",
            Names = new System.Collections.Generic.List<string>()
        };

    }
    public NameListWindow(string NameListPath)
    {
        InitializeComponent();
        
            ModeTitle.Text = "修改名单";
        NamelistPath = NameListPath;
        config = JsonServices.ReadJson<NameListConfig>(NameListPath);
        NameText.Text = config.ListName;
        foreach(var name in config.Names)
        {
            NameContent.Text += $"{name}\n";
        }
        ContinueButton.IsEnabled = false;
    }

    private void Window_Closed(object s, WindowClosingEventArgs e)
    {
        
        
    }

    private void Button_Click(object s, RoutedEventArgs e)
    {
        config.ListName = NameText.Text;
        config.Names.Clear();
        var k = NameContent.Text.Replace("\r","");
        foreach (var i in k.Split("\n"))
        {
            if(!string.IsNullOrEmpty(i.Trim()))
            {
                config.Names.Add(i.Trim());
            }
        }
        if(NamelistPath != null)
        {
            
            JsonServices.WriteJson<NameListConfig>(NamelistPath, config);
        }
        else
        {
            
            if (File.Exists(Path.Combine(RootClasses.ConfigPath(), $"{config.ListName}.json"))) File.Delete(Path.Combine(RootClasses.ConfigPath(), $"{config.ListName}.json"));
            JsonServices.WriteJson<NameListConfig>(Path.Combine(RootClasses.ConfigPath(), $"{config.ListName}.json"), config);
            var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
            a.CurrentNameListPath = Path.Combine(RootClasses.ConfigPath(), $"{config.ListName}.json");
            JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
        }
        this.Close();
    }
    private void Page_Loaded(object s, RoutedEventArgs e)
    {
        config = new NameListConfig
        {
            ListName = "",
            Names = new List<string>()
        };
        foreach (var i in RootPanel.Children)
        {
            i.IsVisible = false;
        }
        StartSPAnimation(RootPanel);
    }

    private void NameText_Changed(object s, RoutedEventArgs e)
    {
        config.ListName = NameText.Text;
        if (!string.IsNullOrEmpty(NameText.Text) && !string.IsNullOrEmpty(NameContent.Text))
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
