using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

namespace ARCRemake;

public partial class HoverWindow : Window
{
    public static IBrush IconForeground = new SolidColorBrush(Color.Parse("#C026D3"));
    public static IBrush IconBackground = new SolidColorBrush(Color.Parse("#C026D3"), 0.5);
    public HoverWindow()
    {
        InitializeComponent();
    }

    private void OnWindowPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        RootClasses.MainWindow.WindowState = WindowState.Normal;
        RootClasses.MainWindow.Activate();
        RootClasses.MainWindow.Focus();
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void OnPointerEntered(object s, PointerEventArgs e)
    {
        path0.Fill = new SolidColorBrush(Color.Parse("#C026D3"));
        path1.Fill = new SolidColorBrush(Color.Parse("#C026D3"));
    }

    private void OnPointerExited(object s, PointerEventArgs e)
    {
        path0.Fill = new SolidColorBrush(Color.Parse("#B9B9B9"));
        path1.Fill = new SolidColorBrush(Color.Parse("#404040"));
    }

    private void Window_Closing(object s,WindowClosingEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        a.HoverWindowX = this.Position.X;
        a.HoverWindowY = this.Position.Y;
        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json", a);
    }

    private void Window_Opened(object s,EventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json");
        this.Position = new PixelPoint(a.HoverWindowX, a.HoverWindowY);
    }
}