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
        readonly HomePage _homepage;
        public HRMLoginStepDefinitions(LoginPage loginPage, BasePage basePage, HomePage homepage)
        {
            _loginPage = loginPage;
            _basePage = basePage;
            _homepage = homepage;
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
            _homepage.IsPieChartDispalyed().Should().BeFalse();
            _homepage.IsSidePanelDisplayed().Should().BeTrue();
        }
    }
}
