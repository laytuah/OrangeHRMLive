using OpenQA.Selenium;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.PageObjects
{
    public class BasePage
    {
        protected IWebDriver driver;
        public BasePage(IWebDriver _driver)
        {
            driver = _driver;
        }
        public void LoadAUT()
        {
            driver.Navigate().GoToUrl(ConfigurationManager.Url);
        }

        public void ScrollToElement(IWebElement locator)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", locator);
        }
    }
}
