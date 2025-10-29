using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Options
{
    public class JwtSettingsOptions
    {
        public const string JwtSettings = "JwtSettings";

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Duration { get; set; }
    }
}
