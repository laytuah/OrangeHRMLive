using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Utilities
{
    public class ExtentReport
    {
        static ExtentReports extent;
        static ExtentTest _feature;
        static ExtentTest _scenario;

        public static string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string resultPath = projectDirectory.Replace("bin\\Debug\\net8.0", "TestResults");

        public void ExtentReportInitialization()
        {
            //Console.WriteLine("Running before test run...");
            
            var htmlReporter = new ExtentHtmlReporter(resultPath); 
            htmlReporter.Config.ReportName = "Automation Status report";
            htmlReporter.Config.DocumentTitle = "Automation Status";
            htmlReporter.Config.Theme = Theme.Dark;
            //htmlReporter.Start();

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Environment", "Staging");
            extent.AddSystemInfo("Test Engineer", "Abdlateef");
        }

        public void BeforeFeature(FeatureContext featureContext)
        {
            //test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            _feature = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        //public void AfterScenario(IWebDriver driver)
        //{
        //    var status = TestContext.CurrentContext.Result.Outcome.Status;
        //    var stackTrace = TestContext.CurrentContext.Result.StackTrace;
        //    DateTime time = DateTime.Now;
        //    string failedTestScreenshotName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
        //    if (status == TestStatus.Failed)
        //    {
        //        test.Fail("Test Failed", CaptureScreenShot(failedTestScreenshotName, driver));
        //        test.Log(Status.Fail, "test failed with logtrace" + stackTrace);
        //    }
        //}

        public void AfterStep(ScenarioContext scenarioContext, IWebDriver driver)
        {
            //Console.WriteLine("Running after step...");
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    _scenario.CreateNode<Given>(stepName);
                else if (stepType == "When")
                    _scenario.CreateNode<When>(stepName);
                else if (stepType == "Then")
                    _scenario.CreateNode<Then>(stepName);
                else if (stepType == "And")
                    _scenario.CreateNode<And>(stepName);
            }
            else if (scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                    _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                else if (stepType == "When")
                    _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                else if (stepType == "Then")
                    _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                else if (stepType == "And")
                    _scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());

            }
        }

        public void ExtentReportTearDown()
        {
            extent.Flush();
        }

        public string AddScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot takeScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takeScreenshot.GetScreenshot();
            string screenShotLocation = Path.Combine(resultPath, scenarioContext.ScenarioInfo.Title + DateTime.Now.ToString("_h_mm_ss") + ".png");
            screenshot.SaveAsFile(screenShotLocation);
            return screenShotLocation;

        }

        //public MediaEntityModelProvider CaptureScreenShot(string screenShotName, IWebDriver driver)
        //{
        //    ITakesScreenshot takeScreenhot = (ITakesScreenshot)driver;
        //    string screenshot = takeScreenhot.GetScreenshot().AsBase64EncodedString;
        //    return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
        //}
    }
}
