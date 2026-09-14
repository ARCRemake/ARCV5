using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Navigation;
using System.Threading;

namespace ARCRemake;

public partial class DianMingPage : UserControl
{
    private bool DianmingStatus = false;
    private DispatcherTimer DMTimer;
    public DianMingPage()
    {
        InitializeComponent();

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
        var timer = new DispatcherTimer();
        DMTimer = timer;
    }
    private async void Page_Unloaded(object s, RoutedEventArgs e)
    {
        DianmingStatus = false;
        
    }

    private void RootButton_Click(object s, RoutedEventArgs e)
    {

    }

    private void Button_ChangeStatus()
    {

    }
}