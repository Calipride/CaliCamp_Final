using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CaliCamp.DataAccess;
using CaliCamp.Helpers;
using CaliCamp.Services;
using System.Text;
using System.Reflection;

try 
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure logging
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    
    var logger = LoggerFactory.Create(config => 
    {
        config.AddConsole();
        config.AddDebug();
    }).CreateLogger("Program");

    logger.LogInformation("Starting application...");

    // Configuration
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

    // Core services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Configure JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "defaultSecretKey123!@#");
    
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"]
        };
    });

    // Swagger Configuration
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CaliCamp API",
            Version = "v1",
            Description = "API for managing camping spots, bookings, and related services",
            Contact = new OpenApiContact
            {
                Name = "CaliCamp Support",
                Email = "support@calicamp.com"
            }
        });

        // Add JWT Authentication to Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
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
                Array.Empty<string>()
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    // Database configuration
    var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");
    Directory.CreateDirectory(dataDirectory);
    var dbPath = Path.Combine(dataDirectory, "MyData.db");
    var connectionString = $"Filename={dbPath}; Connection=shared";
    
    // Register all repositories with consistent connection string
    builder.Services.AddSingleton<IUserRepo>(sp => new UserRepo(connectionString));
    builder.Services.AddSingleton<IBookingRepo>(sp => new BookingRepo(connectionString));
    builder.Services.AddSingleton<ICampingSpotRepo>(sp => new CampingSpotRepo(connectionString));
    builder.Services.AddSingleton<IPaymentRepo>(sp => new PaymentRepo(connectionString));
    builder.Services.AddSingleton<IReviewRepo>(sp => new ReviewRepo(connectionString));
    builder.Services.AddSingleton<IAmenityRepo>(sp => new AmenityRepo(connectionString));
    builder.Services.AddSingleton<IImageRepo>(sp => new ImageRepo(connectionString));
    builder.Services.AddSingleton<ILocationRepo>(sp => new LocationRepo(connectionString));

    // Register helpers and services
    builder.Services.AddSingleton<PasswordHelper>();
    builder.Services.AddScoped<IUserService, UserService>();

    // CORS configuration
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyPolicy", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    // Ensure required directories exist
    Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "uploads"));
    Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "Data"));

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => 
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CaliCamp API V1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseHttpsRedirection();
    app.UseCors("MyPolicy");
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    logger.LogInformation("Starting web server...");
    app.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Application failed to start: {ex.Message}");
    Console.Error.WriteLine($"Stack trace: {ex.StackTrace}");
    throw;
}