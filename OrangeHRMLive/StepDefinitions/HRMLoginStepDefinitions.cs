using FluentAssertions;
using OrangeHRMLive.Model;
using OrangeHRMLive.PageObjects;
using Reqnroll;
namespace OrangeHRMLive.StepDefinitions
{
    [Binding]
    public class HRMLoginStepDefinitions
    {
        readonly LoginPage _loginPage;
        readonly BasePage _basePage;
        readonly HomePage _homePage;
        readonly PIMPage _pimPage;
        readonly ScenarioContext _scenarioContext;

        public HRMLoginStepDefinitions(ScenarioContext scenarioContext)
        {
            _basePage = scenarioContext.ScenarioContainer.Resolve<BasePage>();
            _loginPage = scenarioContext.ScenarioContainer.Resolve<LoginPage>();
            _homePage = scenarioContext.ScenarioContainer.Resolve<HomePage>();
            _pimPage = scenarioContext.ScenarioContainer.Resolve<PIMPage>();
            _scenarioContext = scenarioContext.ScenarioContainer.Resolve<ScenarioContext>();
        }

        [StepDefinition(@"that user navigates to HRMLive page")]
        public void GivenThatUserNavigatesToHRMLivePage()
        {
            _basePage.LoadAUT();
            var employee = new EmployeeProfile();
            _scenarioContext.Set(employee, "employee");
        }

        [StepDefinition(@"the user supplies the provided login details")]
        public void WhenTheUserSuppliesTheProvidedLoginDetails()
        {
            _loginPage.Login();
        }

        [StepDefinition(@"the user must land on the homepage")]
        public void ThenTheUserMustLandOnTheHomepage()
        {
            (_homePage.AreConfirmationImagesDisplayed().pieChart && _homePage.AreConfirmationImagesDisplayed().sidePanel).Should().BeTrue();
        }

        [StepDefinition(@"the user adds a new employee record")]
        public void WhenTheUserAddsANewEmployeeRecord()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.RegisterNewEmployee(employee);
        }

        [StepDefinition(@"newly created record must be found on employee list")]
        public void ThenNewlyCreatedRecordMustBeFoundOnEmployeeList()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.IsNewlyRegisteredEmployeeDisplayed(employee).Should().BeTrue();
        }

        [StepDefinition(@"the user updates the new created employee")]
        public void WhenTheUserUpdatesTheNewCreatedEmployee()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.UpdateNewlyCreatedEmployeeRecord(employee);
        }

        [StepDefinition(@"the newly created employee record must be updated")]
        public void ThenTheNewlyCreatedEmployeeRecordMustBeUpdated()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.GetAllTextFromNewlyCreatedEmployee(employee).Should().Contain(employee.UpdatedFirstname.ToLower());
        }


        [StepDefinition(@"the user updates their information")]
        public void WhenTheUserUpdatesTheirInformation()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.UpdateMyInfo(employee);
        }

        [StepDefinition(@"their information must be updated")]
        public void ThenTheirInformationMustBeUpdated()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.GetAllAdminUserText().Should().Contain(employee.Lastname.ToLower());
        }


        [StepDefinition(@"the user deletes the first employee on employee list")]
        public void WhenTheUserDeletesTheLastEmployeeOnEmployeeList()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.DeleteFirstEmployeeRecord(employee);
        }

        [StepDefinition(@"the first record must be deleted from employee list")]
        public void ThenTheLastRecordMustBeDeletedFromEmployeeList()
        {
            var employee = _scenarioContext.Get<EmployeeProfile>("employee");
            _pimPage.GetFirstAndLastNameOfFirstEmployeeOnList().FirstName.Should().NotBe(employee.Firstname);
            _pimPage.GetFirstAndLastNameOfFirstEmployeeOnList().LastName.Should().NotBe(employee.Lastname);
        }
    }
}
