using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace ARCRemake
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (!Directory.Exists(RootClasses.NameListFolder()))
                {
                    Directory.CreateDirectory(RootClasses.NameListFolder());

                }
                if (File.Exists(RootClasses.ConfigPath()))
                {
                    try
                    {
                        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
                        if (a.OOBEStatus == false)
                        {
                            JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), ConfigHelper.InitConfig());
                            desktop.MainWindow = new OOBEWindow();
                            base.OnFrameworkInitializationCompleted();
                            return;

                        }
                        if(!a.AppUseSystemTheme)
                        {
                            if(a.AppUseDarkTheme)
                            {
                                Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
                            }
                            else
                            {
                                Application.Current.RequestedThemeVariant = ThemeVariant.Light;
                            }
                        }
                        desktop.MainWindow = new MainWindow();
                        base.OnFrameworkInitializationCompleted();
                        return;
                    }
                    catch
                    {
                        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), ConfigHelper.InitConfig());
                        desktop.MainWindow = new OOBEWindow();
                        base.OnFrameworkInitializationCompleted();
                        return;
                    }

                }
                else
                {
                    JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), ConfigHelper.InitConfig());
                    desktop.MainWindow = new OOBEWindow();
                    base.OnFrameworkInitializationCompleted();
                    return;
                }
            }



        }
    }
}