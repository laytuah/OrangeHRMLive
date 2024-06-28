using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
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
        static void BeforeFeature()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [BeforeStep]
        public void BeforeStep()
        {
            test = extent.CreateTest(ScenarioContext.Current.StepContext.StepInfo.Text);
        }

        [BeforeScenario]
        public void StartBrowser()
        {
            webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
        }

        [AfterScenario]
        public void AfterStep(IWebDriver driver)
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            DateTime time = DateTime.Now;
            string failedTestScreenshotName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            if (status == TestStatus.Failed)
            {
                test.Fail("Test Failed", CaptureScreenShot(failedTestScreenshotName, driver));
                test.Log(Status.Fail, "test failed with logtrace" + stackTrace);
            }
            else if (status == TestStatus.Passed)
            {

            }
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            webDriverSupport.CloseAUT();
        }

        [AfterTestRun]
        public static void AfterTest()
        {
            extent.Flush();
        }

        public MediaEntityModelProvider CaptureScreenShot(string screenShotName, IWebDriver driver)
        {
            ITakesScreenshot takeScreenhot = (ITakesScreenshot)driver;
            string screenshot = takeScreenhot.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
        }
    }
}
