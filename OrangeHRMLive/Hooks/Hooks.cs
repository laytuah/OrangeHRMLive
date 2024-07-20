using OrangeHRMLive.Configuration;
using OrangeHRMLive.Utilities;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Hooks
{
    [Binding]
    internal class Hooks
    {
        private readonly WebDriverSupport _webDriverSupport;
        private static ExtentReport _extentReport;

        public Hooks(WebDriverSupport webDriverSupport, ExtentReport extentReport)
        {
            _webDriverSupport = webDriverSupport;
            _extentReport = extentReport;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _extentReport = new ExtentReport();
            _extentReport.ExtentReportInitialization();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _extentReport.BeforeFeature(featureContext);
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
            _extentReport.BeforeScenario(scenarioContext);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            _extentReport.AfterStep(scenarioContext, _webDriverSupport.GetDriver());
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _webDriverSupport.CloseAUT();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _extentReport.ExtentReportTearDown();
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
//        private static ExtentReport _extentReport;
//        public Hooks(WebDriverSupport webDriverSupport, ExtentReport extentReport)
//        {
//            _webDriverSupport = webDriverSupport;
//            _extentReport = extentReport;
//        }

//        static Hooks()
//        {
//            _extentReport = new ExtentReport();
//        }

//        [BeforeTestRun]
//        public static void BeforeTestRun()
//        {
//            _extentReport.ExtentReportInitialization();
//        }

//        [BeforeFeature]
//        public static void BeforeFeature(FeatureContext featureContext)
//        {
//            _extentReport.BeforeFeature(featureContext);
//        }

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
//        public void CloseBrowser()
//        {
//            _webDriverSupport.CloseAUT();
//        }

//        [AfterTestRun]
//        public static void AfterTest()
//        {
//            _extentReport.ExtentReportTearDown();
//        }
//    }
//}

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

