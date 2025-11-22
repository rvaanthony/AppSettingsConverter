using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AppSettingsConverter.Tests;

public class IntegrationTests
{
    private static string GetExecutablePath()
    {
        var assemblyLocation = typeof(IntegrationTests).Assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        var projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.Parent;
        var exePath = Path.Combine(
            projectDirectory!.FullName,
            "AppSettingsConverter",
            "bin",
            "Release",
            "net10.0",
            "AppSettingsConverter.exe"
        );
        return exePath;
    }

    [Fact]
    public void Run_WithValidJsonFile_ProducesCorrectOutput()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var tempOutput = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, """{"ApiUrl": "www.api.com"}""");

            var exePath = GetExecutablePath();
            if (!File.Exists(exePath))
            {
                // Skip if executable doesn't exist (e.g., in CI before build)
                return;
            }

            // Act
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"{tempFile} __ --output {tempOutput}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Assert
            Assert.Equal(0, process.ExitCode);
            Assert.Contains("Output written to", output);
            Assert.True(File.Exists(tempOutput));
            var content = File.ReadAllText(tempOutput);
            Assert.Contains("ApiUrl", content);
            Assert.Contains("www.api.com", content);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
            if (File.Exists(tempOutput)) File.Delete(tempOutput);
        }
    }

    [Fact]
    public void Run_WithJsonFormat_ProducesValidJson()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var tempOutput = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, """{"ApiUrl": "www.api.com"}""");

            var exePath = GetExecutablePath();
            if (!File.Exists(exePath))
            {
                return;
            }

            // Act
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"{tempFile} __ --format json --output {tempOutput}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            // Assert
            Assert.Equal(0, process.ExitCode);
            var content = File.ReadAllText(tempOutput);
            var items = JsonSerializer.Deserialize<List<ConfigItem>>(content);
            Assert.NotNull(items);
            Assert.Single(items);
            Assert.Equal("ApiUrl", items![0].Name);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
            if (File.Exists(tempOutput)) File.Delete(tempOutput);
        }
    }

    [Fact]
    public void Run_WithMissingFile_ReturnsErrorCode()
    {
        // Arrange
        var exePath = GetExecutablePath();
        if (!File.Exists(exePath))
        {
            return;
        }

        // Act
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "nonexistent.json __",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        // Assert
        Assert.NotEqual(0, process.ExitCode);
        Assert.Contains("not found", error);
    }

    [Fact]
    public void Run_WithInvalidJson_ReturnsErrorCode()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "{ invalid json }");

            var exePath = GetExecutablePath();
            if (!File.Exists(exePath))
            {
                return;
            }

            // Act
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"{tempFile} __",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Assert
            Assert.NotEqual(0, process.ExitCode);
            Assert.Contains("Error parsing JSON", error);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void Run_WithHelpFlag_ShowsHelp()
    {
        // Arrange
        var exePath = GetExecutablePath();
        if (!File.Exists(exePath))
        {
            return;
        }

        // Act
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "--help",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Assert
        Assert.Equal(0, process.ExitCode);
        Assert.Contains("Usage", output);
        Assert.Contains("AppSettingsConverter", output);
    }
}

