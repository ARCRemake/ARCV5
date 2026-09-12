using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;

namespace ARCRemake;

public partial class OOBEWindow : AppWindow
{
    public OOBEWindow()
    {
        InitializeComponent();
        RootClasses.OOBEWindow = this;
        RootFrame.Navigate(typeof(FirstScreen));
    }

    

    
}