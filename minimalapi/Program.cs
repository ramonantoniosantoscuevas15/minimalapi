using minimalapi.Entidades;

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



//fin de servicios

var app = builder.Build();
//inicio de area de los middleware
app.UseCors();

app.MapGet("/", () => "Hello World!");

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
});

app.Run();
