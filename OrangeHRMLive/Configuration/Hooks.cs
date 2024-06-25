using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Configuration
{
    [Binding]
    internal class Hooks
    {
        private readonly WebDriverSupport webDriverSupport;
        public IConfiguration Configuration { get; }
        public Hooks(WebDriverSupport _webDriverSupport)
        {
            webDriverSupport = _webDriverSupport;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        [BeforeScenario]
        public void StartBrowser()
        {
            string browsername = Configuration["Browser"];
            webDriverSupport.InitializeBrowser(browsername);
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            webDriverSupport.CloseAUT();
        }
    }
}
