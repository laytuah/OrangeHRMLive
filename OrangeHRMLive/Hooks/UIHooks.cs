using OrangeHRMLive.Configuration;
using OrangeHRMLive.Utilities.UI;
using Reqnroll;

namespace OrangeHRMLive.Hooks
{
    [Binding]
    [Scope(Tag = "ui")]
    internal class UiHooks
    {
        private readonly WebDriverSupport _webDriverSupport;

        public UiHooks(WebDriverSupport webDriverSupport) => _webDriverSupport = webDriverSupport;

        [BeforeScenario]
        public void BeforeScenario()
        {
            _webDriverSupport.InitializeBrowser(ConfigurationManager.BrowserName);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _webDriverSupport.CloseAUT();
        }
    }
}
