namespace Views;

using Models;
using Services;
using Spectre.Console;

public class ArtistMenu
{
    private static ArtistService artistService = new();

    public void showArtists()
    {
        Artist opcion = AnsiConsole.Prompt(
                new SelectionPrompt<Artist>()
                    .Title("[bold underline green] LISTA DE ARTISTAS[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(ArtistService.artists)
                    .UseConverter(choice => $"{choice.Name}"));

    }
    public void Register()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Artista:[/]");
        AnsiConsole.WriteLine();

        var Name = AnsiConsole.Ask<string>("Introduce el nombre del Artista: ");
        var Followers = AnsiConsole.Ask<int>("Introduce el número de seguidores: ");
        var Biography = AnsiConsole.Ask<string>("Introduce la biografía: ");

        Artist artist = new Artist
        {
            Id = ArtistService.artists.Count + 1,
            Name = Name,
            Followers = Followers,
            CreateDate = DateTime.Now,
            Biography = Biography,
            SoftDelete = false,
            Albums = []
        };
        artistService.AddArtist(artist);
    }


}