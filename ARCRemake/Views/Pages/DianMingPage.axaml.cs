using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ARCRemake;

public partial class DianMingPage : UserControl
{
    private bool DianmingStatus = false;

    private List<string> Fullnamelist;
    private List<string> Pickernamelist;
    private DispatcherTimer DMTimer;
    private Random randomname = new Random();
    public DianMingPage()
    {
        InitializeComponent();
        var a2 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(a2.IntervalTick);
        timer.Tick += (s, e) =>
        {
            NameBlock.Text = Fullnamelist[randomname.Next(0, Fullnamelist.Count - 1)];
        };
        DMTimer = timer;
        
    }

    private async void Page_Loaded(object s, RoutedEventArgs e)
    {
        ModeTitle.Text = RootClasses.DianMingMode;
        if(RootClasses.DianMingMode != "批量点名")
        {
            ListDianMingBox.IsVisible = false;
            NameBlock.IsVisible = true;
        }
        else
        {
            ListDianMingBox.IsVisible = true;
            NameBlock.IsVisible = false;
        }
        var a2 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        if(!File.Exists(a2.CurrentNameListPath))
        {
            RootButton.IsEnabled = false;
            var dlg = new ContentDialog
            {
                Title = "错误",
                Content = $"配置项不存在，请到设置页重新选择",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
        }
        try
        {
            JsonServices.ReadJson<NameListConfig>(a2.CurrentNameListPath);
        }
        catch
        {
            RootButton.IsEnabled = false;
            var dlg = new ContentDialog
            {
                Title = "错误",
                Content = $"配置项不合法，请到设置页重新选择",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
        }
        Fullnamelist.AddRange(JsonServices.ReadJson<NameListConfig>(a2.CurrentNameListPath).Names);
        Pickernamelist.AddRange(JsonServices.ReadJson<NameListConfig>(a2.CurrentNameListPath).Names);


    }
    private async void Page_Unloaded(object s, RoutedEventArgs e)
    {
        DianmingStatus = false;
        DMTimer.Stop();
        NameBlock.Text = "请开始点名";
        Fullnamelist.Clear();
        Pickernamelist.Clear();
    }

    private void RootButton_Click(object s, RoutedEventArgs e)
    {
        Button_ChangeStatus();
    }

    private void Button_ChangeStatus()
    {
        switch(RootClasses.DianMingMode)
        {
            case "常规点名":
                if (DianmingStatus)
                {
                    DMTimer.Stop();
                    DianmingStatus = false;
                    if(Pickernamelist.Count > 0){
							NameBlock.Text = Pickernamelist[randomname.Next(0, Pickernamelist.Count - 1)];
                    		Pickernamelist.RemoveAll(x => x == NameBlock.Text);
                	}
					else
					{
                        Pickernamelist.Clear();
                        Pickernamelist.AddRange(Fullnamelist);
                        NameBlock.Text = Pickernamelist[randomname.Next(0, Pickernamelist.Count - 1)];
                    		Pickernamelist.RemoveAll(x => x == NameBlock.Text);
					}
				}
                else
                {
                    DianmingStatus = true;
                    DMTimer.Start();
                }
                break;
            case "立即点名":
				    if(Pickernamelist.Count > 0){
							NameBlock.Text = Pickernamelist[randomname.Next(0, Pickernamelist.Count - 1)];
                    		Pickernamelist.RemoveAll(x => x == NameBlock.Text);
                	}
					else
					{
                        Pickernamelist.Clear();
                        Pickernamelist.AddRange(Fullnamelist);
                        NameBlock.Text = Pickernamelist[randomname.Next(0, Pickernamelist.Count - 1)];
                    	Pickernamelist.RemoveAll(x => x == NameBlock.Text);
					}
                break;
            case "批量点名":
                var a2 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
                for (int i = 0; i < a2.BatchCounts; i++)
                {
                    if (Pickernamelist.Count > 0)
                    {
                        var a = Pickernamelist[randomname.Next(0, Pickernamelist.Count - 1)];
                        ListDianMingBox.Items.Add(a);
                        Pickernamelist.RemoveAll(x => x == a);
                    }
                    else
                    {
                        Pickernamelist.Clear();
                        Pickernamelist.AddRange(Fullnamelist);
                        var a = Pickernamelist[randomname.Next(0, Pickernamelist.Count - 1)];
                        ListDianMingBox.Items.Add(a);
                        Pickernamelist.RemoveAll(x => x == a);
                    }
                }
                break;
            case "定时点名":
                break;
        }


    }
}