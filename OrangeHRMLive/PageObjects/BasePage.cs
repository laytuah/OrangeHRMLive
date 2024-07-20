using OpenQA.Selenium;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.PageObjects
{
    public class BasePage
    {
        protected IWebDriver Driver { get; }

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void LoadAUT()
        {
            Driver.Navigate().GoToUrl(ConfigurationManager.Url);
        }

        public void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
    }
}
