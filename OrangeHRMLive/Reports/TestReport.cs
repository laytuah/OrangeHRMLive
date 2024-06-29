using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Reports
{
    public class TestReport
    {
        static ExtentReports extent;
        ExtentTest test;
        public void ReportSetup()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string reportPath = projectDirectory + "//report.html";
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Environment", "Staging");
        }

        public void BeforeFeature()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        public void BeforeStep(ScenarioContext scenarioContext)
        {
            test = extent.CreateTest(scenarioContext.StepContext.StepInfo.Text);
        }

        public void AfterScenario(IWebDriver driver)
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
        }

        public void AfterTestRun()
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
