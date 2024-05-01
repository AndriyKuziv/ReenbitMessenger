using ReenbitMessenger.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using ReenbitMessenger.API.Services;
using Microsoft.AspNetCore.ResponseCompression;
using ReenbitMessenger.API.Hubs;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseDefaultServiceProvider(options => options.ValidateScopes = false);

var config = builder.Configuration;

builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddHealthChecks();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter a valid JWT Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[]{} }
    });
});

builder.Services.AddDbContext<MessengerDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnection"));
});

builder.Services.AddAuthenticationServices(config);

builder.Services.AddRepositories();

builder.Services.AddApiServices();

builder.Services.AddValidators();

builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MessengerDataContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");

app.MapControllers();

app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed( _ => true)
                .AllowCredentials());

app.Run();

public partial class Program { }
