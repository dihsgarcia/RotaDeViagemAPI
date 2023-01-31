global using RotaDeViagemAPI.Models;
using Microsoft.EntityFrameworkCore;
using RotaDeViagemAPI.Data;
using RotaDeViagemAPI.Services.RotaDeViagemService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    //caso necessario mudar connection string que atenda a configação de BD Local
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionExpress"));
});


//builder.Services.AddScoped<IRotaDeViagemService, RotaDeViagemService>();

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

void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IRotaDeViagemService, RotaDeViagemService>();
}