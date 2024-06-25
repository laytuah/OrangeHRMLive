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
        protected IWebElement UserFullName_label => driver.FindElement(By.XPath("//p[@class='oxd-userdropdown-name']"));
        

        public bool GetHomePageConfirmation()
        {
            return PieChart1.Displayed;
        }

        public string GetUserFullName()
        {
            return UserFullName_label.Text.Trim();
        }
    }
}
