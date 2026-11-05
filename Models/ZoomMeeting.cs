namespace ZoomTesting.Models
{
    public class Request
    {
        public string Token { get; set; }
        public MeetingRequest MeetingRequest { get; set; }
    }
    public class MeetingRequest
    {
        public string topic { get; set; }
        public int type { get; set; } = 1;
        public string password { get; set; }
        public string agenda { get; set; }
        public string start_time { get; set; }
        public string timezone { get; set; }
        public string duration { get; set; }
        public string lectureInformation { get; set; }
        public string speakerId { get; set; }
        public Settings settings { get; set; }
    }
    public class Settings
    {
        public bool host_video { get; set; } = true;
        public bool participant_video { get; set; } = true;
        public bool join_before_host { get; set; } = true;
        public bool mute_upon_entry { get; set; } = true;
        public int approval_type { get; set; } = 0;
    }

    public class UserToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }

    public class ZoomUserResponse
    {
        public int page_count { get; set; }
        public int page_number { get; set; }
        public int page_size { get; set; }
        public int total_records { get; set; }
        public User[] users { get; set; }
    }
    public class User
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public int type { get; set; }
        public long pmi { get; set; }
        public string timezone { get; set; }
        public int verified { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_login_time { get; set; }
        public string language { get; set; }
        public string phone_number { get; set; }
        public string status { get; set; }
    }
    public class MeetingResponse
    {
        public string uuid { get; set; }
        public long id { get; set; }
        public string host_id { get; set; }
        public string host_email { get; set; }
        public string topic { get; set; }
        public int type { get; set; }
        public string status { get; set; }
        public string timezone { get; set; }
        public string agenda { get; set; }
        public DateTime created_at { get; set; }
        public string start_url { get; set; }
        public string join_url { get; set; }
        public string password { get; set; }
        public string h323_password { get; set; }
        public string pstn_password { get; set; }
        public string encrypted_password { get; set; }
        public string start_time { get; set; }
        public string duration { get; set; }
        public Settings1 settings { get; set; }
    }
    public class Settings1
    {
        public bool host_video { get; set; }
        public bool participant_video { get; set; }
        public bool cn_meeting { get; set; }
        public bool in_meeting { get; set; }
        public bool join_before_host { get; set; }
        public bool mute_upon_entry { get; set; }
        public bool watermark { get; set; }
        public bool use_pmi { get; set; }
        public int approval_type { get; set; }
        public string audio { get; set; }
        public string auto_recording { get; set; }
        public bool enforce_login { get; set; }
        public string enforce_login_domains { get; set; }
        public string alternative_hosts { get; set; }
        public bool close_registration { get; set; }
        public bool registrants_confirmation_email { get; set; }
        public bool waiting_room { get; set; }
        public bool request_permission_to_unmute_participants { get; set; }
        public bool registrants_email_notification { get; set; }
        public bool meeting_authentication { get; set; }
    }
}
