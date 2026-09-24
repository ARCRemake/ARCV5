using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Visuals;
using AvaloniaSelFont.FontDialog.Views;
using FluentAvalonia.UI.Controls;

namespace ARCRemake;

public partial class PersonalizeCard : UserControl
{
    public PersonalizeCard()
    {
        InitializeComponent();
    }

    private void Control_Loaded(object s,RoutedEventArgs e)
    {
        var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        if(Application.Current.ActualThemeVariant == ThemeVariant.Dark)
        {
            UseDarkModeT.IsChecked = true;
        }
        else if(Application.Current.ActualThemeVariant == ThemeVariant.Light)
        {
            UseDarkModeT.IsChecked = false;
        }
        FollowSystemThemeT.IsChecked = c.AppUseSystemTheme;
        if (FollowSystemThemeT.IsChecked == true)
        {
            UseDarkModeT.IsEnabled = false;
            
        }
        else
        {
            UseDarkModeT.IsEnabled = true;
        }
        if(c.DianMingFont != null)
        {
            FontButton.Content = $"  请选择字体:{c.DianMingFont}  ";
        }
        if (c.DianMingFontColor is { } color)
        {
            RootPicker.Color = color;
        }
    }

    private void FollowSystemThemeT_Click(object s,RoutedEventArgs e)
    {
        var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        c.AppUseSystemTheme = FollowSystemThemeT.IsChecked ?? false;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(),c);
        if(FollowSystemThemeT.IsChecked == true)
        {
            UseDarkModeT.IsEnabled = false;
            Application.Current.RequestedThemeVariant = ThemeVariant.Default;
            if (Application.Current.ActualThemeVariant == ThemeVariant.Dark)
            {
                UseDarkModeT.IsChecked = true;
            }
            else if (Application.Current.ActualThemeVariant == ThemeVariant.Light)
            {
                UseDarkModeT.IsChecked = false;
            }
        }
        else
        {
            UseDarkModeT.IsEnabled = true;
            if (Application.Current.ActualThemeVariant == ThemeVariant.Dark)
            {
                UseDarkModeT.IsChecked = true;
            }
            else if (Application.Current.ActualThemeVariant == ThemeVariant.Light)
            {
                UseDarkModeT.IsChecked = false;
            }
        }
    }

    private void UseDarkModeT_Click(object s,RoutedEventArgs e)
    {
        var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        c.AppUseDarkTheme = UseDarkModeT.IsChecked ?? false;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), c);
        if(UseDarkModeT.IsChecked == true)
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
        }
        else
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Light;
        }
    }

    private async void ChooseFont_Click(object s,RoutedEventArgs e)
    {
        var dialog = new FontDialog();
        if(RootClasses.MainWindow != null)
        {
            var accepted = await dialog.ShowDialog<bool>(RootClasses.MainWindow);
            if (accepted && dialog.SelectedFont != null)
            {
                var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
                c.DianMingFont = dialog.SelectedFont.Family.Name;
                JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), c);
                FontButton.Content = $"  请选择字体:{dialog.SelectedFont.Family.Name}  ";
            }
        }
        else
        {
            var accepted = await dialog.ShowDialog<bool>(RootClasses.OOBEWindow);
            if (accepted && dialog.SelectedFont != null)
            {
                var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
                c.DianMingFont = dialog.SelectedFont.Family.Name;
                JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), c);
                FontButton.Content = $"  请选择字体:{dialog.SelectedFont.Family.Name}  ";
            }
        }

        
    }

    private void ChooseFontColor_Click(object s, ColorChangedEventArgs e)
    {
        var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        c.DianMingFontColor = RootPicker.Color;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), c);
    }

    private async void DefaultFont_Click(object s, RoutedEventArgs e)
    {
       
        var dlg = new ContentDialog
        {
            Title = "警告",
            Content = $"是否确认要将字体个性化设置恢复默认？",
            PrimaryButtonText = "确定",
            SecondaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary
        };
        if (await dlg.ShowAsync() == ContentDialogResult.Primary)
        {
            var c = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
            c.DianMingFont = null;
            c.DianMingFontColor = null;
            FontButton.Content = $"  请选择字体  ";
            RootPicker.Color = Colors.White;
            JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), c);
        }
    }
}