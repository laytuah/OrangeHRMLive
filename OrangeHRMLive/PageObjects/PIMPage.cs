using OpenQA.Selenium;
using OrangeHRMLive.Model;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(CustomWebDriver driver) : base(driver) { }

        protected IWebElement TextField(string text) => Driver.FindElement(By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));

        protected IWebElement SelectField(int index = 1) => Driver.FindElement(By.XPath($"(//div[@class='oxd-select-text--after'])[{index}]"));

        protected IWebElement SelectGender(string gender) => Driver.FindElement(By.XPath($"//div[@class='--gender-grouped-field']//label[contains(.,\"{gender}\")]"));

        protected IWebElement NewlyRegisteredEmployee(string? ID, string? firstName, string? lastName) => Driver.FindElement(By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\") and contains(.,\"{firstName}\") and contains(.,\"{lastName}\")]"));

        protected IWebElement NewlyRegisteredEmployeeUpdateIcon(string? ID) => Driver.FindElement(By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\")]//i[@class='oxd-icon bi-pencil-fill']"));

        protected IWebElement Pagination_Next => Driver.FindElement(By.XPath("//i[@class='oxd-icon bi-chevron-right']"));




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
            if (IsEmployeeDisplayedOnCurrentPage(employee))
                return true;
            while (Pagination_Next.Displayed)
            {
                Pagination_Next.Click();
                if (IsEmployeeDisplayedOnCurrentPage(employee))
                    return true;
            }
            return false;
        }

        bool IsEmployeeDisplayedOnCurrentPage(EmployeeProfile employee)
        {
            try
            {
                return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void UpdateExistingEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            if (IsEmployeeDisplayedOnCurrentPage(employee))
            {
                NewlyRegisteredEmployeeUpdateIcon(employee.EmployeeID).Click();
            }
            else
            {
                while (Pagination_Next.Displayed)
                {
                    Pagination_Next.Click();
                    if (IsEmployeeDisplayedOnCurrentPage(employee))
                    {
                        NewlyRegisteredEmployeeUpdateIcon(employee.EmployeeID).Click();
                        break;
                    }
                }
            }

            Link_anchor("job").Click();
            SelectField().Click();
            Select_dropdown(employee.JobTitle).Click();
            SelectField(5).Click();
            Select_dropdown(employee.EmploymentStatus).Click();
            Button_button("save").Click();
        }

        public string GetUpdatedEmployeeText(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            if (IsEmployeeDisplayedOnCurrentPage(employee))
            {
                return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).Text.ToLower();
            }
            else
            {
                while (Pagination_Next.Displayed)
                {
                    Pagination_Next.Click();
                    if (IsEmployeeDisplayedOnCurrentPage(employee))
                    {
                        return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).Text.ToLower();
                    }
                }
                return "Employee not found or record not updated.";
            }
        }
    }
}