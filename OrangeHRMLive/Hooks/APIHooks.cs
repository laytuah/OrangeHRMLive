using OrangeHRMLive.Configuration;
using OrangeHRMLive.Utilities.Api;
using Reqnroll;
using Reqnroll.BoDi;

namespace OrangeHRMLive.Hooks
{
    [Binding]
    [Scope(Tag = "api")]
    internal class ApiHooks
    {
        private readonly IObjectContainer _container;

        public ApiHooks(IObjectContainer container) => _container = container;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.ApiBaseUrl),
                Timeout = TimeSpan.FromSeconds(ConfigurationManager.ApiTimeoutSeconds)
            };

            _container.RegisterInstanceAs(httpClient);
            _container.RegisterTypeAs<ApiClient, ApiClient>();
            _container.RegisterTypeAs<ApiDriver, ApiDriver>();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Optional: dispose client if you prefer:
            // _container.Resolve<HttpClient>().Dispose();
        }
    }
}
