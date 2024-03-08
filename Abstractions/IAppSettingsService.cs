using Abstractions.Data;

namespace Abstractions;

/// <summary>
/// Represents a service for managing application settings.
/// </summary>
public interface IAppSettingsService
{
    /// <summary>
    /// Gets the current application settings.
    /// </summary>
    IAppSettings Settings { get; }

    /// <summary>
    /// Saves the provided application settings.
    /// </summary>
    /// <param name="settings">The application settings to be saved.</param>
    /// <returns>The saved application settings.</returns>
    IAppSettings Save(IAppSettings settings);
}