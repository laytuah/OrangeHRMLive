using OpenQA.Selenium;

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
            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/");
        }

        public void ScrollToElement(IWebElement locator) 
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", locator);
        }
    }
}
