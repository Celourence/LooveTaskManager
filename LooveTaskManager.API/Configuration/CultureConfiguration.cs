using System.Globalization;

namespace LooveTaskManager.API.Configuration;

public static class CultureConfiguration
{
    public static void ConfigureCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
    }
} 