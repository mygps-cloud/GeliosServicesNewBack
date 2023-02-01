using GeliosFill.Api.AppServices.FuelFillAppService;
using GeliosFill.Api.AppServices.UserAppService;
using GeliosFill.Data;
using Microsoft.EntityFrameworkCore;

const string angularApp = "AngularApp";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));
builder.Services.AddDbContext<MyGpsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyGpsDb")));


builder.Services.AddCors(options => options.AddPolicy(angularApp, build => build
    .AllowAnyHeader()
    .WithOrigins("http://192.168.1.133:4200", "http://localhost:4200", "https://cardistance.mygps.ge")
    .AllowAnyMethod()
    .AllowCredentials()));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IFuelFill, FuelFill>();
builder.Services.AddTransient<IUser, User>();


var app = builder.Build();

// Configure the HTTP request pipeline.
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


