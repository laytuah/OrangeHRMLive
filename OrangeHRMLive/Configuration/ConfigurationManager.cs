using Microsoft.Extensions.Configuration;

namespace OrangeHRMLive.Configuration
{
    public static class ConfigurationManager
    {
        private static IConfiguration Configuration;

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public static string BrowserName => Configuration["Browser"];
        public static string Url => Configuration["Site Url"];
        public static string TesterName => Configuration["Tester Name"];
        public static string MobileDeviceName => Configuration["Mobile Device Name"];
        public static bool Headless => bool.Parse(Configuration["Headless"]);
    }
}
