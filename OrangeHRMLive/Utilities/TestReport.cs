using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using OrangeHRMLive.Configuration;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.Utilities
{
    public class TestReport
    {
        private static ExtentReports _extent;
        private static ExtentTest _feature;
        private static ExtentTest _scenario;

        private static readonly string ProjectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string ReportPath = ProjectDirectory.Replace("bin\\Debug\\net8.0", "TestResults\\Reports");
        private static readonly string ScreenshotPath = ProjectDirectory.Replace("bin\\Debug\\net8.0", "TestResults\\Screenshots");
        private static readonly string NetworkLogPath = ProjectDirectory.Replace("bin\\Debug\\net8.0", "TestResults\\NetworkLogs");

        public void ExtentReportInitialization()
        {
            var htmlReporter = new ExtentHtmlReporter(ReportPath);
            htmlReporter.Config.ReportName = "Automation Status Report";
            htmlReporter.Config.Theme = Theme.Dark;

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
            _extent.AddSystemInfo("Environment", ConfigurationManager.Url);
            _extent.AddSystemInfo("Browser", ConfigurationManager.BrowserName);
            _extent.AddSystemInfo("Test Engineer", ConfigurationManager.TesterName);
        }

        public void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        public void AfterStep(ScenarioContext scenarioContext, IWebDriver driver)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;

            if (scenarioContext.TestError == null)
            {
                CreateStepNode(stepType, stepName).Pass("Step Passed");
            }
            else
            {
                string failureMessage = scenarioContext.TestError.Message;
                string? stackTrace = scenarioContext.TestError?.StackTrace;
                string logFilePath = SaveNetworkLogs(driver);
                var attachScreenshot = MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build();
                var attachNetworkLog = MediaEntityBuilder.CreateScreenCaptureFromPath(logFilePath).Build();

                CreateStepNode(stepType, stepName)
                    .Fail($"Message: {failureMessage}\nStackTrace: {stackTrace}", attachScreenshot)
                    .Fail("Network Logs", attachNetworkLog);
            }
        }

        public void ExtentReportTearDown()
        {
            _extent.Flush();
        }

        private ExtentTest CreateStepNode(string stepType, string stepName)
        {
            return stepType switch
            {
                "Given" => _scenario.CreateNode<Given>(stepName),
                "When" => _scenario.CreateNode<When>(stepName),
                "Then" => _scenario.CreateNode<Then>(stepName),
                "And" => _scenario.CreateNode<And>(stepName),
                _ => _scenario.CreateNode<And>(stepName)
            };
        }

        private string TakeScreenShot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            string screenshotLocation = Path.Combine(ScreenshotPath, $"{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.png");
            screenshot.SaveAsFile(screenshotLocation);
            return screenshotLocation;
        }

        private string SaveNetworkLogs(IWebDriver driver)
        {
            var logs = driver.Manage().Logs.GetLog(LogType.Performance);
            string logFilePath = Path.Combine(NetworkLogPath, $"NetworkLog_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log");
            File.WriteAllLines(logFilePath, logs.Select(log => log.ToString()));
            return logFilePath;
        }
    }
}
