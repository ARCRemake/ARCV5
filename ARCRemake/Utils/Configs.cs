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
                CurrentNameListPath = ""
            };
        }
    }
}
