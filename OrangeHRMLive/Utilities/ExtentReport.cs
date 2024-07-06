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
        static ExtentTest feature;
        static ExtentTest scenario;

        public static string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string resultPath = projectDirectory.Replace("bin\\Debug\\net8.0", "TestResults");

        public void ExtentReportInitialization()
        {
            var htmlReporter = new ExtentHtmlReporter(resultPath); 
            htmlReporter.Config.ReportName = "Automation Status report";
            htmlReporter.Config.DocumentTitle = "Automation Status";
            htmlReporter.Config.Theme = Theme.Dark;

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Environment", "Staging");
            extent.AddSystemInfo("Test Engineer", "Abdlateef");
        }

        public void BeforeFeature(FeatureContext featureContext)
        {
            feature = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            scenario = feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        public void AfterStep(ScenarioContext scenarioContext, IWebDriver driver)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            string? failureMesage = scenarioContext.TestError?.Message;
            string? stackTrace = scenarioContext.TestError?.StackTrace;
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName).Pass("Step Passed");
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName).Pass("Step Passed");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName).Pass("Step Passed");
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepName).Pass("Step Passed");
            }
            else if (scenarioContext.TestError != null)
            {
                var attachScreenshotMedia = MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build();
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia);
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia);
            }
        }

        public void ExtentReportTearDown()
        {
            extent.Flush();
        }

        public string TakeScreenShot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot takeScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takeScreenshot.GetScreenshot();
            string screenShotLocation = Path.Combine(resultPath, scenarioContext.ScenarioInfo.Title + DateTime.Now.ToString("_h_mm_ss") + ".png");
            screenshot.SaveAsFile(screenShotLocation);
            return screenShotLocation;
        }
    }
}