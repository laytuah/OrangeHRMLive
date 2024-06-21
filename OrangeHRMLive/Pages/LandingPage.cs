using OrangeHRMLive.Hooks;

namespace OrangeHRMLive.Pages
{
    public class LandingPage
    {
        Context context;
        public LandingPage(Context _context)
        {
            context = _context;
        }
        public void LoadSUT()
        {
            context.driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com");
        }
    }
}
