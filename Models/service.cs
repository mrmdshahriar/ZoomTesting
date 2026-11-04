using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace ZoomTesting.Models
{
    public class ZoomMeetingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ZoomMeetingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // 1. Fetch the Server-to-Server Access Token
        public async Task<string> GetAccessTokenAsync()
        {
            var zoomConfig = _configuration.GetSection("Zoom");
            var accountId = zoomConfig["AccountId"];
            var clientId = zoomConfig["ClientId"];
            var clientSecret = zoomConfig["ClientSecret"];

            var tokenUrl = $"https://zoom.us{accountId}";

            var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(jsonString);

            return jsonDoc.RootElement.GetProperty("access_token").GetString()
                   ?? throw new InvalidOperationException("Failed to retrieve access token.");
        }

        // 2. Create the Meeting
        public async Task<ZoomMeetingResponse> CreateMeetingAsync(string topic, int durationInMinutes)
        {
            var accessToken = await GetAccessTokenAsync();

            // Zoom API Endpoint to create a meeting for the authenticated user ('me')
            var url = "https://api.zoom.us/v2/users/me/meetings";

            var meetingRequest = new
            {
                topic = topic,
                type = 2, // 2 = Scheduled Meeting
                duration = durationInMinutes,
                settings = new
                {
                    host_video = true,
                    participant_video = true,
                    join_before_host = false,
                    mute_upon_entry = true
                }
            };

            var jsonPayload = JsonSerializer.Serialize(meetingRequest);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Zoom API Error: {jsonResponse}");
            }

            return JsonSerializer.Deserialize<ZoomMeetingResponse>(jsonResponse)!;
        }
    }

    // DTO to capture essential Zoom API responses
    public class ZoomMeetingResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;

        [JsonPropertyName("join_url")]
        public string JoinUrl { get; set; } = string.Empty;

        [JsonPropertyName("start_url")]
        public string StartUrl { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
