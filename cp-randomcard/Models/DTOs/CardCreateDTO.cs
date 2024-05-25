namespace cp_randomcard.Models.DTOs
{
    [Serializable]
    public record CardCreateDTO(string Title, string Atribute, int Power, int Health);
}
