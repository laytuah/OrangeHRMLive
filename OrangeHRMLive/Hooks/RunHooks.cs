using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRMLive.Utilities;
using Reqnroll;

[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace OrangeHRMLive.Hooks
{
    [Binding]
    internal class RunHooks
    {
        private readonly TestReport _testReport;
        private static TestReport? _staticTestReport;

        public RunHooks(TestReport testReport) => _testReport = testReport;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _staticTestReport = new TestReport();
            _staticTestReport.ExtentReportInitialization();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _staticTestReport!.BeforeFeature(featureContext);
        }

        // Pass both contexts so we can reliably find the feature
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _testReport.BeforeScenario(scenarioContext, featureContext);
        }

        // Centralised step logging for BOTH UI and API
        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            IWebDriver? driver = null;
            try { driver = scenarioContext.ScenarioContainer.Resolve<IWebDriver>(); }
            catch { /* API: no driver registered */ }

            _testReport.AfterStepGeneric(scenarioContext, driver);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _staticTestReport!.ExtentReportTearDown();
        }
    }
}
