using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace OrangeHRMLive.Configuration
{
    public static class ConfigurationManager
    {
        static readonly IConfiguration _configuration;

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public static string BrowserName => GetConfigurationValue("Browser");
        public static string Url => GetConfigurationValue("Site Url");
        public static string TesterName => GetConfigurationValue("Tester Name");
        public static string MobileDeviceName => GetConfigurationValue("Mobile Device Name");
        public static bool Headless => bool.Parse(GetConfigurationValue("Headless"));
        public static bool PrivateBrowser => bool.Parse(GetConfigurationValue("Private Browser"));
        public static string LoadingIconXpath => (GetConfigurationValue("Loading Icon Xpath"));
        public static string ApiBaseUrl   => GetConfigurationValue("Api Base Url");
        public static int ApiTimeoutSeconds => int.TryParse(GetConfigurationValue("Api Timeout Seconds"), out var s) ? s : 30;

        static string GetConfigurationValue(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException($"Configuration value for '{key}' is missing or empty.");
            }
            return value;
        }
    }
}

