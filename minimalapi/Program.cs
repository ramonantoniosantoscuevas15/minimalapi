using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using minimalapi;
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

var endpointsGeneros = app.MapGroup("/generos");

endpointsGeneros.MapGet("/", ObtenerGenenros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));

endpointsGeneros.MapGet("/{id:int}",ObtenerGenenroporId );

endpointsGeneros.MapPost("/", CrearGenero);

endpointsGeneros.MapPut("/{id:int}",Actualizargenero);

endpointsGeneros.MapDelete("/{id:int}",Borrargenero);
// fin del middleware
app.Run();

static async Task<Ok<List<minimalapi.Entidades.Generos>>> ObtenerGenenros (IRepositorioGeneros repositorio) 
{

    var generos = await repositorio.ObtenerTodos();
    return TypedResults.Ok(generos);
}

static async Task<Results<Ok<minimalapi.Entidades.Generos>,NotFound>> ObtenerGenenroporId (IRepositorioGeneros repositorio, int id) 
{
    var genero = await repositorio.ObtenerPorId(id);

    if (genero is null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(genero);

}

static async Task<Created<minimalapi.Entidades.Generos>> CrearGenero (minimalapi.Entidades.Generos genero, IRepositorioGeneros repositorio,
    IOutputCacheStore outputCacheStore) 
{
    var id = await repositorio.Crear(genero);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return TypedResults.Created($"/generos/{id}", genero);
}

static async Task<Results<NoContent,NotFound>> Actualizargenero (int id, minimalapi.Entidades.Generos genero, IRepositorioGeneros repositorio,
   IOutputCacheStore outputCacheStore
   ) 
{
    var existe = await repositorio.Existe(id);

    if (!existe)
    {
        return TypedResults.NotFound();
    }
    await repositorio.Actualizar(genero);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return TypedResults.NoContent();

}

static async Task<Results<NoContent,NotFound>> Borrargenero (int id, IRepositorioGeneros repositorio,
    IOutputCacheStore outputCacheStore) 
{
    var existe = await repositorio.Existe(id);

    if (!existe)
    {
        return TypedResults.NotFound();
    }
    await repositorio.Borrar(id);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return TypedResults.NoContent();
}