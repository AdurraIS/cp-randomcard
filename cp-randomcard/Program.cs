using cp_randomcard.Data;
using cp_randomcard.DTOs;
using cp_randomcard.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CardContext>(opts =>
opts.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));
builder.Services.AddDbContext<CardContext>(opt =>
        opt.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICardService, CardService>();

var app = builder.Build();

app.MapPost("/cards", async (ICardService cardService, CardCreateDTO dto) =>
{
    return await cardService.CreateCard(dto);
})
.Accepts<CardCreateDTO>("application/json")
.Produces<CardCreateDTO>(StatusCodes.Status201Created)
.WithName("Create Card");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
