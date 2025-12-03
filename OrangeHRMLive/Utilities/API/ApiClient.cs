using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using OrangeHRMLive.Model.API.Response;

namespace OrangeHRMLive.Utilities.Api
{
    /// <summary>
    /// Thin wrapper around HttpClient for raw HTTP calls.
    /// Assumes BaseAddress & Timeout are already set on the provided HttpClient.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> GetAsync(string path)
        {
            var requestPath = path.StartsWith("/") ? path : "/" + path;

            var sw = Stopwatch.StartNew();
            using var response = await _httpClient.GetAsync(requestPath);
            sw.Stop();

            var content = await response.Content.ReadAsStringAsync();

            return new ApiResponse
            {
                StatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase ?? response.StatusCode.ToString(),
                Content = content,
                DurationMs = sw.ElapsedMilliseconds
            };
        }
    }
}
