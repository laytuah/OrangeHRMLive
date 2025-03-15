using OrangeHRMLive.Configuration;
using OrangeHRMLive.Utilities;
using Reqnroll;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace OrangeHRMLive.Hooks
{
    [Binding]
    internal class Hooks
    {
        readonly WebDriverSupport _webDriverSupport;
        readonly TestReport _testReport;
        static TestReport _staticTestReport;

        public Hooks(WebDriverSupport webDriverSupport, TestReport testReport)
        {
            _webDriverSupport = webDriverSupport;
            _testReport = testReport;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _staticTestReport = new TestReport();
            _staticTestReport.ExtentReportInitialization();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _staticTestReport.BeforeFeature(featureContext);
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
            _testReport.BeforeScenario(scenarioContext);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            _testReport.AfterStep(scenarioContext, _webDriverSupport.GetDriver());
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _webDriverSupport.CloseAUT();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _staticTestReport.ExtentReportTearDown();
        }
    }
}
