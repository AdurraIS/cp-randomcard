namespace cp_randomcard.Models.DTOs
{
    [Serializable]
    public record CardDTO(string Title, string Atribute, int Power, int Health);
}
