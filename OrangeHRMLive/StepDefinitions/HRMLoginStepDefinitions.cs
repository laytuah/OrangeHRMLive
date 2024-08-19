using BoDi;
using FluentAssertions;
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

        public HRMLoginStepDefinitions(IObjectContainer objectContainer)
        {
            _basePage = objectContainer.Resolve<BasePage>();
            _loginPage = objectContainer.Resolve<LoginPage>();
            _homePage = objectContainer.Resolve<HomePage>();
            _pimPage = objectContainer.Resolve<PIMPage>();
        }

        [StepDefinition(@"that user navigates to HRMLive page")]
        public void GivenThatUserNavigatesToHRMLivePage()
        {
            _basePage.LoadAUT();
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
            _pimPage.RegisterNewEmployee();
        }

        [StepDefinition(@"newly created record must be found on employee list")]
        public void ThenNewlyCreatedRecordMustBeFoundOnEmployeeList()
        {
            throw new PendingStepException();
        }
    }
}
