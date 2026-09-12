using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace ARCRemake;

public partial class LastScreen : UserControl
{
    public LastScreen()
    {
        InitializeComponent();
    }
    private void Button_Click(object s, RoutedEventArgs e)
    {
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
}