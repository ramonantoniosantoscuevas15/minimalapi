using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using minimalapi;
using minimalapi.Entidades;
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

app.MapGet("/", [EnableCors(policyName:"Libre")]() => "Hello World!");

app.MapGet("/generos", () =>
{
    var generos = new List<Generos>
    {
        new Generos
        {
            Id = 1,
            Nombre = "Drama"
        },
        new Generos
        {
            Id = 2,
            Nombre = "Accion"
        },
        new Generos
        {
            Id = 3,
            Nombre = "Comedia"
        },
    };
    return generos;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

app.MapPost("/generos", async (Generos genero, IRepositorioGeneros repositorio) =>
{
    var id = await repositorio.Crear(genero);
    return Results.Created($"/generos/{id}",genero);
});


app.Run();
