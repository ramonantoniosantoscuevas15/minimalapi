using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using minimalapi;
using minimalapi.Endpoints;
using minimalapi.Entidades;
using minimalapi.Migrations;
using minimalapi.Repositorios;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<String>("origenesPermitidos")!;
//inicio de servicios
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>

     {
         configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
     });
    opciones.AddPolicy("Libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });
});


builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepositorioGeneros, RepositoriosGeneros>();
builder.Services.AddAutoMapper(typeof(Program));
//fin de servicios

builder.Services.AddDbContext<AplicationDbContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();
//inicio de area de los middleware

//usar si es necesaria la produccion
//if (builder.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseOutputCache();


app.MapGet("/", [EnableCors(policyName: "Libre")] () => "Hello World!");

 app.MapGroup("/generos").MapGeneros();


// fin del middleware
app.Run();

