using OpenQA.Selenium;
using OrangeHRMLive.Model;
using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(IWebDriver driver) : base(driver) { }

        protected UIElement TextField(string text) => new UIElement(Driver, By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));

        protected UIElement SelectField(int index = 1) => new UIElement(Driver, By.XPath($"(//div[@class='oxd-select-text--after'])[{index}]"));

        protected UIElement SelectGender(string gender) => new UIElement(Driver, By.XPath($"//div[@class='--gender-grouped-field']//label[contains(.,\"{gender}\")]"));

        protected UIElement NewlyRegisteredEmployee(string? ID, string? firstName, string? lastName) => new UIElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\") and contains(.,\"{firstName}\") and contains(.,\"{lastName}\")]"));

        protected UIElement NewlyRegisteredEmployeeUpdateIcon(string? ID) => new UIElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\")]//i[@class='oxd-icon bi-pencil-fill']"));

        protected UIElement NewlyRegisteredEmployeeDeleteIcon(string? ID) => new UIElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\")]//i[@class='oxd-icon bi-trash']"));

        protected UIElement Pagination_Next => new UIElement(Driver, By.XPath("//i[@class='oxd-icon bi-chevron-right']"));




        public void RegisterNewEmployee(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            Button_button("add").Click();
            TextField("first name").EnterText(employee.Firstname);
            TextField("middle name").EnterText(employee.Middlename);
            TextField("last name").EnterText(employee.Lastname);
            employee.EmployeeID = TextField("employee id").GetAttributeOrDefault("value");
            Button_button("save").Click();
            TextField("driver's license number").EnterText(employee.DriversLicenseNumber);
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

        public void UpdateExistingEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            if (IsEmployeeDisplayedOnCurrentPage(employee))
            {
                NewlyRegisteredEmployeeUpdateIcon(employee.EmployeeID).Click();
            }
            else
            {
                while (IsNextPageChevronDisplayed())
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
                return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).GetTrimmedText().ToLower();
            }
            else
            {
                while (IsNextPageChevronDisplayed())
                {
                    Pagination_Next.Click();
                    if (IsEmployeeDisplayedOnCurrentPage(employee))
                    {
                        return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).GetTrimmedText().ToLower();
                    }
                }
                return "Employee not found or record not updated.";
            }
        }

        public void DeleteEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            if (IsEmployeeDisplayedOnCurrentPage(employee))
            {
                NewlyRegisteredEmployeeDeleteIcon(employee.EmployeeID).Click();
            }
            else
            {
                while (IsNextPageChevronDisplayed())
                {
                    Pagination_Next.Click();
                    if (IsEmployeeDisplayedOnCurrentPage(employee))
                    {
                        NewlyRegisteredEmployeeDeleteIcon(employee.EmployeeID).Click();
                        break;
                    }
                }
            }
            Button_button("yes, delete").Click();
        }
    }
}