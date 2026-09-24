using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace ARCRemake
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = "Microsoft YaHei UI",
                FontFallbacks = new[]
            {
                new FontFallback { FontFamily = new FontFamily("Noto Sans CJK SC") },
                new FontFallback { FontFamily = new FontFamily("Segoe UI") },
                new FontFallback { FontFamily = new FontFamily("Arial") } 
            }
            })
                
                .LogToTrace();
    }
}
