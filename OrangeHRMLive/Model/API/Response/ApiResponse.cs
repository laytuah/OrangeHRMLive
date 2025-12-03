namespace OrangeHRMLive.Model.API.Response
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; } = "";
        public string Content { get; set; } = "";
        public long DurationMs { get; set; }
    }
}
