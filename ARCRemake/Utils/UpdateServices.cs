using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using LiveMarkdown.Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ARCRemake.Utils
{
    public class UpdateServices
    {
        public static async Task CheckUpdateAsync(bool Silent)
        {
            var a = new HttpClient();
            try
            {
                var cfg = await a.GetStringAsync("https://raw.giteeusercontent.com/Wang120229/ARCRemake.UpdateService/raw/master/Index.json");
                if (cfg != null) {
                    var d = JsonServices.ReadJson<UpdateConfig>(cfg);
                    if(d.LatestVersion != RootClasses.AppVersion)
                    {
                        var sp = new StackPanel();
                        sp.Children.Add(new TextBlock
                        {
                            Text = $"新版本可用！最新版本：{d.LatestVersion}",
                            FontWeight = Avalonia.Media.FontWeight.Bold
                        });
                        var k = new ObservableStringBuilder();
                        k.Append(await a.GetStringAsync("https://raw.giteeusercontent.com/Wang120229/ARCRemake.UpdateService/raw/master/Index.md"));
                        sp.Children.Add(new MarkdownRenderer
                        {
                            MarkdownBuilder = k
                        });
                        var dlg = new ContentDialog
                        {
                            Title = "提示",
                            Content = sp,
                            DefaultButton = ContentDialogButton.Primary,
                            PrimaryButtonText = "确定"
                        };
                        if(await dlg.ShowAsync() == ContentDialogResult.Primary)
                        {
                            if(GetSystem() == "win-x64")
                            {
                                if(File.Exists($"{Path.GetTempPath}\\UpdateServices-winx64.exe"))
                                {
                                    File.Delete($"{Path.GetTempPath}\\UpdateServices-winx64.exe");
                                }
                                File.Copy($"{Environment.CurrentDirectory}\\UpdateServices-winx64.exe", $"{Path.GetTempPath}\\UpdateServices-winx64.exe");
                                var m = new ProcessStartInfo
                                {
                                    WorkingDirectory = Path.GetTempPath(),
                                    FileName = "UpdateServices-winx64.exe",
                                };
                                m.ArgumentList.Add($"{d.LatestLink}");
                                m.ArgumentList.Add($"{Environment.ProcessPath}");
                                Process.Start(m);
                            }
                            else if (GetSystem() == "win-x86")
                            {
                                if (File.Exists($"{Path.GetTempPath}\\UpdateServices-winx86.exe"))
                                {
                                    File.Delete($"{Path.GetTempPath}\\UpdateServices-winx86.exe");
                                }
                                File.Copy($"{Environment.CurrentDirectory}\\UpdateServices-winx86.exe", $"{Path.GetTempPath}\\UpdateServices-winx86.exe");
                                var m = new ProcessStartInfo
                                {
                                    WorkingDirectory = Path.GetTempPath(),
                                    FileName = "UpdateServices-winx86.exe",
                                };
                                m.ArgumentList.Add($"{d.LatestLink}");
                                m.ArgumentList.Add($"{Environment.ProcessPath}");
                                Process.Start(m);
                            }
                            else if (GetSystem() == "linux-arm64")
                            {
                                if (File.Exists($"{Path.GetTempPath}\\UpdateServices-linuxarm64"))
                                {
                                    File.Delete($"{Path.GetTempPath}\\UpdateServices-linuxarm64");
                                }
                                File.Copy($"{Environment.CurrentDirectory}\\UpdateServices-linuxarm64", $"{Path.GetTempPath}\\UpdateServices-linuxarm64");
                                var m = new ProcessStartInfo
                                {
                                    WorkingDirectory = Path.GetTempPath(),
                                    FileName = "UpdateServices-linuxarm64",
                                };
                                m.ArgumentList.Add($"{d.LatestLink}");
                                m.ArgumentList.Add($"{Environment.ProcessPath}");
                                Process.Start(m);
                            }
                            else if (GetSystem() == "linux-x64")
                            {
                                if (File.Exists($"{Path.GetTempPath}\\UpdateServices-linuxx64"))
                                {
                                    File.Delete($"{Path.GetTempPath}\\UpdateServices-linuxx64");
                                }
                                File.Copy($"{Environment.CurrentDirectory}\\UpdateServices-linuxx64", $"{Path.GetTempPath}\\UpdateServices-linuxx64");
                                var m = new ProcessStartInfo
                                {
                                    WorkingDirectory = Path.GetTempPath(),
                                    FileName = "UpdateServices-linuxx64",
                                };
                                m.ArgumentList.Add($"{d.LatestLink}");
                                m.ArgumentList.Add($"{Environment.ProcessPath}");
                                Process.Start(m);
                            }
                        }
                    }
                    else
                    {
                        if(!Silent)
                        {
                            var dlg = new ContentDialog
                            {
                                Title = "提示",
                                Content = "当前版本已经是最新版，无需更新。",
                                DefaultButton = ContentDialogButton.Primary,
                                PrimaryButtonText = "确定"
                            };
                            await dlg.ShowAsync();
                        }
                    }
                }
            }
            catch (Exception ex) { 
                if(!Silent)
                {
                    var dlg = new ContentDialog
                    {
                        Title = "提示",
                        Content = $"检查更新时发生错误：\r\n{ex}",
                        DefaultButton = ContentDialogButton.Primary,
                        PrimaryButtonText = "确定"
                    };
                }
            }
        }

        public static string GetSystem()
        {
            var arch = RuntimeInformation.ProcessArchitecture;

            if (OperatingSystem.IsWindows())
            {
                return arch switch
                {
                    Architecture.X64 => "win-x64",
                    Architecture.X86 => "win-x86",
                    _ => "unknown"
                };
            }

            if (OperatingSystem.IsLinux())
            {
                return arch switch
                {
                    Architecture.Arm64 => "linux-arm64",
                    Architecture.X64 => "linux-x64",
                    _ => "unknown"
                };
            }

            return "unknown";
        }
    }
}
