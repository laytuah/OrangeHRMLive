using OpenQA.Selenium;
using OrangeHRMLive.Utilities;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Configuration
{
    [Binding]
    internal class Hooks
    {
        private readonly WebDriverSupport webDriverSupport;
        private static ExtentReport extentReport;
        public Hooks(WebDriverSupport _webDriverSupport, ExtentReport _extentReport)
        {
            webDriverSupport = _webDriverSupport;
            extentReport = _extentReport;
        }

        static Hooks()
        {
            extentReport = new ExtentReport();
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            extentReport.ExtentReportInitialization();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            extentReport.BeforeFeature(featureContext);
        }

        //[BeforeStep]
        //public void BeforeStep(ScenarioContext scenarioContext)
        //{
        //    extentReport.BeforeScenario(scenarioContext);
        //}

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
            extentReport.BeforeScenario(scenarioContext);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext, IWebDriver driver)
        {
            extentReport.AfterStep(scenarioContext, driver);
        }

        [AfterScenario]
        public void CloseBrowser(ScenarioContext scenarioContext)
        {
            //extentReport.PrintTestLog(scenarioContext);
            webDriverSupport.CloseAUT();
        }

        [AfterTestRun]
        public static void AfterTest()
        {
            extentReport.ExtentReportTearDown();
        }
    }
}
