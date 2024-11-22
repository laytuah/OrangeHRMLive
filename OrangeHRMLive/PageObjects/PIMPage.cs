using OpenQA.Selenium;
using OrangeHRMLive.Model;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(IWebDriver driver) : base(driver) { }

        protected PageElement TextField(string text) => new PageElement(Driver, By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));
        protected PageElement SelectGender(string gender) => new PageElement(Driver, By.XPath($"//div[@class='--gender-grouped-field']//label[contains(.,\"{gender}\")]"));
        protected PageElement NewlyRegisteredEmployee(string? ID, string? firstName, string? lastName) => new PageElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\") and contains(.,\"{firstName}\") and contains(.,\"{lastName}\")]"));
        protected PageElement Pagination_Next => new PageElement(Driver, By.XPath("//i[@class='oxd-icon bi-chevron-right']"));
        protected PageElement FirstEmployeeOnList => new PageElement(Driver, By.XPath("//div[@class='oxd-table-card'  and not(contains(.,'Human Resources'))][position()=1]"));
        protected PageElement GetCertainTextFromFirstEmployeeOnPIMList(int textIndex) => new PageElement(Driver, By.XPath($"(//div[@class='oxd-table-card'  and not(contains(.,'Human Resources'))][position()=1]//div[text()])[{textIndex}]"));
        protected PageElement DeleteIconForFirstEmployee => new PageElement(Driver, By.XPath("//div[@class='oxd-table-card'  and not(contains(.,'Human Resources'))][position()=1]//i[@class='oxd-icon bi-trash']"));
        protected PageElement Delete_button => new PageElement(Driver, By.XPath("//i[@class='oxd-icon bi-trash oxd-button-icon']"));

        public void RegisterNewEmployee(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            Button_button("add").Click();
            TextField("first name").ClearAndSendKeys(employee.Firstname);
            TextField("middle name").ClearAndSendKeys(employee.Middlename);
            TextField("last name").ClearAndSendKeys(employee.Lastname);
            TextField("employee id").ClearAndSendKeys(employee.EmployeeID);
            Button_button("save").Click();
            Select_Dropdown("nationality").Click();
            SelectFromDropdownList(employee.Nationality).Click();

            Select_Dropdown("marital status").Click();
            SelectFromDropdownList(employee.MaritalStatus).Click();
            Select_Dropdown("blood type").Click();
            SelectFromDropdownList(employee.BloodGroup).Click();
            SelectGender(employee.Gender).Click();
            Button_button("save").Click();
            Button_button("save", 2).Click();
        }

        public bool IsNewlyRegisteredEmployeeDisplayed(EmployeeProfile employee)
        {
            Mainmenu_item("pim").ActionClick();
            if (IsEmployeeDisplayedOnCurrentPage(employee))
                return true;
            while (IsNextPageChevronDisplayed())
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
                return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).IsDisplayed();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        bool IsNextPageChevronDisplayed()
        {
            try
            {
                return Pagination_Next.IsDisplayed();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void UpdateFirstEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").ActionClick();
            FirstEmployeeOnList.ActionClick();
            Link_anchor("job").Click();
            Select_Dropdown("job title").Click();
            SelectFromDropdownList(employee.JobTitle).Click();
            Button_button("save").Click();
        }

        public void UpdateMyInfo(EmployeeProfile employee)
        {
            Mainmenu_item("my info").ActionClick();
            if(!Pagination_Next.IsDisplayed())
                TextField("last name").ClearAndSendKeys(employee.Lastname);
            Button_button("save").Click();
        }

        public string GetAllFirstEmployeeText()
        {
            Mainmenu_item("pim").ActionClick();
            return FirstEmployeeOnList.Text.ToLower();
        }

        public void DeleteFirstEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").ActionClick();
            employee.Firstname = GetCertainTextFromFirstEmployeeOnPIMList(6).Text.Trim();
            employee.Lastname = GetCertainTextFromFirstEmployeeOnPIMList(8).Text.Trim();
            while (GetCertainTextFromFirstEmployeeOnPIMList(6).Text.Trim() == employee.Firstname
                    && GetCertainTextFromFirstEmployeeOnPIMList(8).Text.Trim() == employee.Lastname)
            {
                DeleteIconForFirstEmployee.ActionClick();
                Delete_button.Click();
            }
        }

        public string GetAllFirstAdminText()
        {
            Mainmenu_item("admin").ActionClick();
            return FirstEmployeeOnList.Text.ToLower();
        }

        public (string FirstName, string LastName) GetFirstAndLastNameOfFirstEmployeeOnList()
        {
            string firstName = GetCertainTextFromFirstEmployeeOnPIMList(6).Text.Trim();
            string lastName = GetCertainTextFromFirstEmployeeOnPIMList(8).Text.Trim();
            return (firstName, lastName);
        }
    }
}