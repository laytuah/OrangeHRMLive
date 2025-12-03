using FluentAssertions;
using OrangeHRMLive.Model.API.Response;
using OrangeHRMLive.Utilities.Api;
using Reqnroll;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeHRMLive.StepDefinitions.API
{
    [Binding]
    [Scope(Tag = "api")]
    public class BookingApiSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ApiDriver _api;

        public BookingApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _api = scenarioContext.ScenarioContainer.Resolve<ApiDriver>();
        }

        [Given(@"that user sends a GET request to '([^']*)' endpoint")]
        public async Task GivenThatUserSendsAGetRequestTo(string path)
        {
            await _api.SendGet(path);
        }

        [Then(@"the response should be (\d+) '([^']*)'")]
        public void ThenTheResponseShouldBe(int expectedStatus, string expectedReason)
        {
            var res = _api.LastResponse;

            res.StatusCode.Should().Be(expectedStatus);

            // Normalize e.g., 'Not Found' vs 'NotFound'
            static string Norm(string s) => new string(s.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
            Norm(res.ReasonPhrase).Should().Be(Norm(expectedReason));
        }

        [Then(@"the response should contain BookingID '(\d+)'")]
        public void ThenTheResponseShouldContainBookingId(int expectedId)
        {
            using var r = new ApiResponseReader(_api.LastResponse, null);
            var list = r.Deserialize<List<BookingListItem>>();
            list.Any(x => x.BookingId == expectedId)
                .Should().BeTrue($"expected BookingID {expectedId} in the response.");
        }
    }
}
