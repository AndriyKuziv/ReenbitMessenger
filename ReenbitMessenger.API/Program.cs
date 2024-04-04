using ReenbitMessenger.DataAccess.AppServices;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Library.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.AppServices.Commands;
using ReenbitMessenger.DataAccess.AppServices.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IdentityDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnection"));
});

var keyString = builder.Configuration["JwtKey"];
var key = Encoding.ASCII.GetBytes(keyString);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});


builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<IdentityDataContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<ICommandHandler<EditUserInfoCommand>, EditUserInfoCommandHandler>();
builder.Services.AddTransient<IQueryHandler<GetUserByIdQuery, User>, GetUserByIdQueryHandler>();

builder.Services.AddSingleton<HandlersDispatcher>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
