using MellonAPI.Authorization;
using MellonAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MellonAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<MellonAPIContext>(
            options => options.UseSqlite("Data Source=mellon.db;Cache=Shared"));

        builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Auth0:Domain"];
            options.Audience = builder.Configuration["Auth0:Audience"];
        });

        builder.Services.AddAuthorization(options =>
        {
            string[] scopes = { "read:todos", "write:todos" };

            foreach(string scope in scopes)
            {
                options.AddPolicy(scope, policy => policy.Requirements.Add(new HasScopeRequirement(scope, $"{builder.Configuration["Auth0:Domain"]}/")));
            }            
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Auth0", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows()
                {
                    AuthorizationCode = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{builder.Configuration["Auth0:Domain"]}/authorize"),
                        Scopes = new Dictionary<string,string>()
                        {
                            { "read:todos", "Read todos" },
                            { "write:todos", "Create, edit, and delete todos" }
                        },
                        TokenUrl = new Uri($"{builder.Configuration["Auth0:Domain"]}/oauth/token"),
                    }
                }                 
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id = "Auth0"
                        }
                    },                    
                    new string[]{ }
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.OAuthAdditionalQueryStringParams(new()
                {
                    { "audience", app.Configuration["Auth0:Audience"] }
                });
            });
        //}

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
