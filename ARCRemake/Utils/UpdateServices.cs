using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using LiveMarkdown.Avalonia;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
    }
}
