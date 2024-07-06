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
            Console.WriteLine("Running before test run...");
            
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
            Console.WriteLine("Running after step...");
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName);
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepName);
            }
            else if (scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build());
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build());
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build());
                    
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message, 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShot(driver, scenarioContext)).Build());
            }
        }

        //public void PrintTestLog(ScenarioContext scenarioContext)
        //{
        //    Console.WriteLine("Running after scenario...");
        //    if (scenarioContext.TestError != null)
        //    {
        //        scenario.Fail("Scenario failed with the following error: " + scenarioContext.TestError.Message);
        //    }
        //    else
        //    {
        //        scenario.Pass("Scenario executed successfully.");
        //    }
        //}

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