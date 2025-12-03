using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using OrangeHRMLive.Configuration;
using Reqnroll;

namespace OrangeHRMLive.Utilities
{
    public class TestReport
    {
        private static ExtentReports? _extent;

        // Keep one Feature node per feature file (thread-safe)
        private static readonly ConcurrentDictionary<string, ExtentTest> _featureNodes = new();

        // Scenario node flows across awaits
        private static readonly AsyncLocal<ExtentTest?> _scenario = new();

        private static readonly string ProjectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string TestResultsDirectory = Path.Combine(ProjectDirectory, "TestResults");
        private static readonly string ReportPath = Path.Combine(ProjectDirectory, "TestResults", "Reports");
        private static readonly string ScreenshotPath = Path.Combine(ProjectDirectory, "TestResults", "Screenshots");
        private static readonly string NetworkLogPath = Path.Combine(ProjectDirectory, "TestResults", "NetworkLogs");

        public void ExtentReportInitialization()
        {
            if (!Directory.Exists(TestResultsDirectory)) Directory.CreateDirectory(TestResultsDirectory);
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
            var title = featureContext.FeatureInfo.Title;
            // Create once; reuse for all scenarios in this feature
            _featureNodes.TryAdd(title, _extent!.CreateTest<Feature>(title));
        }

        // Accept FeatureContext so we can resolve the correct parent consistently
        public void BeforeScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            var featureTitle = featureContext.FeatureInfo.Title;

            if (!_featureNodes.TryGetValue(featureTitle, out var featureNode))
            {
                featureNode = _extent!.CreateTest<Feature>(featureTitle);
                _featureNodes[featureTitle] = featureNode;
        }

            _scenario.Value = featureNode.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        /// <summary>
        /// One step logger for UI & API.
        /// UI (driver != null): attach screenshot + network logs on failure.
        /// API (driver == null): pass/fail only.
        /// </summary>
        public void AfterStepGeneric(ScenarioContext scenarioContext, IWebDriver? driver = null)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;

            var node = CreateStepNode(stepType, stepName);

            if (scenarioContext.TestError == null)
            {
                node.Pass("Step Passed");
                return;
            }

                string failureMessage = scenarioContext.TestError.Message;
                string? stackTrace = scenarioContext.TestError?.StackTrace;

            if (driver != null)
            {
                string logFilePath = SaveNetworkLogs(driver, scenarioContext);
                var attachScreenshot = MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build();
                var attachNetworkLog = MediaEntityBuilder.CreateScreenCaptureFromPath(logFilePath).Build();

                node.Fail($"Message: {failureMessage}\nStackTrace: {stackTrace}", attachScreenshot)
                    .Fail("Network Logs", attachNetworkLog);
            }
            else
            {
                node.Fail($"Message: {failureMessage}\nStackTrace: {stackTrace}");
            }
        }

        public void ExtentReportTearDown()
        {
            _extent!.Flush();
        }

        private ExtentTest CreateStepNode(string stepType, string stepName)
        {
            return stepType switch
            {
                "Given" => _scenario.Value!.CreateNode<Given>(stepName),
                "When" => _scenario.Value!.CreateNode<When>(stepName),
                "Then" => _scenario.Value!.CreateNode<Then>(stepName),
                "And" => _scenario.Value!.CreateNode<And>(stepName),
                _ => _scenario.Value!.CreateNode<And>(stepName)
            };
        }

        private string TakeScreenShot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot ss = (ITakesScreenshot)driver;
            var screenshot = ss.GetScreenshot();
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            string file = Path.Combine(ScreenshotPath, $"{scenarioContext.ScenarioInfo.Title}_{threadId}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.png");
            screenshot.SaveAsFile(file);
            return file;
        }

        private string SaveNetworkLogs(IWebDriver driver, ScenarioContext scenarioContext)
        {
            var logs = driver.Manage().Logs.GetLog(LogType.Performance);
            string id = Thread.CurrentThread.ManagedThreadId.ToString();
            string file = Path.Combine(NetworkLogPath, $"{scenarioContext.ScenarioInfo.Title}_{id}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log");
            File.WriteAllLines(file, logs.Select(l => l.ToString()));
            return file;
        }
    }
}
