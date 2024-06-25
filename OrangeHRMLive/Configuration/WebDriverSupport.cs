using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager.DriverConfigs.Impl;

namespace OrangeHRMLive.Configuration
{
    public class WebDriverSupport
    {
        private readonly IObjectContainer objectContainer;
        private IWebDriver driver;
        public WebDriverSupport(IObjectContainer _objectContainer)
        {
            objectContainer = _objectContainer;
        }

        public void InitializeBrowser(string browserName)
        {

            Action setupAction = browserName.ToLower() switch
            {
                "edge" => () => { new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig()); driver = new EdgeDriver(); }
                ,
                "chrome" => () => { new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); driver = new ChromeDriver(); }
                ,
                "firefox" => () => { new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig()); driver = new FirefoxDriver(); }
                ,
                _ => throw new Exception("Unknown browser selected")
            };
            setupAction();
            objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void CloseAUT()
        {
            driver.Quit();
        }
    }
}
