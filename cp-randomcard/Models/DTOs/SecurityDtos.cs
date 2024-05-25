
using System.ComponentModel;

namespace cp_randomcard.Models.DTOs
{


    public class SecurityDtos
    {
        public record AutenticateRequest([property: DefaultValue("System")] string Username,
            [property: DefaultValue("System")] string Password);

        public record AutenticateResponse(string Nome, string Username, string Token);

        public class AppSettings
        {
            public string Secret { get; set; }

            public AppSettings()
            {

            }

            public AppSettings(string secret)
            {
                Secret = secret;
            }
        };
    }
}
