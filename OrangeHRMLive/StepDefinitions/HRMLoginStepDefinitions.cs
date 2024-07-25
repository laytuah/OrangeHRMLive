using FluentAssertions;
using OrangeHRMLive.PageObjects;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.StepDefinitions
{
    [Binding]
    public class HRMLoginStepDefinitions
    {
        private readonly LoginPage _loginPage;
        private readonly BasePage _basePage;
        private readonly HomePage _homepage;
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
            _homepage.IsPieChartDispalyed().Should().BeTrue();
            _homepage.IsSidePanelDisplayed().Should().BeTrue();
        }
    }
}
