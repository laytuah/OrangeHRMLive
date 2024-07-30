using OpenQA.Selenium;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(IWebDriver driver): base(driver) { }

        protected IWebElement TextField(string placeholder) => Driver.FindElement(By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))='{placeholder}']"));

        public void RegisterNewEmployee()
        {
            Mainmenu_item("PIM").Click();
            Button("add").Click();
            TextField("first name").SendKeys(DataGenerator.GenerateRandomString());
            TextField("middle name").SendKeys(DataGenerator.GenerateRandomString());
            TextField("last name").SendKeys(DataGenerator.GenerateRandomString());
            Button("save").Click();
        }
    }
}
