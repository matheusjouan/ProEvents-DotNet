using Microsoft.Extensions.Configuration;

namespace ProEvents.Infra;

public static class ConnectionStringManager
{
    public static string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, reloadOnChange: true);

        return builder.Build().GetConnectionString("DefaultConnection");
    }
}
