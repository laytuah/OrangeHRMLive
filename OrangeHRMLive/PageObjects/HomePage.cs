using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        PageElement PieChart1 => new PageElement(Driver, By.XPath("(//div[@class='oxd-pie-chart'])[1]"));
        PageElement SidePanel => new PageElement(Driver, By.XPath("//div[@class='oxd-sidepanel-body']"));

        public (bool pieChart, bool sidePanel) AreConfirmationImagesDisplayed() => (PieChart1.IsDisplayed(), SidePanel.IsDisplayed());
    }
}
