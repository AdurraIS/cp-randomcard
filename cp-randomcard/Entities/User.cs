
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cp_randomcard.Entities
{
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Username { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public bool IsActive { get; set; }

        public User(string? username, string? password, bool isActive)
        {
            Username = username;
            Password = password;
            IsActive = isActive;
        }
    }
}
