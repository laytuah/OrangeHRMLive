using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }

        protected IWebElement PieChart1 => driver.FindElement(By.XPath("(//div[@class='oxd-pie-chart'])[1]"));
        protected IWebElement PieChart2 => driver.FindElement(By.XPath("(//div[@class='oxd-pie-chart'])[2]"));

        public bool GetHomePageConfirmation1()
        {
            return PieChart1.Displayed;
        }

        public bool GetHomePageConfirmation2()
        {
            return PieChart2.Displayed;
        }
    }
}
