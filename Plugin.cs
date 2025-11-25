using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.WebOSScrollFix;

/// <summary>
/// Plugin that fixes WebOS TV Guide scroll issue using File Transformation.
/// </summary>
public class Plugin : BasePlugin<PluginConfiguration>
{
    private readonly ILogger<Plugin> _logger;

    public Plugin(
        IApplicationPaths applicationPaths,
        ILoggerFactory loggerFactory,
        IJsonSerializer jsonSerializer,
        IConfigurationManager configurationManager)
        : base(applicationPaths, loggerFactory, jsonSerializer, configurationManager)
    {
        _logger = loggerFactory.CreateLogger<Plugin>();
        Instance = this;
        
        // Register transformation after a short delay to ensure File Transformation is loaded
        Task.Delay(2000).ContinueWith(_ => RegisterFileTransformation());
    }

    public static Plugin? Instance { get; private set; }

    /// <inheritdoc />
    public override string Name => "WebOS Scroll Fix";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");

    /// <inheritdoc />
    public override string Description => "Fixes TV Guide scroll issue on WebOS devices. Uses File Transformation plugin to inject CSS and JavaScript.";

    /// <summary>
    /// Registers the file transformation with File Transformation plugin.
    /// </summary>
    private void RegisterFileTransformation()
    {
        try
        {
            // Find File Transformation assembly using reflection
            Assembly? fileTransformationAssembly = AssemblyLoadContext.All
                .SelectMany(x => x.Assemblies)
                .FirstOrDefault(x => x.FullName?.Contains(".FileTransformation") ?? false);

            if (fileTransformationAssembly == null)
            {
                _logger.LogWarning("File Transformation plugin not found. Please install it first.");
                return;
            }

            Type? pluginInterfaceType = fileTransformationAssembly.GetType("Jellyfin.Plugin.FileTransformation.PluginInterface");

            if (pluginInterfaceType == null)
            {
                _logger.LogWarning("File Transformation PluginInterface not found.");
                return;
            }

            MethodInfo? registerMethod = pluginInterfaceType.GetMethod("RegisterTransformation");

            if (registerMethod == null)
            {
                _logger.LogWarning("File Transformation RegisterTransformation method not found.");
                return;
            }

            // Create transformation payload
            var payload = new
            {
                id = Guid.NewGuid().ToString(),
                fileNamePattern = @"index\.html$",
                callbackAssembly = Assembly.GetExecutingAssembly().FullName,
                callbackClass = "Jellyfin.Plugin.WebOSScrollFix.TransformationHandler",
                callbackMethod = "TransformIndexHtml"
            };

            // Register the transformation
            registerMethod.Invoke(null, new object?[] { JsonSerializer.Serialize(payload) });

            _logger.LogInformation("WebOS Scroll Fix transformation registered successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register file transformation. Make sure File Transformation plugin is installed.");
        }
    }
}

