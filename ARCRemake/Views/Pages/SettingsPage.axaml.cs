using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ARCRemake;

public partial class SettingsPage : UserControl
{
    
    public SettingsPage()
    {
        InitializeComponent();
        var a2 = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        NameListBox.Text = $"{JsonServices.ReadJson<AppConfig>(a2.CurrentNameListPath)}（{a2.CurrentNameListPath}）";
        CGDM.Value = a2.IntervalTick;
        DSDM.Value = a2.ScheduledSeconds;
        PLDM.Value = a2.BatchCounts;
        foreach (var animation in RootPanel.Children)
        {

            animation.IsVisible = false;
        }

    }

    private async void Page_Loaded(object s,RoutedEventArgs e)
    {
        await StartSPAnimation(RootPanel);
        foreach (var a in Directory.EnumerateFiles($"{Environment.CurrentDirectory}\\NameLists", "*.json"))
        {
            try
            {
                var b = JsonServices.ReadJson<NameListConfig>(a);
                var ci = new ComboBoxItem
                {
                    Content = $"{b.ListName}（{a}）",
                    Tag = $"{a}"
                };
                
                NameListBox.Items.Add(ci);
            }
            catch
            {
                continue;
            }

        }
        var a2 = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        var a3 = new ComboBoxItem { };
        foreach(var a4 in NameListBox.Items)
        {
            if(a4 is ComboBoxItem a5)
            {
                if(Path.GetFileName((string)a5.Tag) == Path.GetFileName(a2.CurrentNameListPath))
                {
                    NameListBox.SelectedItem = a5;
                    break;
                }
            }
        }

        
 
        CGDM.Value = a2.IntervalTick;
        DSDM.Value = a2.ScheduledSeconds;
        PLDM.Value = a2.BatchCounts;
    }

    private void NameListBox_SelectionChanged(object s,RoutedEventArgs e)
    {
        
        var a = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        a.CurrentNameListPath = (string)((ComboBoxItem)NameListBox.SelectedItem).Tag;
        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json",a);
    }

    private void Page_UnLoaded(object s, RoutedEventArgs e)
    {
        foreach (var animation in RootPanel.Children)
        {

            animation.IsVisible = false;
        }
        var a2 = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        NameListBox.Items.Clear();
        
        CGDM.Value = a2.IntervalTick;
        DSDM.Value = a2.ScheduledSeconds;
        PLDM.Value = a2.BatchCounts;
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

    private void Recovery_Click(object s,RoutedEventArgs e)
    {
        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json", ConfigHelper.InitConfig());
        Directory.Delete($"{Environment.CurrentDirectory}/NameLists", true);
        var exePath = Environment.ProcessPath
            ?? throw new InvalidOperationException("无法获取当前可执行文件路径。");

        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            UseShellExecute = false,
            WorkingDirectory = AppContext.BaseDirectory,
        };



        Process.Start(psi);

        if (Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
        else
        {
            Environment.Exit(0);
        }
    }

    private void CGDM_TextChanged(object s,RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        a.IntervalTick = (int)CGDM.Value;
        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json",a);
    }

    private void DSDM_TextChanged(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        a.ScheduledSeconds = (int)DSDM.Value;
        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json", a);
    }

    private void PLDM_TextChanged(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        a.BatchCounts = (int)PLDM.Value;
        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json", a);
    }


}