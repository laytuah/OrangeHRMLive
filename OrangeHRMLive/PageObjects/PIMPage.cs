using OpenQA.Selenium;
using OrangeHRMLive.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(CustomWebDriver driver): base(driver) { }

        protected IWebElement TextField(string text) => Driver.FindElement(By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));

        protected IWebElement SelectField(int index = 1) => Driver.FindElement(By.XPath($"(//div[@class='oxd-select-text--after'])[{index}]"));

        protected IWebElement SelectGender(string gender) => Driver.FindElement(By.XPath($"//div[@class='--gender-grouped-field']//label[contains(.,\"{gender}\")]"));


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
            SelectGender("Male").Click();
            Button_button("save").Click();
            Button_button("save", 2).Click();
        }
    }
}