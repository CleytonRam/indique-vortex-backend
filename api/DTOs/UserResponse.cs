namespace ReferralApi.DTOs
{
    public class UserResponse
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string refCode { get; set; } = string.Empty;
        public int points { get; set; }
        public string refLink { get; set; } = string.Empty;
        public string? referredByName { get; set; }



    }
}
