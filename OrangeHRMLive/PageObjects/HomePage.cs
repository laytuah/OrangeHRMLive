using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }

        #region Locators
        protected IWebElement PieChart1 => driver.FindElement(By.XPath("(//div[@class='oxd-pie-chart'])[1]"));
        protected IWebElement UserFullName_label => driver.FindElement(By.XPath("//p[@class='oxd-userdropdown-name']"));
        protected IWebElement SidePanel => driver.FindElement(By.XPath("//div[@class='oxd-sidepanel-body']"));

        #endregion

        #region Actions
        public bool GetHomePageConfirmation()
        {
            return PieChart1.Displayed;
        }

        public string GetUserFullName()
        {
            return UserFullName_label.Text.Trim();
        }

        public bool IsSidePanelDisplayed()
        {
            return SidePanel.Displayed;
        }

        #endregion
    }
}
