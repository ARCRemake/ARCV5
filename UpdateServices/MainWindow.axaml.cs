using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace UpdateServices
{
    //应依照如下方法给参: UpdateAPI.exe <Websitepath> <Outputpath>
    public partial class MainWindow : AppWindow
    {
        private string[] args;
        private string Websitepath;
        private string Outputpath;
        private string StartApplicationName;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            args = Environment.GetCommandLineArgs();
            if (args.Length != 3)
            {
                await new ContentDialog
                {
                    Title = "错误",
                    Content = $"应依照如下方法给参: UpdateAPI.exe <Websitepath> <Outputpath>",
                    PrimaryButtonText = "确定并退出",
                    DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
                Environment.Exit(0);
            }
            Websitepath = args[1];
            Outputpath = Path.GetDirectoryName(args[2]);
            StartApplicationName = args[2];
            log.Text += "尝试启动下载……\r\n";
            bool DownloadStatus = false;
            if (File.Exists(Path.GetTempPath() + "\\Temp.zip"))
            {
                File.Delete(Path.GetTempPath() + "\\Temp.zip");
            }
            try
            {

                var downloader = new Downloader
                {
                    Url = Websitepath,
                    SavePath = Path.GetTempPath() + "\\Temp.zip",
                    Completed = (async (s, e) =>
                    {
                        if (s)
                        {

                            log.Text += "下载已完成。\r\n";
                            log.Text += "正在尝试解压资源文件……\r\n";

                            DownloadStatus = true;

                        }
                        else
                        {
                            await new ContentDialog
                            {
                                Title = "错误",
                                Content = $"下载时发生错误：{e}",
                                PrimaryButtonText = "确定并退出",
                                DefaultButton = ContentDialogButton.Primary
                            }.ShowAsync();
                            Environment.Exit(0);
                        }
                    }),
                    Progress = ((p, s) =>
                    {
                        //Console.WriteLine($"目前进度:{(int)p}%");
                        progressbar.Value = ((int)p) * 0.9;
                        progresstext.Text = "当前进度：" + (((int)p) * 0.9).ToString() + "%";

                    })
                };
                downloader.StartDownload();
                log.Text += "下载启动成功，请稍候……。\r\n";
                while (DownloadStatus == false)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }


                try
                {
                    ZipFile.ExtractToDirectory(Path.GetTempPath() + "\\Temp.zip", Outputpath, true);

                    log.Text += "解压已完成，正在进行清理，即将退出程序……\r\n";
                    File.Delete(Path.GetTempPath() + "\\Temp.zip");
                }
                catch (Exception ex)
                {
                    await new ContentDialog
                    {
                        Title = "错误",
                        Content = $"解压时发生错误：{ex.Message}",
                        PrimaryButtonText = "确定并退出",
                        DefaultButton = ContentDialogButton.Primary
                    }.ShowAsync();
                    Environment.Exit(0);
                }
                progresstext.Text = "当前进度：100%";
                progressbar.Value = 100;
                await Task.Delay(100);
                var stdinfo = new ProcessStartInfo
                {
                    FileName = StartApplicationName,
                    WorkingDirectory = Outputpath,
                    UseShellExecute = true
                };
                Process.Start(stdinfo);
                await Task.Delay(100);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title="错误",
                    Content=$"启动时发生错误：{ex.Message}",
                    PrimaryButtonText="确定并退出",
                    DefaultButton=ContentDialogButton.Primary
                }.ShowAsync();
                
                Environment.Exit(0);
            }
        }
    }
}