namespace Utils;

using Models;
using Services;
using Spectre.Console;

public class TrackMenu
{
    private TrackService trackService = new();

    public void showTracksByAlbum(Album album)
    {
        Track opcion = AnsiConsole.Prompt(
                new SelectionPrompt<Track>()
                    .Title("[bold underline green] LISTA DE CANCIONES[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(album.Tracks)
                    .UseConverter(choice => $"{choice.Name}"));
    }

    public void Register()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Canción:[/]");
        AnsiConsole.WriteLine();

        var Name = AnsiConsole.Ask<string>("Introduce el nombre de la Canción: ");
        var Duration = AnsiConsole.Ask<int>("Introduce la duración de la Canción: ");
        var ReleaseDate = AnsiConsole.Ask<DateTime>("Introduce la fecha de lanzamiento: ");
        Artist opcionArtist = AnsiConsole.Prompt(
            new SelectionPrompt<Artist>()
                .Title("[bold underline green] LISTA DE ARTISTAS[/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(ArtistService.artists)
                .UseConverter(choice => $"{choice.Name}"));
        
        Album opcionAlbum =AnsiConsole.Prompt(
            new SelectionPrompt<Album>()
                .Title("[bold underline green] LISTA DE ALBUMES[/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(opcionArtist.Albums)
                .UseConverter(choice => $"{choice.Name}"));

        Track track = new Track
        {
            Id = trackService.GetTracks().Count + 1,
            Name = Name,
            IdArtist = opcionArtist.Id,
            IdAlbum = opcionAlbum.Id,
            Duration = Duration,
            ReleaseDate = ReleaseDate,
            Plays = 0,
            SoftDelete = false,
        };
        trackService.AddTrack(track);
    }

}