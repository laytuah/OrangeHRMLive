//using AventStack.ExtentReports.Reporter;
//using AventStack.ExtentReports;
//using NUnit.Framework.Interfaces;
//using OrangeHRMLive.Configuration;

//namespace OrangeHRMLive.Reports
//{
//    internal class TestReport
//    {
//        private readonly WebDriverSupport webDriverSupport;
//        private static ExtentReports extent;
//        ExtentTest test;

//        public TestReport(WebDriverSupport _webDriverSupport)
//        {
//            webDriverSupport = _webDriverSupport;
//        }

//        public static void ReportSetup()
//        {
//            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
//            string reportPath = projectDirectory + "//report.html";
//            var htmlReporter = new ExtentHtmlReporter(reportPath);
//            extent = new ExtentReports();
//            extent.AttachReporter(htmlReporter);
//            extent.AddSystemInfo("Environment", "Staging");
//        }

//        public void SetUp()
//        {
//            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
//        }

//        public void AfterTest()
//        {
//            var status = TestContext.CurrentContext.Result.Outcome.Status;
//            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
//            DateTime time = DateTime.Now;
//            string failedTestScreenshotName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
//            if (status == TestStatus.Failed)
//            {
//                test.Fail("Test Failed", webDriverSupport.CaptureScreenShot(failedTestScreenshotName));
//                test.Log(Status.Fail, "test failed with logtrace" + stackTrace);
//            }
//            else if (status == TestStatus.Passed)
//            {

//            }

//            extent.Flush();
//        }
//    }
//}
