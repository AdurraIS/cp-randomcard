
using cp_randomcard.DTOs;

namespace cp_randomcard.Services
{
    public interface ICardService
    {
        Task<IResult> GetRandomCard();
        Task<IResult> CreateCard(CardCreateDTO dto);
    }
}
