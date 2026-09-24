using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARCRemake.Utils
{
    
    public class NameListConfig
    {
        public required string ListName { get; set; }
        public required List<string> Names { get; set; }
    }

    public class AppConfig
    {
        public required bool AppUseDarkTheme { get; set; }
        public required bool AppUseSystemTheme { get; set; }
        public required bool OOBEStatus { get; set; }
        public required string CurrentNameListPath { get; set;  }
        public required int IntervalTick { get; set; }
        public required int ScheduledSeconds { get; set; }
        public required int BatchCounts { get; set; }
        public required bool StartUpCheckUpdate { get; set; }
        public required bool UsingHoverBall { get; set; }
        public required int HoverWindowX { get; set; }
        public required int HoverWindowY { get; set; }
        public required Color? DianMingFontColor { get; set; }
        public required string? DianMingFont { get; set; }
    }

    public class UpdateConfig
    {
        public required string LatestVersion { get; set; }
        public required string LatestLink { get; set; }
    }
    
    public static class ConfigHelper
    {
        public static AppConfig InitConfig()
        {
            return new AppConfig
            {
                AppUseDarkTheme = true,
                AppUseSystemTheme = true,
                OOBEStatus = false,
                CurrentNameListPath = "",
                IntervalTick = 50,
                ScheduledSeconds = 10,
                BatchCounts = 3,
                StartUpCheckUpdate = true,
                UsingHoverBall = true,
                HoverWindowX = 100,
                HoverWindowY = 100,
                DianMingFont = null,
                DianMingFontColor = null
            };
        }
    }
}
