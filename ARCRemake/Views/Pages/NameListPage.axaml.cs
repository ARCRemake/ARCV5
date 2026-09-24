using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARCRemake;

public partial class NameListPage : UserControl
{
    private NameListConfig config;
    public NameListPage()
    {
        InitializeComponent();
		config = new NameListConfig
            {
                ListName = "",
                Names = new System.Collections.Generic.List<string>()
            };
		NameText.Text = "";
        NameContent.Text = "";
    }

    private void Window_Closed(object s, RoutedEventArgs e)
    {
        RootClasses.MainWindow.RootNavi.IsBackEnabled=false;
        NameText.Text = "";
        NameContent.Text = "";
    }

    private void Button_Click(object s, RoutedEventArgs e)
    {
        config.ListName = NameText.Text;
        config.Names.Clear();
        var k = NameContent.Text.Replace("\r", "");
        foreach (var i in k.Split("\n"))
        {
            if (!string.IsNullOrEmpty(i.Trim()))
            {
                config.Names.Add(i.Trim());
            }
        }
        if (RootClasses.EditListPath != null)
        {

            JsonServices.WriteJson<NameListConfig>(RootClasses.EditListPath, config);
        }
        else
        {

            if (File.Exists(Path.Combine(RootClasses.NameListFolder(), $"{config.ListName}.json"))) File.Delete(Path.Combine(RootClasses.NameListFolder(), $"{config.ListName}.json"));
            JsonServices.WriteJson<NameListConfig>(Path.Combine(RootClasses.NameListFolder(), $"{config.ListName}.json"), config);
            var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
            a.CurrentNameListPath = Path.Combine(RootClasses.NameListFolder(), $"{config.ListName}.json");
            JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
        }
        RootClasses.MainWindow.RootFrame.Navigate(typeof(SettingsPage));
    }
    private void Page_Loaded(object s, RoutedEventArgs e)
    {
        if (RootClasses.EditListPath == null)
        {
            ModeTitle.Text = "创建名单";

            config = new NameListConfig
            {
                ListName = "",
                Names = new System.Collections.Generic.List<string>()
            };
            ContinueButton.IsEnabled = false;
        }
        else
        {
            ModeTitle.Text = "修改名单";
            config = JsonServices.ReadJson<NameListConfig>(RootClasses.EditListPath);
            NameText.Text = config.ListName;
            foreach (var name in config.Names)
            {
                NameContent.Text += $"{name}\n";
            }
            ContinueButton.IsEnabled = false;
        }
        RootClasses.MainWindow.RootNavi.IsBackEnabled=true;
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