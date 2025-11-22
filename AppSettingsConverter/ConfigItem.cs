// <copyright file="ConfigItem.cs" company="AppSettingsConverter">
// Copyright (c) 2024 AppSettingsConverter Contributors
// </copyright>

namespace AppSettingsConverter;

/// <summary>
/// Represents a flattened configuration item with name, value, and slot setting.
/// </summary>
public class ConfigItem
{
    /// <summary>
    /// Gets or sets the flattened configuration key name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the configuration value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this is a slot setting (used in Azure App Service).
    /// </summary>
    public bool SlotSetting { get; set; }
}

