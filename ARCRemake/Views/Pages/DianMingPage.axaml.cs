using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ARCRemake;

public partial class DianMingPage : UserControl
{
    private bool DianmingStatus = false;

    private List<string> Fullnamelist = new List<string>();
    private List<string> Pickernamelist = new List<string>();
    private DispatcherTimer DMTimer;
    private Random randomname = new Random();
    
    public DianMingPage()
    {
        InitializeComponent();
        
        

    }

    private async void Page_Loaded(object s, RoutedEventArgs e)
    {
        
       
        var timer = new DispatcherTimer();
        DMTimer = timer;
        RootClasses.DMPageCTS = new CancellationTokenSource();
        ButtonIcon.Icon = FluentIcons.Common.Icon.Play;
        ButtonText.Text = "开始点名";
        ModeTitle.Text = RootClasses.DianMingMode;
        if (RootClasses.DianMingMode != "批量点名")
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
        if (a2.CurrentNameListPath == "")
        {
            RootButton.IsEnabled = false;
            var dlg = new ContentDialog
            {
                Title = "错误",
                Content = $"名单不存在，请到设置页重新选择",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
            return;
        }
        if (!File.Exists(a2.CurrentNameListPath))
        {
            RootButton.IsEnabled = false;
            var dlg = new ContentDialog
            {
                Title = "错误",
                Content = $"名单不存在，请到设置页重新选择",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
            return;
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
                Content = $"名单不合法，请到设置页重新选择",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
            return;
        }
		var a23 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
		Fullnamelist = JsonServices.ReadJson<NameListConfig>(a23.CurrentNameListPath).Names;
        Pickernamelist = JsonServices.ReadJson<NameListConfig>(a23.CurrentNameListPath).Names;
		if(a23.DianMingFont != null)
        {
            NameBlock.FontFamily = new Avalonia.Media.FontFamily(a23.DianMingFont);
        }
        else
        {
            NameBlock.ClearValue(TextBlock.FontFamilyProperty);
        }
        if(a23.DianMingFontColor is { } color)
        {
            NameBlock.Foreground = new SolidColorBrush(color);
        }
        else
        {
            NameBlock.ClearValue(TextBlock.ForegroundProperty);
        }
        timer.Interval = TimeSpan.FromMilliseconds(a23.IntervalTick);
        timer.Tick += (s, e) =>
        {
            NameBlock.Text = Fullnamelist[randomname.Next(0, Fullnamelist.Count)];
        };
        DMTimer = timer;
        
		RootButton.IsEnabled = true;
	
        
        


    }
    private async void Page_Unloaded(object s, RoutedEventArgs e)
    {
		DianmingStatus = false;
        DMTimer.Stop();
        RootClasses.DMPageCTS.Cancel();
        NameBlock.Text = "请开始点名";
        Fullnamelist.Clear();
        Pickernamelist.Clear();
        ButtonIcon.Icon = FluentIcons.Common.Icon.Play;
        ButtonText.Text = "开始点名";
    }

    private async void RootButton_Click(object s, RoutedEventArgs e)
    {
        await Button_ChangeStatus(RootClasses.DMPageCTS);
    }
    
    

    public async Task Button_ChangeStatus(CancellationTokenSource cts)
    {
		if(RootButton.IsEnabled == false)
		{
			return;
		}
        switch (RootClasses.DianMingMode)
        {
            case "常规点名":
                if (DianmingStatus)
                {
                    ButtonIcon.Icon = FluentIcons.Common.Icon.Play;
                    ButtonText.Text = "开始点名";
                    DMTimer.Stop();
                    DianmingStatus = false;
                    if (Pickernamelist.Count > 0)
                    {
                        NameBlock.Text = Pickernamelist[randomname.Next(0, Pickernamelist.Count)];
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
                    ButtonIcon.Icon = FluentIcons.Common.Icon.Pause;
                    ButtonText.Text = "停止点名";
                    DianmingStatus = true;
                    DMTimer.Start();
                }
                break;
            case "立即点名":
                if (Pickernamelist.Count > 0)
                {
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
                if (ListDianMingBox.Items.Count != 0) ListDianMingBox.Items.Clear();
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
            case "延时点名":
                var a23 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
                ButtonIcon.Icon = FluentIcons.Common.Icon.Pause;
                ButtonText.Text = "停止点名";
                DianmingStatus = true;
                DMTimer.Start();
                RootButton.IsEnabled = false;
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(a23.ScheduledSeconds), cts.Token);
                }
                catch (Exception ex)
                {
					return;
                }
                RootButton.IsEnabled = true;
                ButtonIcon.Icon = FluentIcons.Common.Icon.Play;
                ButtonText.Text = "开始点名";
                DMTimer.Stop();
                DianmingStatus = false;
                if (Pickernamelist.Count > 0)
                {
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
        }


    }
}
