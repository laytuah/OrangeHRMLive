using TechTalk.SpecFlow;

namespace OrangeHRMLive.Configuration
{
    [Binding]
    internal class Hooks
    {
        private readonly WebDriverSupport webDriverSupport;
        public Hooks(WebDriverSupport _webDriverSupport)
        {
            webDriverSupport = _webDriverSupport;
        }

        [BeforeScenario]
        public void StartBrowser()
        {
            webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            webDriverSupport.CloseAUT();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            var htmlReporter = new ExtentHtmlReporter();
        }
    }
}
