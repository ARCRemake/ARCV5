using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Navigation;

namespace ARCRemake;

public partial class DianMingPage : UserControl
{
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
    }

    private void RootButton_Click(object s, RoutedEventArgs e)
    {

    }
}