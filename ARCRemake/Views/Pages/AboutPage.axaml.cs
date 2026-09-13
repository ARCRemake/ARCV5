using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using LiveMarkdown.Avalonia;
using System;
using System.Diagnostics;
using System.Net.Http;
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

    private async void EULA_Click(object s, RoutedEventArgs e)
    {
        var k = new ObservableStringBuilder();
        k.Append("正在加载许可协议…");
        var dlg = new ContentDialog
        {
            Title = "许可协议",
            Content = new MarkdownRenderer
            {
                MarkdownBuilder = k
            },
            PrimaryButtonText = "确定",
            DefaultButton = ContentDialogButton.Primary
        };
        var showTask = dlg.ShowAsync();
        _ = Task.Run(async () =>
        {
            try
            {
                using var client = new HttpClient();
                var tex = await client.GetStringAsync(
                    "https://raw.giteeusercontent.com/Wang120229/ARCRemake.UpdateService/raw/master/LICENSE.md");

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    k.Clear();
                    k.Append(tex);
                });
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    k.Clear();
                    k.Append($"加载失败：{ex.Message}");
                });
            }
        });
        await showTask;
    }

    private void BUGReport_Click(object s, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/arcremake/arcv5/issues/new",
            UseShellExecute = true
        });
    }

    private void Link_Click(object s, RoutedEventArgs e)
    {
        
            Process.Start(new ProcessStartInfo
            {
                FileName = ((string)(((HyperlinkButton)s).Tag)),
                UseShellExecute = true
            });
    }
}