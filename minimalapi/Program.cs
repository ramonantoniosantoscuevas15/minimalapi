using minimalapi.Entidades;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/generos",() =>
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
