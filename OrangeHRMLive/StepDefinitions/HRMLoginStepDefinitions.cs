using FluentAssertions;
using OrangeHRMLive.PageObjects;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.StepDefinitions
{
    [Binding]
    public class HRMLoginStepDefinitions
    {
        LoginPage loginPage;
        BasePage basePage;
        HomePage homepage;
        public HRMLoginStepDefinitions(LoginPage _loginPage, BasePage _basePage, HomePage _homepage)
        {
            loginPage = _loginPage;
            basePage = _basePage;
            homepage = _homepage;
        }

        [StepDefinition(@"that user navigates to HRMLive page")]
        public void GivenThatUserNavigatesToHRMLivePage()
        {
            basePage.LoadAUT();
        }

        [StepDefinition(@"the user supplies the provided login details")]
        public void WhenTheUserSuppliesTheProvidedLoginDetails()
        {
            loginPage.Login();
        }

        [StepDefinition(@"the user must land on the homepage")]
        public void ThenTheUserMustLandOnTheHomepage()
        {
            homepage.GetHomePageConfirmation().Should().BeTrue();
            homepage.GetUserFullName().Should().Contain("Abdelrahman Mubarak");
        }
    }
}
