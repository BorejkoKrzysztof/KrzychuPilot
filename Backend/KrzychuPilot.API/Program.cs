using FluentValidation;
using KrzychuPilot.API.Hubs;
using KrzychuPilot.API.Middleware;
using KrzychuPilot.API.Services;
using KrzychuPilot.Application.Common.Interfaces;
using KrzychuPilot.Application.Common.Models.Behaviours;
using KrzychuPilot.Application.Prompts.Commands.CreatePrompt;
using KrzychuPilot.Infrastructure.BackgroundServices;
using KrzychuPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreatePromptGroupCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(CreatePromptGroupCommand).Assembly);

builder.Services.AddSignalR();
builder.Services.AddSingleton<IPromptNotificationService, PromptNotificationService>();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<PromptProcessingWorker>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS with origins from environment variable
var corsOrigins = builder.Configuration.GetValue<string>("CORS_ORIGINS")?.Split(",") ?? new[] { "http://localhost:5173", "http://localhost:3000" };
builder.Services.AddCors(options => options.AddPolicy("AllowFront", policy => policy.WithOrigins(corsOrigins)
                                                   .AllowAnyMethod()
                                                   .AllowAnyHeader()
                                                   .AllowCredentials()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseCors("AllowFront");
app.UseAuthorization();
app.MapControllers();


app.MapHub<PromptHub>("/promptHub");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while migrating the database");
    }
}


app.Run();

