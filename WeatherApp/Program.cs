using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApp.DataAccess;
using WeatherApp.Managers;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Servisleri konteynere ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<ApiWeatherWebService>();
builder.Services.AddSingleton<WeatherManager>();
builder.Services.AddSingleton<WeatherDataRepository>();
builder.Services.AddSingleton<WeatherValidationService>();


builder.Services.AddHostedService<WeatherBackgroundService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();