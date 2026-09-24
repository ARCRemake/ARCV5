using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace ARCRemake;

public partial class HoverWindow : Window
{
    private bool _isPressed;
    private bool _isDragging;
    private PixelPoint _startScreenPoint;     
    private PixelPoint _windowStartPosition;   
    private const double DragThresholdPixels = 6;
    private DispatcherTimer toptimer = new DispatcherTimer();

    public HoverWindow()
    {
        InitializeComponent();
        toptimer.Interval = TimeSpan.FromSeconds(1);
        toptimer.Tick += (s, e) =>
        {
            this.Topmost = true;
        };
        toptimer.Start();

    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var point = e.GetCurrentPoint(this);
        if (!point.Properties.IsLeftButtonPressed) return;

        _isPressed = true;
        _isDragging = false;

        var posInWindow = e.GetPosition(this);
        _startScreenPoint = this.PointToScreen(posInWindow);
        _windowStartPosition = this.Position;

        e.Pointer.Capture(this);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_isPressed) return;

        var point = e.GetCurrentPoint(this);
        if (!point.Properties.IsLeftButtonPressed) return;

        var currentPosInWindow = e.GetPosition(this);
        var currentScreenPoint = this.PointToScreen(currentPosInWindow);

        double dx = currentScreenPoint.X - _startScreenPoint.X;
        double dy = currentScreenPoint.Y - _startScreenPoint.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        if (!_isDragging && distance >= DragThresholdPixels)
        {
            _isDragging = true;
        }

        if (_isDragging)
        {
            this.Position = new PixelPoint(
                _windowStartPosition.X + (int)dx,
                _windowStartPosition.Y + (int)dy
            );
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (!_isPressed) return;

        e.Pointer.Capture(null); 

        if (!_isDragging)
        {
            OnWindowClick();
        }

        _isPressed = false;
        _isDragging = false;
    }

    private void OnWindowClick()
    {
        RootClasses.MainWindow.WindowState = WindowState.Normal;
        RootClasses.MainWindow.Activate();
        RootClasses.MainWindow.Focus();
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
        toptimer.Stop();
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.HoverWindowX = this.Position.X;
        a.HoverWindowY = this.Position.Y;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
    }

    private void Window_Opened(object s,EventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        this.Position = new PixelPoint(a.HoverWindowX, a.HoverWindowY);
    }
}