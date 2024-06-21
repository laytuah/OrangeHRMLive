using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager.DriverConfigs.Impl;

namespace OrangeHRMLive.Hooks
{
    public class Context
    {
        public IWebDriver driver;
        public void StartBrowser(string browserName)
        {

            Action setupAction = browserName.ToLower() switch
            {
                "edge" => () => { new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig()); driver = new EdgeDriver(); }
                ,
                "chrome" => () => { new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); driver = new ChromeDriver(); }
                ,
                "firefox" => () => { new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig()); driver = new FirefoxDriver(); }
                ,
                _ => () => { new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); driver = new ChromeDriver(); }
            };

            setupAction();
        }
    }
}
