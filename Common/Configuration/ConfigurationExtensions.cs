using Microsoft.Extensions.Configuration;

namespace HelseId.Samples.Common.Configuration;

public static class ConfigurationExtensions
{
    public static T GetOptions<T>(this IConfiguration configuration) where T : ConfigurationOptions, new()
    {
        var optionsInstance = new T();
        var options = configuration.GetSection(optionsInstance.FeatureName).Get<T>();
        if (options == null)
        {
            throw new Exception($"Could not find the configuration parameter '{optionsInstance.FeatureName}' in the configuration properties.");
        }
        return options;
    }
}