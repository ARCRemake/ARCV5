using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

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

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        path0.Fill = new SolidColorBrush(Color.Parse("#C026D3"));
        path1.Fill = new SolidColorBrush(Color.Parse("#C026D3"));
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        path0.Fill = new SolidColorBrush(Color.Parse("#B9B9B9"));
        path1.Fill = new SolidColorBrush(Color.Parse("#404040"));
    }
}