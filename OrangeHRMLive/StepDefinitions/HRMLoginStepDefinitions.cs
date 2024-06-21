using OrangeHRMLive.Hooks;
using OrangeHRMLive.Pages;
using System;
using TechTalk.SpecFlow;

namespace OrangeHRMLive.StepDefinitions
{
    [Binding]
    public class HRMLoginStepDefinitions
    {
        Context context; LandingPage landingPage;
        public HRMLoginStepDefinitions(Context _context, LandingPage _landingPage)
        {
                context = _context; landingPage = _landingPage;
        }

        [StepDefinition(@"that user navigates to HRMLive page")]
        public void GivenThatUserNavigatesToHRMLivePage()
        {
            context.StartBrowser("chrome");
            landingPage.LoadSUT();
        }

        [StepDefinition(@"the user supplies the provided login details")]
        public void WhenTheUserSuppliesTheProvidedLoginDetails()
        {
            throw new PendingStepException();
        }

        [StepDefinition(@"the user must land on the homepage")]
        public void ThenTheUserMustLandOnTheHomepage()
        {
            throw new PendingStepException();
        }
    }
}
