using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Configuration
{
    [Binding]
    internal class Hooks
    {
        private readonly WebDriverSupport webDriverSupport;
        private static ExtentReports extent;
        private static ExtentTest test;
        public Hooks(WebDriverSupport _webDriverSupport)
        {
            webDriverSupport = _webDriverSupport;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string reportPath = projectDirectory + "//report.html";
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Environment", "Staging");
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [BeforeStep]
        public static void BeforeStep()
        {
            test = extent.CreateTest(ScenarioContext.Current.ScenarioInfo.Title);
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

        [AfterTestRun]
        public static void AfterTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            DateTime time = DateTime.Now;
            string failedTestScreenshotName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            if (status == TestStatus.Failed)
            {
                test.Fail("Test Failed", WebDriverSupport.CaptureScreenShot(failedTestScreenshotName));
                test.Log(Status.Fail, "test failed with logtrace" + stackTrace);
            }
            else if (status == TestStatus.Passed)
            {

            }

            extent.Flush();
        }
    }
}
