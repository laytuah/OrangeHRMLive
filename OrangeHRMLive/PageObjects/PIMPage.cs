using OpenQA.Selenium;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(CustomWebDriver driver): base(driver) { }

        protected IWebElement TextField(string text) => Driver.FindElement(By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));

        protected IWebElement SelectField(int index = 1) => Driver.FindElement(By.XPath($"(//div[@class='oxd-select-text--after'])[{index}]"));
        

        public void RegisterNewEmployee()
        {
            Mainmenu_item("pim").Click();
            Button_button("add").Click();
            TextField("first name").SendKeys(DataGenerator.GenerateRandomString());
            TextField("middle name").SendKeys(DataGenerator.GenerateRandomString());
            TextField("last name").SendKeys(DataGenerator.GenerateRandomString());
            Button_button("save").Click();
            TextField("driver's license number").SendKeys(DataGenerator.GenerateRandomAlphanumerics());
            SelectField().Click();
            Select_dropdown("nigerian").Click();
            SelectField(2).Click();
            Select_dropdown("single").Click();
            SelectField(3).Click();
            Select_dropdown("o+").Click();
        }
    }
}