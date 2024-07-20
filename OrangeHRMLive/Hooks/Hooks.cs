using OrangeHRMLive.Configuration;
using OrangeHRMLive.Utilities;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Hooks
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

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
            extentReport.BeforeScenario(scenarioContext);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            extentReport.AfterStep(scenarioContext, webDriverSupport.GetDriver());
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            webDriverSupport.CloseAUT();
        }

        [AfterTestRun]
        public static void AfterTest()
        {
            extentReport.ExtentReportTearDown();
        }
    }
}




//using OrangeHRMLive.Configuration;
//using OrangeHRMLive.Utilities;
//using TechTalk.SpecFlow;

//namespace OrangeHRMLive.Hooks
//{
//    [Binding]
//    internal class Hooks
//    {
//        private readonly WebDriverSupport _webDriverSupport;
//        private readonly ExtentReport _extentReport;

//        public Hooks(WebDriverSupport webDriverSupport, ExtentReport extentReport)
//        {
//            _webDriverSupport = webDriverSupport;
//            _extentReport = extentReport;
//        }

//        // Static methods for BeforeTestRun, AfterTestRun, BeforeFeature, AfterFeature
//        [BeforeTestRun]
//        public static void BeforeTestRun()
//        {
//            // Static instance of ExtentReport to be used in static context
//            var extentReport = new ExtentReport();
//            extentReport.ExtentReportInitialization();
//        }

//        [BeforeFeature]
//        public static void BeforeFeature(FeatureContext featureContext)
//        {
//            // Static instance of ExtentReport to be used in static context
//            var extentReport = new ExtentReport();
//            extentReport.BeforeFeature(featureContext);
//        }

//        [AfterTestRun]
//        public static void AfterTestRun()
//        {
//            // Static instance of ExtentReport to be used in static context
//            var extentReport = new ExtentReport();
//            extentReport.ExtentReportTearDown();
//        }

//        // Instance methods for BeforeScenario, AfterScenario, and AfterStep
//        [BeforeScenario]
//        public void BeforeScenario(ScenarioContext scenarioContext)
//        {
//            _webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
//            _extentReport.BeforeScenario(scenarioContext);
//        }

//        [AfterStep]
//        public void AfterStep(ScenarioContext scenarioContext)
//        {
//            _extentReport.AfterStep(scenarioContext, _webDriverSupport.GetDriver());
//        }

//        [AfterScenario]
//        public void AfterScenario()
//        {
//            _webDriverSupport.CloseAUT();
//        }
//    }
//}

