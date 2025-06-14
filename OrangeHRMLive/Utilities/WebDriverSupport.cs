using Reqnroll.BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.Utilities
{
    public class WebDriverSupport
    {
        readonly IObjectContainer _objectContainer;
        IWebDriver? _driver;

        public WebDriverSupport(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        public void InitializeBrowser(string browserName)
        {
            bool headless = ConfigurationManager.Headless;
            bool incognito = ConfigurationManager.PrivateBrowser;

            Action setupAction = browserName.ToLower() switch
            {
                "edge" => () => _driver = SetupEdgeDriver(headless, incognito),
                "chrome" => () => _driver = SetupChromeDriver(headless, incognito),
                "firefox" => () => _driver = SetupFirefoxDriver(headless, incognito),
                "mobile" => () => _driver = SetupMobileDriver(headless, incognito),
                _ => throw new ArgumentException($"Unknown browser: {browserName}")
            };

            setupAction.Invoke();
            _objectContainer.RegisterInstanceAs(_driver!);
            if (!headless)
                _driver!.Manage().Window.Maximize();
            _driver!.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void CloseAUT()
        {
            _driver!.Quit();
        }

        public IWebDriver GetDriver()
        {
            return _driver!;
        }

        IWebDriver SetupEdgeDriver(bool headless, bool incognito)
        {
            var options = new EdgeOptions();
            options.SetLoggingPreference(LogType.Performance, LogLevel.All);
            if (headless)
            {
                options.AddArgument("headless=new");
                options.AddArgument("window-size=1920,1080");
            }
            if (incognito) options.AddArgument("inprivate");
            return new EdgeDriver(options);
        }

        IWebDriver SetupChromeDriver(bool headless, bool incognito)
        {
            var options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Performance, LogLevel.All);
            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080"); 
            }
            if (incognito) options.AddArgument("incognito");
            return new ChromeDriver(options);
        }

        IWebDriver SetupFirefoxDriver(bool headless, bool incognito)
        {
            var options = new FirefoxOptions();
            if (headless)
            {
                options.AddArgument("-headless");
                options.AddArgument("--width=1920");
                options.AddArgument("--height=1080");
            }
            if (incognito) options.AddArgument("-private");
            return new FirefoxDriver(options);
        }

        IWebDriver SetupMobileDriver(bool headless, bool incognito)
        {
            var options = new ChromeOptions();
            options.EnableMobileEmulation(ConfigurationManager.MobileDeviceName);
            options.SetLoggingPreference(LogType.Performance, LogLevel.All);
            if (headless) options.AddArgument("headless");
            if (incognito) options.AddArgument("incognito");
            return new ChromeDriver(options);
        }
    }
}
