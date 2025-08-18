namespace Utils;

using Models;
using Services;
using Spectre.Console;

public class AlbumMenu
{
    private AlbumService albumService = new();
    private TrackMenu trackMenu = new();

    public void showAlbums()
    {
        Album opcionAlbum = AnsiConsole.Prompt(
                new SelectionPrompt<Album>()
                    .Title("[bold underline green] LISTA DE ALBUMES[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(albumService.GetAlbums())
                    .UseConverter(choice => $"{choice.Name}"));

        trackMenu.showTracksByAlbum(opcionAlbum);
    }

    public void Register()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Album:[/]");
        AnsiConsole.WriteLine();

        var Name = AnsiConsole.Ask<string>("Introduce el nombre del Album: ");
        var ReleaseDate = AnsiConsole.Ask<DateTime>("Introduce la fecha de lanzamiento: ");
        Artist opcion = AnsiConsole.Prompt(
            new SelectionPrompt<Artist>()
                .Title("[bold underline green] LISTA DE ARTISTAS[/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(ArtistService.artists)
                .UseConverter(choice => $"{choice.Name}"));

        Album album = new Album
        {
            Id = albumService.GetAlbums().Count + 1,
            Name = Name,
            IdArtist = opcion.Id,
            ReleaseDate = ReleaseDate,
            Tracks = [],
            Duration = 0,
            SoftDelete = false,

        };
        albumService.AddAlbum(album);
    }
}