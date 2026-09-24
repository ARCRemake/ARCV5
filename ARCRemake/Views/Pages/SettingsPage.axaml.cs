using ARCRemake.Utils;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ARCRemake;

public partial class SettingsPage : UserControl
{

    public SettingsPage()
    {
        InitializeComponent();
        var a2 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        CGDM.Value = a2.IntervalTick;
        DSDM.Value = a2.ScheduledSeconds;
        PLDM.Value = a2.BatchCounts;
        foreach (var animation in RootPanel.Children)
        {

            animation.IsVisible = false;
        }

    }

    private async void Page_Loaded(object s, RoutedEventArgs e)
    {
        await StartSPAnimation(RootPanel);
        NameListBox.Items.Clear();
        foreach (var a in Directory.EnumerateFiles(RootClasses.NameListFolder(), "*.json"))
        {
            try
            {
                var b = JsonServices.ReadJson<NameListConfig>(a);
                var ci = new ComboBoxItem
                {
                    Content = $"{b.ListName}（{a}）",
                    Tag = $"{a}"
                };

                NameListBox.Items.Add(ci);
            }
            catch
            {
                continue;
            }

        }
        var a2 = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        foreach (var a4 in NameListBox.Items)
        {
            if (a4 is ComboBoxItem a5)
            {
                if (Path.GetFileName((string)a5.Tag) == Path.GetFileName(a2.CurrentNameListPath))
                {
                    NameListBox.SelectedItem = a5;
                    break;
                }
            }
        }



        CGDM.Value = a2.IntervalTick;
        DSDM.Value = a2.ScheduledSeconds;
        PLDM.Value = a2.BatchCounts;
        UsingHoverBall.IsChecked = a2.UsingHoverBall;
        StartUpCheckUpdate.IsChecked = a2.StartUpCheckUpdate;
    }

    private void NameListBox_SelectionChanged(object s, RoutedEventArgs e)
    {

        if (NameListBox.SelectedItem is ComboBoxItem cbi)
        {
            if (cbi.Tag is string cbitag)
            {
                if (cbitag != null)
                {
                    var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
                    a.CurrentNameListPath = (string)((ComboBoxItem)NameListBox.SelectedItem).Tag;
                    JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
                }
            }
        }
    }

    private void Page_UnLoaded(object s, RoutedEventArgs e)
    {
        foreach (var animation in RootPanel.Children)
        {

            animation.IsVisible = false;
        }
    }



    public async Task StartSPAnimation(StackPanel sp)
    {


        foreach (var animation in sp.Children)
        {

            animation.IsVisible = false;
            var transform = new TranslateTransform();
            animation.RenderTransform = transform;
            transform.X = -1500;
            animation.IsVisible = true;
            transform.Transitions = new Transitions
            {
                new DoubleTransition
                {
                    Property = TranslateTransform.XProperty,
                    Duration = TimeSpan.FromMilliseconds(500),
                    Easing = new ExponentialEaseInOut()
                }
            };
            transform.X = 0;
            await Task.Delay(50);


        }

    }

