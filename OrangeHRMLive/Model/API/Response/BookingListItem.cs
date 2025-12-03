using Newtonsoft.Json;

namespace OrangeHRMLive.Model.API.Response
{
    public class BookingListItem
    {
        [JsonProperty("bookingid")]
        public int BookingId { get; set; }
    }
}
