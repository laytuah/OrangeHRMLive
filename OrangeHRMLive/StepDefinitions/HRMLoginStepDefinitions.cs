using FluentAssertions;
using OrangeHRMLive.Model;
using OrangeHRMLive.PageObjects;
using TechTalk.SpecFlow;

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
            _scenarioContext.Set<EmployeeProfile>(employee, "employee");
        }

        [StepDefinition(@"the user supplies the provided login details")]
        public void WhenTheUserSuppliesTheProvidedLoginDetails()
        {
            _loginPage.Login();
        }

        [StepDefinition(@"the user must land on the homepage")]
        public void ThenTheUserMustLandOnTheHomepage()
        {
            _homePage.IsPieChartDispalyed().Should().BeTrue();
            _homePage.IsSidePanelDisplayed().Should().BeTrue();
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
    }
}
