namespace cp_randomcard.Models.DTOs
{
    [Serializable]
    public record RegisterUserDto(string username, string password, bool isActive = true);
}
