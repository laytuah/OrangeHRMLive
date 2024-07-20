using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.Utilities
{
    public class WebDriverSupport
    {
        //private IObjectContainer objectContainer;
        private IWebDriver driver;
        //public WebDriverSupport(IObjectContainer _objectContainer)
        //{
        //    objectContainer = _objectContainer;
        //}

        public void InitializeBrowser(string browserName)
        {
            bool headless = ConfigurationManager.Headless;
            bool incognito = ConfigurationManager.PrivateBrowser;

            Action setupAction = browserName.ToLower() switch
            {
                "edge" => () =>
                {
                    var options = new EdgeOptions();
                    options.SetLoggingPreference(LogType.Performance, LogLevel.All);
                    if (headless) options.AddArgument("headless");
                    if (incognito) options.AddArgument("inprivate");
                    driver = new EdgeDriver();
                }
                ,
                "chrome" => () =>
                {
                    var options = new ChromeOptions();
                    options.SetLoggingPreference(LogType.Performance, LogLevel.All);
                    if (headless) options.AddArgument("headless");
                    if (incognito) options.AddArgument("incognito");
                    driver = new ChromeDriver(options);
                }
                ,
                "firefox" => () =>
                {
                    var options = new FirefoxOptions();
                    if (headless) options.AddArgument("-headless");
                    if (incognito) options.AddArgument("-private");
                    driver = new FirefoxDriver(options);
                }
                ,
                "mobile" => () =>
                {
                    var options = new ChromeOptions();
                    options.EnableMobileEmulation(ConfigurationManager.MobileDeviceName);
                    options.SetLoggingPreference(LogType.Performance, LogLevel.All);
                    if (headless) options.AddArgument("headless");
                    if (incognito) options.AddArgument("incognito");
                    driver = new ChromeDriver(options);
                }
                ,
                _ => throw new Exception("Unknown browser selected")
            };
            setupAction();
            //objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void CloseAUT()
        {
            driver.Quit();
        }
    }
}
