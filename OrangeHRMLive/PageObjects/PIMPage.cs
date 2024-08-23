using OpenQA.Selenium;
using OrangeHRMLive.Model;
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

        protected IWebElement NewlyRegisteredEmployee(string? ID, string? firstName, string? lastName) => Driver.FindElement(By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\") and contains(.,\"{firstName}\") and contains(.,\"{lastName}\")]"));
        


        public void RegisterNewEmployee(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            Button_button("add").Click();
            TextField("first name").SendKeys(employee.Firstname);
            TextField("middle name").SendKeys(employee.Middlename);
            TextField("last name").SendKeys(employee.Lastname);
            employee.EmployeeID = TextField("employee id").GetAttribute("value");
            Button_button("save").Click();
            TextField("driver's license number").SendKeys(employee.DriversLicenseNumber);
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

        public bool IsNewlyRegisteredEmployeeDisplayed(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).Displayed;
        }
    }
}