using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ARCRemake;

public partial class AddNameList : UserControl
{
    public AddNameList()
    {
        InitializeComponent();
    }

    private void Button_Click(object s,RoutedEventArgs e)
    {
        RootClasses.OOBEWindow.RootFrame.Navigate(typeof(LastScreen));
    }
}