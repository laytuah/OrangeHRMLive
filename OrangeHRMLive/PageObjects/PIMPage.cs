using OpenQA.Selenium;
using OrangeHRMLive.Model;

namespace OrangeHRMLive.PageObjects
{
    public class PIMPage : BasePage
    {
        public PIMPage(IWebDriver driver) : base(driver) { }

        protected PageElement TextField(string text) => new PageElement(Driver, By.XPath($"//input[normalize-space(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))=\"{text}\"] | //div[label[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{text}\"]]/following-sibling::div//input"));

        protected PageElement SelectField(int index = 1) => new PageElement(Driver, By.XPath($"(//div[@class='oxd-select-text--after'])[{index}]"));

        protected PageElement SelectGender(string gender) => new PageElement(Driver, By.XPath($"//div[@class='--gender-grouped-field']//label[contains(.,\"{gender}\")]"));

        protected PageElement NewlyRegisteredEmployee(string? ID, string? firstName, string? lastName) => new PageElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\") and contains(.,\"{firstName}\") and contains(.,\"{lastName}\")]"));

        protected PageElement NewlyRegisteredEmployeeUpdateIcon(string? ID) => new PageElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\")]//i[@class='oxd-icon bi-pencil-fill']"));

        protected PageElement NewlyRegisteredEmployeeDeleteIcon(string? ID) => new PageElement(Driver, By.XPath($"//div[@class='oxd-table-card' and contains(.,\"{ID}\")]//i[@class='oxd-icon bi-trash']"));

        protected PageElement Pagination_Next => new PageElement(Driver, By.XPath("//i[@class='oxd-icon bi-chevron-right']"));

        protected PageElement ConfirmationToastMessages(string confirmationText) => new PageElement(Driver, By.XPath($"//div[@class='oxd-toast-container oxd-toast-container--bottom']//p[text()='{confirmationText}']"));

        protected PageElement FirstUserOnPimTable => new PageElement(Driver, By.XPath("(//div[@class='oxd-table-card'])[1]"));

        protected PageElement DeleteFirstUserOnPimTable => new PageElement(Driver, By.XPath("(//i[@class='oxd-icon bi-trash'])[1]"));

        public void RegisterNewEmployee(EmployeeProfile employee)
        {
            Mainmenu_item("pim").Click();
            Button_button("add").Click();
            TextField("first name").ClearAndSendKeys(employee.Firstname);
            TextField("middle name").ClearAndSendKeys(employee.Middlename);
            TextField("last name").ClearAndSendKeys(employee.Lastname);
            TextField("employee id").ClearAndSendKeys(employee.EmployeeID);
            Button_button("save").Click();
            //SelectField().Click();
            //Select_dropdown(employee.Nationality).Click();

            //SelectField(2).Click();
            //Select_dropdown(employee.MaritalStatus).Click();
            //SelectField(3).Click();
            //Select_dropdown(employee.BloodGroup).Click();
            //SelectGender(employee.Gender).Click();
            //Button_button("save").Click();
            //Button_button("save", 2).Click();
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

        public void UpdateExistingEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").ActionClick();
            FirstUserOnPimTable.Click();
            //if (IsEmployeeDisplayedOnCurrentPage(employee))
            //{
            //    NewlyRegisteredEmployeeUpdateIcon(employee.EmployeeID).ActionClick();
            //}
            //else
            //{
            //    while (IsNextPageChevronDisplayed())
            //    {
            //        Pagination_Next.Click();
            //        if (IsEmployeeDisplayedOnCurrentPage(employee))
            //        {
            //            NewlyRegisteredEmployeeUpdateIcon(employee.EmployeeID).ActionClick();
            //            break;
            //        }
            //    }
            //}

            Link_anchor("job").Click();
            SelectField().Click();
            Select_dropdown(employee.JobTitle).Click();
            SelectField(5).Click();
            Select_dropdown(employee.EmploymentStatus).Click();
            Button_button("save").Click();
        }

        public bool IsUpdateToastMessageDisplayed()
        {
            return ConfirmationToastMessages("Successfully Updated").IsDisplayed();
        }

        public bool IsNewlyCreatedEmployeeToastMessageDisplayed()
        {
            return ConfirmationToastMessages("Successfully Saved").IsDisplayed();
        }

        public bool IsDeleteToastMessageDisplayed()
        {
            return ConfirmationToastMessages("Successfully Deleted").IsDisplayed();
        }

        public string GetUpdatedEmployeeText(EmployeeProfile employee)
        {
            Mainmenu_item("pim").ActionClick();
            if (IsEmployeeDisplayedOnCurrentPage(employee))
            {
                return NewlyRegisteredEmployee(employee.EmployeeID, employee.Firstname, employee.Lastname).Text.ToLower();
            }
            else
            {
                while (IsNextPageChevronDisplayed())
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

        public void DeleteExistingEmployeeRecord(EmployeeProfile employee)
        {
            Mainmenu_item("pim").ActionClick();
            DeleteFirstUserOnPimTable.ActionClick();
            if (Button_button("yes, delete").ElementExists() && Button_button("yes, delete").IsDisplayed())
                Button_button("yes, delete").ActionClick();
            //    if (IsEmployeeDisplayedOnCurrentPage(employee))
            //    {
            //        ClickDeleteButton(employee);
            //    }
            //    else
            //    {
            //        while (IsNextPageChevronDisplayed())
            //        {
            //            Pagination_Next.Click();
            //            if (IsEmployeeDisplayedOnCurrentPage(employee))
            //            {
            //                ClickDeleteButton(employee);
            //                break;
            //            }
            //        }
            //    }
        }

        public void ClickDeleteButton(EmployeeProfile employee)
        {
            while (IsEmployeeDisplayedOnCurrentPage(employee))
            {
                NewlyRegisteredEmployeeDeleteIcon(employee.EmployeeID).ActionClick();
                if (Button_button("yes, delete").ElementExists() && Button_button("yes, delete").IsDisplayed())
                    Button_button("yes, delete").ActionClick();
            }
        }
    }
}