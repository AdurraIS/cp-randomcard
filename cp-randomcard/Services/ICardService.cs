using cp_randomcard.Models.DTOs;

namespace cp_randomcard.Services
{
    public interface ICardService
    {
        Task<IResult> GetRandomCard();
        Task<IResult> CreateCard(CardCreateDTO dto);
    }
}
