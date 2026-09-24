using Avalonia;
using FluentAvalonia.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;

namespace ARCRemake.Utils
{
    public class RootClasses
    {
        public const string AppVersion = "1.1.0";
        public static string DianMingMode = "常规点名";
        public static string? EditListPath = null;
        public static MainWindow MainWindow;
        public static OOBEWindow OOBEWindow;
        public static HoverWindow HoverWindow;
		public static CancellationTokenSource DMPageCTS = new CancellationTokenSource();
        
        public static string ConfigPath()
        {
            return Path.Combine(Environment.CurrentDirectory,"Config.json");
        }
        public static string NameListFolder()
        {
            return Path.Combine(Environment.CurrentDirectory, "NameLists");
        }
        

    }

      
    

    
}
