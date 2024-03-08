using System.Text.Json;
using Abstractions;
using Abstractions.Data;
using Services.Data;

namespace Services;

public class AppSettingsService : IAppSettingsService
{
    private const string SettingsFilePath = "appSettings.json";

    public IAppSettings Settings => Read() ?? Save(new AppSettings());
    
    public IAppSettings Save(IAppSettings settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFilePath, json);
            return settings;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
            return new AppSettings();
        }
    }

    private static AppSettings? Read()
    {
        try
        {
            if (File.Exists(SettingsFilePath))
            {
                var jsonString = File.ReadAllText(SettingsFilePath);
                return JsonSerializer.Deserialize<AppSettings>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings: {ex.Message}");
        }

        return null;
    }
}