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

    // login configured
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    
    var logger = LoggerFactory.Create(config => 
    {
        config.AddConsole();
        config.AddDebug();
    }).CreateLogger("Program");

    logger.LogInformation("Starting application...");

    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

    // then i registered core services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
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

       
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    // registration for helpers and services
    builder.Services.AddSingleton<PasswordHelper>();
    builder.Services.AddSingleton<IUserRepo>(serviceProvider =>
    {
        var connectionString = "Filename=MyData.db; Connection=shared";
        return new UserRepo(connectionString);
    });

    // Register repositories
    builder.Services.AddSingleton(typeof(IAmenityRepo), typeof(AmenityRepo));
    builder.Services.AddSingleton(typeof(IBookingRepo),typeof(BookingRepo));
    builder.Services.AddSingleton(typeof(ICampingSpotRepo),typeof(CampingSpotRepo));
    builder.Services.AddSingleton(typeof(IImageRepo),typeof(ImageRepo));
    builder.Services.AddSingleton(typeof(ILocationRepo), typeof(LocationRepo));
    builder.Services.AddSingleton(typeof(IPaymentRepo), typeof(PaymentRepo));
    builder.Services.AddSingleton(typeof(IReviewRepo), typeof(ReviewRepo));

   
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
    builder.Services.AddDirectoryBrowser();

    logger.LogInformation("Building the application...");
    var app = builder.Build();

    // Ensured the upload directory exists
    var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
    Directory.CreateDirectory(uploadsPath);

    // Configured the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        logger.LogInformation("Configuring Swagger for development environment");
        app.UseSwagger();
        app.UseSwaggerUI();
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