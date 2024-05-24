
namespace cp_randomcard.DTOs
{
    [Serializable]
    public record CardCreateDTO(String Title, String Atribute, int Power, int Health);
}
