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
        static ExtentReports _extent;
        [ThreadStatic]
        static ExtentTest _feature;
        [ThreadStatic]
        static ExtentTest _scenario;

        static readonly string ProjectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static readonly string testResultsDirectory = Path.Combine(ProjectDirectory, "TestResults");
        static readonly string ReportPath = Path.Combine(ProjectDirectory, "TestResults", "Reports");
        static readonly string ScreenshotPath = Path.Combine(ProjectDirectory, "TestResults", "Screenshots");
        static readonly string NetworkLogPath = Path.Combine(ProjectDirectory, "TestResults", "NetworkLogs");

        public void ExtentReportInitialization()
        {
            if (!Directory.Exists(testResultsDirectory))
                Directory.CreateDirectory(testResultsDirectory);

            Directory.CreateDirectory(ReportPath);
            Directory.CreateDirectory(ScreenshotPath);
            Directory.CreateDirectory(NetworkLogPath);

            string reportFileName = $"AutomationStatusReport_{ConfigurationManager.BrowserName}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.html";
            string fullReportPath = Path.Combine(ReportPath, reportFileName);

            var htmlReporter = new ExtentHtmlReporter(fullReportPath);
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
                string logFilePath = SaveNetworkLogs(driver, scenarioContext);
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

        ExtentTest CreateStepNode(string stepType, string stepName)
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

        string TakeScreenShot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            string screenshotLocation = Path.Combine(ScreenshotPath, $"{scenarioContext.ScenarioInfo.Title}_{threadId}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.png");
            screenshot.SaveAsFile(screenshotLocation);
            return screenshotLocation;
        }

        string SaveNetworkLogs(IWebDriver driver, ScenarioContext scenarioContext)
        {
            var logs = driver.Manage().Logs.GetLog(LogType.Performance);
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            string logFilePath = Path.Combine(NetworkLogPath, $"{scenarioContext.ScenarioInfo.Title}_{threadId}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log");
            File.WriteAllLines(logFilePath, logs.Select(log => log.ToString()));
            return logFilePath;
        }
    }
}
