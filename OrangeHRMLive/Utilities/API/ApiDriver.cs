using OrangeHRMLive.Model.API.Response;

namespace OrangeHRMLive.Utilities.Api
{
    public class ApiDriver
    {
        private readonly ApiClient _client;

        public ApiDriver(ApiClient client)
        {
            _client = client;
        }

        private ApiResponse? _lastResponse;

        public async Task SendGet(string path)
        {
            _lastResponse = await _client.GetAsync(path);
        }

        public ApiResponse LastResponse =>
            _lastResponse ?? throw new System.InvalidOperationException("No API response available. Call a request step first.");
    }
}
