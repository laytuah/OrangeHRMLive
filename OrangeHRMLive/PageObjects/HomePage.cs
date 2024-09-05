using OpenQA.Selenium;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        UIElement PieChart1 => new UIElement(Driver, By.XPath("(//div[@class='oxd-pie-chart'])[1]"));
        UIElement SidePanel => new UIElement(Driver, By.XPath("//div[@class='oxd-sidepanel-body']"));

        PageElement siPanell => new PageElement(Driver, By.XPath("//div[@class='oxd-sidepanel-body']"));

        public bool IsPieChartDispalyed() => PieChart1.IsDisplayed();

        public bool IsSidePanelDisplayed() => SidePanel.IsDisplayed();
    }
}