    private async void Recovery_Click(object s, RoutedEventArgs e)
    {
        var dlg = new ContentDialog
        {
            Title = "警告",
            Content = $"是否确认要将程序的所有配置永久恢复默认？(真的很久！)",
            PrimaryButtonText = "确定",
            SecondaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary
        };
        if (await dlg.ShowAsync() == ContentDialogResult.Primary)
        {
            JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), ConfigHelper.InitConfig());
            Directory.Delete(RootClasses.NameListFolder(), true);
            var exePath = Environment.ProcessPath
                ?? throw new InvalidOperationException("无法获取当前可执行文件路径。");

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = false,
                WorkingDirectory = AppContext.BaseDirectory,
            };



            Process.Start(psi);

            if (Application.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
            else
            {
                Environment.Exit(0);
            }
        }

    }

    private void CGDM_TextChanged(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.IntervalTick = (int)CGDM.Value;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
    }

    private void DSDM_TextChanged(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.ScheduledSeconds = (int)DSDM.Value;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
    }

    private void PLDM_TextChanged(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.BatchCounts = (int)PLDM.Value;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
    }

    private void AddNameList_Click(object s, RoutedEventArgs e)
    {
        RootClasses.EditListPath = null;
        RootClasses.MainWindow.RootFrame.Navigate(typeof(NameListPage));

    }

    private void ModifyNameList_Click(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        RootClasses.EditListPath = a.CurrentNameListPath;
        RootClasses.MainWindow.RootFrame.Navigate(typeof(NameListPage));

    }

    private async void DelNameList_Click(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        var dlg = new ContentDialog
        {
            Title = "警告",
            Content = $"是否确认删除名单“{a.CurrentNameListPath}”？它将会永久删除！(真的很久！)",
            PrimaryButtonText = "确定",
            SecondaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary
        };
        if (await dlg.ShowAsync() == ContentDialogResult.Primary)
        {
            File.Delete(a.CurrentNameListPath);
            if (Directory.EnumerateFiles(RootClasses.NameListFolder()).Count() == 0)
            {
                a.CurrentNameListPath = "";
                JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
                NameListBox.Items.Clear();
            }
            else
            {
                var a2 = Directory.EnumerateFiles(RootClasses.NameListFolder()).ToList();
                a.CurrentNameListPath = a2[0];
                JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
                NameListBox.Items.Clear();
                foreach (var a3 in Directory.EnumerateFiles(RootClasses.NameListFolder(), "*.json"))
                {
                    try
                    {
                        var b = JsonServices.ReadJson<NameListConfig>(a3);
                        var ci = new ComboBoxItem
                        {
                            Content = $"{b.ListName}（{a3}）",
                            Tag = $"{a3}"
                        };

                        NameListBox.Items.Add(ci);
                    }
                    catch
                    {
                        continue;
                    }

                }
                NameListBox.SelectedIndex = 0;
            }
        }

    }

    private void UsingHoverBall_Click(object s, RoutedEventArgs e)
    {


        if (UsingHoverBall.IsChecked == true)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var existing = desktop.Windows
                    .OfType<HoverWindow>()
                    .FirstOrDefault(w => w.IsVisible);

                if (existing == null)
                {
                    RootClasses.HoverWindow = new HoverWindow();
                    RootClasses.HoverWindow.Show();
                }
            }


        }
        else
        {
            RootClasses.HoverWindow.Close();
        }
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.UsingHoverBall = UsingHoverBall.IsChecked ?? true;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
    }

    private void StartUpCheckUpdate_Click(object s, RoutedEventArgs e)
    {
        var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        a.StartUpCheckUpdate = StartUpCheckUpdate.IsChecked ?? true;
        JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), a);
    }

    private async void Import_Click(object s, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "打开新配置项",
            AllowMultiple = false,
            FileTypeFilter = new[] {
            new FilePickerFileType("配置文件") { Patterns = new[] { "*.json" } }
        }
        });
        await using var stream = await files[0].OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();
        try
        {
            var newcfg = JsonServices.ReadJson<AppConfig>(fileContent);
            var dlg = new ContentDialog
            {
                Title = "提示",
                Content = $"是否要覆盖到当前配置文件？",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            if (await dlg.ShowAsync() == ContentDialogResult.Primary)
            {
                if (!File.Exists(newcfg.CurrentNameListPath))
                {
                    var dlg2 = new ContentDialog
                    {
                        Title = "警告",
                        Content = $"配置项不合法，请重新选择。",
                        PrimaryButtonText = "确定",
                        SecondaryButtonText = "取消",
                        DefaultButton = ContentDialogButton.Primary
                    };
                    await dlg2.ShowAsync();
                    return;
                }
                File.Copy(newcfg.CurrentNameListPath, Path.Combine(RootClasses.NameListFolder(), Path.GetFileName(newcfg.CurrentNameListPath)));
                JsonServices.WriteJson<AppConfig>(RootClasses.ConfigPath(), newcfg);

                var dl2g = new ContentDialog
                {
                    Title = "提示",
                    Content = $"导入配置成功。需要重启以应用配置项。",
                    PrimaryButtonText = "确定",
                    DefaultButton = ContentDialogButton.Primary
                };
                await dl2g.ShowAsync();
                var exePath = Environment.ProcessPath
               ?? throw new InvalidOperationException("无法获取当前可执行文件路径。");

                var psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = false,
                    WorkingDirectory = AppContext.BaseDirectory,
                };



                Process.Start(psi);

                if (Application.Current?.ApplicationLifetime
                    is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
        catch
        {
            var dlg = new ContentDialog
            {
                Title = "警告",
                Content = $"配置项不合法，请重新选择。",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
        }


    }

    private async void Export_Click(object s, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存配置文件",
            FileTypeChoices = new[] {
            new FilePickerFileType("配置文件") { Patterns = new[] { "*.json" } }
        }
        });

        if (file is not null)
        {
            JsonServices.WriteJson<AppConfig>(file.TryGetLocalPath(), JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath()));
            var dlg = new ContentDialog
            {
                Title = "提示",
                Content = $"导出配置成功。",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
        }
    }
    private async void ImportNL_Click(object s, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "导入名单配置文件",
            AllowMultiple = false,
            FileTypeFilter = new[] {
            new FilePickerFileType("名单文件") { Patterns = new[] { "*.json" } }
        }
        });
        try
        {
            var k = JsonServices.ReadJson<NameListConfig>(files[0].TryGetLocalPath());
            File.Copy(files[0].TryGetLocalPath(), Path.Combine(RootClasses.NameListFolder(), $"{files[0].Name}"));
            var dlg = new ContentDialog
            {
                Title = "提示",
                Content = $"导入名单成功。",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
            var ci = new ComboBoxItem
            {
                Content = $"{k.ListName}（{files[0].TryGetLocalPath()}）",
                Tag = $"{files[0].TryGetLocalPath()}"
            };

            NameListBox.Items.Add(ci);
        }
        catch
        {
            var dlg = new ContentDialog
            {
                Title = "警告",
                Content = $"配置项不合法，请重新选择。",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
        }

    }

    private async void ExportNL_Click(object s, RoutedEventArgs e)
    {
		var a = JsonServices.ReadJson<AppConfig>(RootClasses.ConfigPath());
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存名单文件",
            FileTypeChoices = new[] {
            new FilePickerFileType("配置文件") { Patterns = new[] { "*.json" } }
        }
        });

        if (file is not null)
        {
            JsonServices.WriteJson<NameListConfig>(file.TryGetLocalPath(), JsonServices.ReadJson<NameListConfig>(a.CurrentNameListPath));
            var dlg = new ContentDialog
            {
                Title = "提示",
                Content = $"导出名单成功。",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary
            };
            await dlg.ShowAsync();
        }
    }


}