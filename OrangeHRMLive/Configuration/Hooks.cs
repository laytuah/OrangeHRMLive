using AventStack.ExtentReports;
using OpenQA.Selenium;
using OrangeHRMLive.Reports;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Configuration
{
    [Binding]
    internal class Hooks
    {
        private readonly WebDriverSupport webDriverSupport;
        private static ExtentReports extent;
        private static TestReport testReport;
        public Hooks(WebDriverSupport _webDriverSupport, TestReport _testReport)
        {
            webDriverSupport = _webDriverSupport;
            testReport = _testReport;
        }

        static Hooks()
        {
            testReport = new TestReport();
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            testReport.ReportSetup();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            testReport.BeforeFeature();
        }

        [BeforeStep]
        public void BeforeStep(ScenarioContext scenarioContext)
        {
            testReport.BeforeStep(scenarioContext);
        }

        [BeforeScenario]
        public void StartBrowser()
        {
            webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
        }


        [AfterScenario]
        public void CloseBrowser(IWebDriver driver)
        {
            testReport.AfterScenario(driver);
            webDriverSupport.CloseAUT();
        }

        [AfterTestRun]
        public static void AfterTest()
        {
            testReport.AfterTestRun();
        }
    }
}
