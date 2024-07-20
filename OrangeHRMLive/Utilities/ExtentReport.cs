using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using OrangeHRMLive.Configuration;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Utilities
{
    public class ExtentReport
    {
        static ExtentReports extent;
        static ExtentTest feature;
        static ExtentTest scenario;

        public static string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string reportPath = projectDirectory.Replace("bin\\Debug\\net8.0", "TestResults\\Reports");
        public static string screenshotPath = projectDirectory.Replace("bin\\Debug\\net8.0", "TestResults\\Screenshots");
        public static string networkLogPath = projectDirectory.Replace("bin\\Debug\\net8.0", "TestResults\\NetworkLogs");

        public void ExtentReportInitialization()
        {
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            htmlReporter.Config.ReportName = "Automation Status report";
            htmlReporter.Config.Theme = Theme.Dark;

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Environment", ConfigurationManager.Url);
            extent.AddSystemInfo("Browser", ConfigurationManager.BrowserName);
            extent.AddSystemInfo("Test Engineer", ConfigurationManager.TesterName);
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
                string failureMesage = scenarioContext.TestError.Message;
                string? stackTrace = scenarioContext.TestError?.StackTrace;
                var logs = driver.Manage().Logs.GetLog(LogType.Performance);
                string logFilePath = Path.Combine(networkLogPath, $"NetworkLog_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log");
                File.WriteAllLines(logFilePath, logs.Select(log => log.ToString()));
                var attachScreenshotMedia = MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build();
                var attachNetworkLogMedia = MediaEntityBuilder.CreateScreenCaptureFromPath(logFilePath).Build();

                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia)
                        .Fail("Network Logs", attachNetworkLogMedia);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia)
                        .Fail("Network Logs", attachNetworkLogMedia);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia)
                        .Fail("Network Logs", attachNetworkLogMedia);
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepName).Fail($"Message: \n {failureMesage} \n StackTrace: \n {stackTrace}", attachScreenshotMedia)
                        .Fail("Network Logs", attachNetworkLogMedia);
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
            string screenShotLocation = Path.Combine(screenshotPath, scenarioContext.ScenarioInfo.Title + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss") + ".png");
            screenshot.SaveAsFile(screenShotLocation);
            return screenShotLocation;
        }
    }
}