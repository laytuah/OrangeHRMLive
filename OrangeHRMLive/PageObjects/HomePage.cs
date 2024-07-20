using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        private IWebElement PieChart1 => Driver.FindElement(By.XPath("(//div[@class='oxd-pie-chart'])[1]"));
        private IWebElement SidePanel => Driver.FindElement(By.XPath("//div[@class='oxd-sidepanel-body']"));

        public bool IsPieChartDispalyed() => PieChart1.Displayed;

        public bool IsSidePanelDisplayed() => SidePanel.Displayed;
    }
}
