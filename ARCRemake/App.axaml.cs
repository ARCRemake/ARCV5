using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
                        if (JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath()).OOBEStatus == false)
                        {
                            JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), ConfigHelper.InitConfig());
                            desktop.MainWindow = new OOBEWindow();
                            base.OnFrameworkInitializationCompleted();
                            return;

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