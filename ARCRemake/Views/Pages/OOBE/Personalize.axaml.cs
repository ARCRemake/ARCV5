using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ARCRemake;

public partial class Personalize : UserControl
{
    public Personalize()
    {
        InitializeComponent();
        
    }

    private async void Page_Loaded(object s, RoutedEventArgs e)
    {
        RootClasses.OOBEWindow.RootFrame.Navigate(typeof(AddNameList));
    }
}