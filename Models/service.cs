using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Newtonsoft.Json;
using static ZoomTesting.Models.ZoomMeetingService;

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


        static readonly string ZOOM_ACCOUNT_ID = "BoMQjc7jQAuA8DLJrjyFLQ";
        static readonly string ZOOM_CLIENT_ID = "dvjIuLMTdyb4_pFQoFvQQ";
        static readonly string ZOOM_CLIENT_SECRET = "wco3GnCK2TibJ4KPG5Giba5n0y0U6DeR";

        //static readonly string ZOOM_ACCOUNT_ID = "406KHXdXRHqppqO5SQ2aFw";
        //static readonly string ZOOM_CLIENT_ID = "jltuUxaMSb2oitaTRp3wRA";
        //static readonly string ZOOM_CLIENT_SECRET = "qeFsdkzPqJW7GxXvx6qWXusriXEHfIIe";



        //[HttpPost("/api/meeting/getAccessToken")]
        public async Task<UserToken> GetAccessToken([FromBody] Request request)  // request = null
        {
            using (var client = new HttpClient())
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{ZOOM_CLIENT_ID}:{ZOOM_CLIENT_SECRET}");
                var encodedString = System.Convert.ToBase64String(plainTextBytes);
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedString}");
                var response = await client.PostAsJsonAsync($"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={ZOOM_ACCOUNT_ID}", request);
                string result = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<UserToken>(result);
                return userResponse!;
            }
        }


        //[HttpPost("/api/meeting/createMeeting")]
        public async Task<MeetingResponse> CreateMeeting([FromBody] Request? request)
        {
            string userId = string.Empty;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {request?.Token}");
                var response = await client.GetAsync("https://api.zoom.us/v2/users");
                string result = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ZoomUserResponse>(result);
                userId = userResponse.users[0].id;
            }

            using (var client = new HttpClient())
            {
                request.MeetingRequest.settings = new Settings
                {
                    approval_type = 0,
                    host_video = true,
                    join_before_host = true,
                    mute_upon_entry = true,
                    participant_video = true
                };
                //request.MeetingRequest.start_time = Convert.ToDateTime(request.MeetingRequest.start_time).ToString("YYYY-MM-ddTHH:mm:ssZ");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.Token}");
                var response = await client.PostAsJsonAsync($"https://api.zoom.us/v2/users/{userId}/meetings", request.MeetingRequest);
                string result = await response.Content.ReadAsStringAsync();
                var mr = JsonConvert.DeserializeObject<MeetingResponse>(result);
                //var data = dashboardDAL.InsertTELS154(mr.id, mr.topic, mr.join_url, request.MeetingRequest.start_time, mr.password, request.MeetingRequest.duration, HttpContext.Session.GetString("EMP_CODE"));
                return mr;
            }
            //using (var client = new HttpClient())
            //{
            //    request.MeetingRequest.settings = new Settings
            //    {
            //        approval_type = 0,
            //        host_video = true,
            //        join_before_host = true,
            //        mute_upon_entry = true,
            //        participant_video = true
            //    };
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.Token}");
            //    //var response = await client.PostAsJsonAsync($"https://api.zoom.us/v2/report/meetings/82920081127/participants", request.MeetingRequest);
            //    var response = await client.GetAsync($"https://api.zoom.us/v2/meetings/87140885608");
            //    string result = await response.Content.ReadAsStringAsync();
            //    var mr = JsonConvert.DeserializeObject<MeetingResponse>(result);
            //    var data = dashboardDAL.InsertTELS154(mr.id, mr.topic, mr.join_url, request.MeetingRequest.start_time, mr.password, request.MeetingRequest.duration, HttpContext.Session.GetString("EMP_CODE"));
            //    return Ok(mr);
            //}
        }




        //-------------------------------------------------------

        //    // 1. Fetch the Server-to-Server Access Token
        //    public async Task<string> GetAccessTokenAsync()
        //    {
        //        var zoomConfig = _configuration.GetSection("Zoom");
        //        var accountId = zoomConfig["AccountId"];
        //        var clientId = zoomConfig["ClientId"];
        //        var clientSecret = zoomConfig["ClientSecret"];

        //        var tokenUrl = $"https://zoom.us{accountId}";

        //        var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        //        using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

        //        var response = await _httpClient.SendAsync(request);
        //        response.EnsureSuccessStatusCode();

        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        using var jsonDoc = JsonDocument.Parse(jsonString);

        //        return jsonDoc.RootElement.GetProperty("access_token").GetString()
        //               ?? throw new InvalidOperationException("Failed to retrieve access token.");
        //    }

        //    // 2. Create the Meeting
        //    public async Task<ZoomMeetingResponse> CreateMeetingAsync(string topic, int durationInMinutes)
        //    {
        //        var accessToken = await GetAccessTokenAsync();

        //        // Zoom API Endpoint to create a meeting for the authenticated user ('me')
        //        var url = "https://api.zoom.us/v2/users/me/meetings";

        //        var meetingRequest = new
        //        {
        //            topic = topic,
        //            type = 2, // 2 = Scheduled Meeting
        //            duration = durationInMinutes,
        //            settings = new
        //            {
        //                host_video = true,
        //                participant_video = true,
        //                join_before_host = false,
        //                mute_upon_entry = true
        //            }
        //        };

        //        var jsonPayload = JsonSerializer.Serialize(meetingRequest);
        //        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.SendAsync(request);
        //        var jsonResponse = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            throw new Exception($"Zoom API Error: {jsonResponse}");
        //        }

        //        return JsonSerializer.Deserialize<ZoomMeetingResponse>(jsonResponse)!;
        //    }
        //}

        //// DTO to capture essential Zoom API responses
        //public class ZoomMeetingResponse
        //{
        //    [JsonPropertyName("id")]
        //    public long Id { get; set; }

        //    [JsonPropertyName("topic")]
        //    public string Topic { get; set; } = string.Empty;

        //    [JsonPropertyName("join_url")]
        //    public string JoinUrl { get; set; } = string.Empty;

        //    [JsonPropertyName("start_url")]
        //    public string StartUrl { get; set; } = string.Empty;

        //    [JsonPropertyName("password")]
        //    public string Password { get; set; } = string.Empty;
    }
}
