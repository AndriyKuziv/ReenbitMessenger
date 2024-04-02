using ReenbitMessenger.API.AppServices;
using ReenbitMessenger.API.Data;
using ReenbitMessenger.API.Repositories;
using ReenbitMessenger.API.Utils;
using ReenbitMessenger.Library.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddTransient<ICommandHandler<EditUserInfoCommand>, EditUserInfoCommandHandler>();
builder.Services.AddTransient<IQueryHandler<GetUserByIdQuery, User>, GetUserByIdQueryHandler>();

builder.Services.AddSingleton<HandlersDispatcher>();

builder.Services.AddDbContext<IdentityDataContext>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

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
