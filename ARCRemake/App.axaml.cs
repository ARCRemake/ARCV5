using ARCRemake.Utils;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
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
                if(File.Exists($"{Environment.CurrentDirectory}/Config.json"))
                {
                    if(JsonServices.ReadJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json").OOBEStatus == false)
                    {
                        JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json", ConfigHelper.InitConfig());
                        desktop.MainWindow = new OOBEWindow();
                        base.OnFrameworkInitializationCompleted();
                        return;

                    }
                    desktop.MainWindow = new MainWindow();
                    base.OnFrameworkInitializationCompleted();
                    return;
                }
                else
                {
                    JsonServices.WriteJson<AppConfig>($"{Environment.CurrentDirectory}/Config.json", ConfigHelper.InitConfig());
                    desktop.MainWindow = new OOBEWindow();
                    base.OnFrameworkInitializationCompleted();
                    return;
                }
            }
            

            
        }
    }
}