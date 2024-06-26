using TechTalk.SpecFlow;
using AventStack.ExtentReports.Reporter;

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
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string reportPath = projectDirectory + "//report.html";
            var htmlReporter = new ExtentHtmlReporter(reportPath);
        }
    }
}
