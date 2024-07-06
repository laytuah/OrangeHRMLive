using Microsoft.Extensions.Configuration;

namespace OrangeHRMLive.Configuration
{
    public static class ConfigurationManager
    {
        private static readonly IConfiguration Configuration;

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public static string BrowserName => Configuration["Browser"];
        public static string Url => Configuration["SiteUrl"];
        public static string TesterName => Configuration["TesterName"];
    }
}
