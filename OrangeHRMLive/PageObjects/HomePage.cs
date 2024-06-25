using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }
    }
}
