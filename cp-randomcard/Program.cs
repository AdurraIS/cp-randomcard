using cp_randomcard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using cp_randomcard.Infra.Data;
using cp_randomcard.HealthChecks;
using cp_randomcard.Config;
using cp_randomcard.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
#region Database
builder.Services.AddDbContext<CardContext>(opts =>
opts.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));

builder.Services.AddDbContext<UserContext>(opts =>
opts.UseOracle(builder.Configuration.GetConnectionString("FiapOracleConnection")));
#endregion

#region Services
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

#endregion

#region Application Services
builder.Services.Configure<SecurityDtos.AppSettings>(builder.Configuration.GetSection("AppSettings"));
#endregion

#region OpenApi/Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "Version 1",
        Title = "Card API V1",
        Description = "Essa é a versão 1 da API de Cards"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
});
#endregion

#region Versioning

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
}).AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

#endregion

#region Security
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Secret"] ?? string.Empty)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

#endregion

#region HealthChecks

builder.Services.AddHealthChecks()
    .AddOracle(builder.Configuration.GetConnectionString("FiapOracleConnection") ?? string.Empty,
        healthQuery: "SELECT 1 FROM DUAL", name: "Oracle Fiap Server",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "Feedback", "Database" })
    .AddCheck<RemoteHealthCheck>("Remote Health Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Remote" });

builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(10);
    opt.MaximumHistoryEntriesPerEndpoint(60);
    opt.SetApiMaxActiveRequests(1);
    opt.AddHealthCheckEndpoint("API Health", "api/health");
}).AddInMemoryStorage();

#endregion


var app = builder.Build();



#region Endpoints
app.MapPost("api/cards", async (ICardService cardService, CardCreateDTO dto) =>
{
    return await cardService.CreateCard(dto);
})
.Accepts<CardCreateDTO>("application/json")
.Produces<CardCreateDTO>(StatusCodes.Status201Created)
.WithName("Create Card");

app.MapGet("api/cards/random", async (ICardService cardService) =>
{
    return await cardService.GetRandomCard();
})
.Produces<CardCreateDTO>(StatusCodes.Status200OK)
.WithName("Get Random Card");

app.MapPost("/api/authenticate", async (IAuthenticationService authenticationService, SecurityDtos.AutenticateRequest loginRequest,
        HttpContext context) =>
{
    return await authenticationService.Autenticate(loginRequest, context);
})
    .AllowAnonymous()
    .Produces<string>();

app.MapPost("/api/register", async (IAuthenticationService authenticationService, RegisterUserDto dto,
        HttpContext context) =>
{
    return await authenticationService.Register(dto, context);
})
    .AllowAnonymous()
    .Produces<RegisterUserDto>(StatusCodes.Status201Created)
    .WithName("Register User");

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureMiddlewares();

app.Run();
