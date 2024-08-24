using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OrangeHRMLive.Configuration;
using System.Collections.ObjectModel;

namespace OrangeHRMLive.Utilities
{
    public class WebDriverSupport
    {
        readonly IObjectContainer _objectContainer;
        IWebDriver _driver;

        public WebDriverSupport(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }
        public void Dispose() => _driver.Dispose();
        public void Close() => _driver.Close();
        public void Quit() => _driver.Quit();
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public IWebElement FindElement(By by) => _driver.FindElement(by);
        public IWindow Window => _driver.Manage().Window;
        public IOptions Manage() => _driver.Manage();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
        public IJavaScriptExecutor JavaScriptExecutor => (IJavaScriptExecutor)_driver;
        public ICookieJar Cookies => _driver.Manage().Cookies;
        public INavigation Navigate() => _driver.Navigate();
        public string PageSource => _driver.PageSource;
        public string Title => _driver.Title;
        public string Url
        {
            get => _driver.Url;
            set => _driver.Url = value;
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
            _objectContainer.RegisterInstanceAs(_driver);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void CloseAUT()
        {
            _driver.Quit();
        }

        public IWebDriver GetDriver()
        {
            return _driver;
        }

        IWebDriver SetupEdgeDriver(bool headless, bool incognito)
        {
            var options = new EdgeOptions();
            options.SetLoggingPreference(LogType.Performance, LogLevel.All);
            if (headless) options.AddArgument("headless");
            if (incognito) options.AddArgument("inprivate");
            return new EdgeDriver(options);
        }

        IWebDriver SetupChromeDriver(bool headless, bool incognito)
        {
            var options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Performance, LogLevel.All);
            if (headless) options.AddArgument("headless");
            if (incognito) options.AddArgument("incognito");
            return new ChromeDriver(options);
        }

        IWebDriver SetupFirefoxDriver(bool headless, bool incognito)
        {
            var options = new FirefoxOptions();
            if (headless) options.AddArgument("-headless");
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
        //public void Click(By by)
        //{
        //    IWebElement element = _driver.FindElement(by);
        //    element.Click();
        //    WaitForLoadingIconToDisappear();
        //}
        //public void SendKeys(By by, string text)
        //{
        //    IWebElement element = _driver.FindElement(by);
        //    element.SendKeys(text);
        //    WaitForLoadingIconToDisappear();
        //}
        //void WaitForLoadingIconToDisappear()
        //{
        //    if (!string.IsNullOrEmpty(ConfigurationManager.LoadingIconXpath))
        //    {
        //        var loadingElements = _driver.FindElements(By.XPath(ConfigurationManager.LoadingIconXpath));
        //        if (loadingElements.Count > 0 && loadingElements[0].Displayed)
        //        {
        //            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        //            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(ConfigurationManager.LoadingIconXpath)));
        //        }
        //    }
        //}
    }
}
