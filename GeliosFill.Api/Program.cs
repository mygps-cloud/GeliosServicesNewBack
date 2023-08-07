using GeliosFill.Api.AppServices.FineAppService;
using GeliosFill.Api.AppServices.FuelFillAppService;
using GeliosFill.Api.AppServices.UserAppService;
using GeliosFill.Data;
using Microsoft.EntityFrameworkCore;

const string angularApp = "AngularApp";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb"), b => b.MigrationsAssembly("GeliosFill.Api")));
builder.Services.AddDbContext<MyGpsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyGpsDb")));
builder.Services.AddDbContext<SmsSenderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmsSenderDb"), b => b.MigrationsAssembly("GeliosFill.Api")));


builder.Services.AddCors(options => options.AddPolicy(angularApp, build => build
    .AllowAnyHeader()
    .WithOrigins("http://192.168.1.133:4200", "http://localhost:4200", "https://cardistance.mygps.ge")
    .AllowAnyMethod()
    .AllowCredentials()));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IFuelFill, FuelFill>();
builder.Services.AddTransient<IUser, User>();
builder.Services.AddTransient<IFine, Fine>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(angularApp);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();