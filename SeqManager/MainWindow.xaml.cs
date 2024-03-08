using Microsoft.Win32;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using Abstractions;
using Autofac;
using SeqManager.ViewModels;

namespace SeqManager;

public partial class MainWindow
{
    private readonly SettingsViewModel settingsViewModel;
    private readonly IAppSettingsService appSettingsService;
    private readonly ILogFileImportService logFileImportService;

    public MainWindow()
    {
        var container = ContainerConfig.ConfigureContainer();
        appSettingsService = container.Resolve<IAppSettingsService>();
        logFileImportService = container.Resolve<ILogFileImportService>();
        
        settingsViewModel = new SettingsViewModel();
        settingsViewModel.Reset(appSettingsService);
        DataContext = settingsViewModel;
        
        InitializeComponent();
    }

    private void SelectFiles_Click(object sender, RoutedEventArgs e)
    {
        var fileSelection = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "JSON-Files|*.json"
        };

        if (fileSelection.ShowDialog() is false)
        {
            return;
        }
        
        foreach (var filePath in fileSelection.FileNames)
        {
            if (logFileImportService.TryAddFilePath(filePath, out var fileName))
            {
                LogListBox.Items.Add(fileName);
            }
        }
    }

    private void DiscardSelection_Click(object sender, RoutedEventArgs e)
    {
        LogListBox.Items.Clear();
        logFileImportService.ClearLogFilePaths();
    }
    
    private async void StartSeq_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ProgressBar.Visibility = Visibility.Visible;
            var result = await logFileImportService.TryStartAsync();

            if (result.Success is false)
            {
                MessageBox.Show(
                    result.Error, 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error: {ex.Message}", 
                "Error", 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);
        }
        finally
        {
            ProgressBar.Visibility = Visibility.Collapsed;
        }
    }

    private void LogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LogListBox.SelectedItem is null)
        {
            return;
        }
        
        SystemSounds.Exclamation.Play();

        var result = MessageBox.Show(
            $"Do you want to remove '{LogListBox.SelectedItem}' from the selection?",
            "Remove Item",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            var selectedIndex = LogListBox.Items.IndexOf(LogListBox.SelectedItem);
            LogListBox.Items.RemoveAt(selectedIndex);
            logFileImportService.RemoveLogFilePathAt(selectedIndex);
        }

        LogListBox.SelectedItem = null;
    }
    
    private void ShowSettings_Click(object sender, RoutedEventArgs e)
    {
        MainPanel.Visibility = Visibility.Collapsed;
        SettingsPanel.Visibility = Visibility.Visible;
    }

    private void CancelSettings_Click(object sender, RoutedEventArgs e)
    {
        MainPanel.Visibility = Visibility.Visible;
        SettingsPanel.Visibility = Visibility.Collapsed;
        settingsViewModel.Reset(appSettingsService);
    }

    private void SaveSettings_Click(object sender, RoutedEventArgs e)
    {
        MainPanel.Visibility = Visibility.Visible;
        SettingsPanel.Visibility = Visibility.Collapsed;
        settingsViewModel.SaveChanges(appSettingsService);
    }
}
