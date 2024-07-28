

using OpenQA.Selenium;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(IWebDriver driver): base(driver) { }

        public void RegisterNewEmployee()
        {
            Mainmenu_item("PIM").Click();
            Button("add").Click();
        }
    }
}
