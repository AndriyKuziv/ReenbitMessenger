using ReenbitMessenger.DataAccess.AppServices;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.AppServices.Commands;
using ReenbitMessenger.DataAccess.AppServices.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseDefaultServiceProvider(options => options.ValidateScopes = false);

var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<IdentityDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnection"));
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
});

builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityDataContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenHandler, ReenbitMessenger.DataAccess.Repositories.TokenHandler>();

builder.Services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EditUserInfoCommand>, EditUserInfoCommandHandler>();

builder.Services.AddScoped<IQueryHandler<GetUserByIdQuery, IdentityUser>, GetUserByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<LogInQuery, IdentityUser>, LogInQueryHandler>();

builder.Services.AddSingleton<HandlersDispatcher>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/identity").MapIdentityApi<IdentityUser>().AllowAnonymous();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
