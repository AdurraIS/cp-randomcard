using cp_randomcard.Data;
using cp_randomcard.DTOs;
using cp_randomcard.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cp_randomcard.Services
{
    public class CardService : ICardService
    {
        private readonly CardContext _context;

        public CardService(CardContext context)
        {
            _context = context;
        }


        public async Task<IResult> GetRandomCard()
        {
            var count = await _context.Cards.CountAsync();
            var randomIndex = new Random().Next(0, count);
            var randomItem = await _context.Cards.Skip(randomIndex).FirstOrDefaultAsync();

            if (randomItem == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(randomItem);
        }
        public async Task<IResult> CreateCard(CardCreateDTO dto)
        {
            var card = new Card(dto.Title, dto.Atribute, dto.Power, dto.Health);

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return TypedResults.Created($"/cards/{card.Id}", card);
        }
    }
}
