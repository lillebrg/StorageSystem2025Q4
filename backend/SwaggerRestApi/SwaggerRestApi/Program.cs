using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SwaggerRestApi;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.DBAccess;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddDbContext<DBContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Storage API",
                Version = "v1",
                Description = "En løsning til opbevaring i et lager og mulighed for at udlåne ting derfra"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
            });
        });

        // Adds DBAccess so it gets dependency injected
        builder.Services.AddScoped<UserDBAccess>();
        builder.Services.AddScoped<ItemDBAccess>();
        builder.Services.AddScoped<ShelfDBAccess>();
        builder.Services.AddScoped<RackDBAccess>();
        builder.Services.AddScoped<StorageDBAccess>();
        builder.Services.AddScoped<BorrowedRequestDBAccess>();
        builder.Services.AddScoped<NotificationDBAccess>();

        // Adds Business logic so it gets dependency injected
        builder.Services.AddScoped<UserLogic>();
        builder.Services.AddScoped<BaseItemLogic>();
        builder.Services.AddScoped<SpecificItemLogic>();
        builder.Services.AddScoped<ShelfLogic>();
        builder.Services.AddScoped<RackLogic>();
        builder.Services.AddScoped<StorageLogic>();
        builder.Services.AddScoped<BorrowedRequestLogic>();

        builder.Services.AddScoped<SharedLogic>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerBootstrap(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        await using var context = app.Services.CreateAsyncScope().ServiceProvider.GetService<DBContext>();
        SeedData seedData = new SeedData(context);
        await seedData.StartUserData();

        if (app.Environment.IsDevelopment())
        {
            await seedData.StartStorageData();
            await seedData.StartItemsData();
        }

        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
