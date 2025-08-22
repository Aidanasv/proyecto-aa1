namespace Utils;

using Microsoft.VisualBasic.FileIO;
using Models;
using Services;
using Spectre.Console;

public class TrackMenu
{
    private TrackService trackService = new();

    public void ShowTracksByAlbum(Album album)
    {
        var tracks = album.Tracks.ToList();
        var back = new Track { Id = -1, Name = "🔙 Volver al menú anterior" };
        tracks.Add(back);


        Track opcionTrack = AnsiConsole.Prompt(
                new SelectionPrompt<Track>()
                    .Title("[bold underline green] LISTA DE CANCIONES[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(tracks)
                    .UseConverter(choice => $"🎵 {choice.Name}"));

        if (opcionTrack.Id == -1)
        {
            return;
        }

        ActionsToTrack(opcionTrack);
    }

    public void ShowTracksByPlaylists(Playlist playlist)
    {
        var tracks = playlist.Tracks.ToList();
        var back = new Track { Id = -1, Name = "🔙 Volver al menú anterior" };
        tracks.Add(back);

        Track opcionTrack = AnsiConsole.Prompt(
                new SelectionPrompt<Track>()
                    .Title("[bold underline green] LISTA DE CANCIONES[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(tracks)
                    .UseConverter(choice => $"🎵 {choice.Name}"));

        if (opcionTrack.Id == -1)
        {
            return;
        }
    }

    public void ShowTrackDetails(Track track)
    {
        var details = new Panel(
            $"[bold cyan]🎤 Nombre:[/] {track.Name}\n" +
            $"[bold cyan]💿 Duración:[/] {track.Duration}\n" +
            $"[bold cyan]📅 Fecha de lanzamiento:[/] {track.ReleaseDate:dd/MM/yyyy}\n" +
            $"[bold cyan]💿 Reproducciones:[/] {track.Plays}\n" +
            $"[bold cyan]🗑️ Activo:[/] {(track.SoftDelete ? "[red]❌ No[/]" : "[green]✅ Sí[/]")}\n"

        )

        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader($"[bold yellow] {track.Name}[/]"),
            Padding = new Padding(2, 1, 2, 1),
            Expand = true
        };

        AnsiConsole.Write(details);
    }

    public void ActionsToTrack(Track track)
    {
        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "▶️ Reproducir"},
                { 2, "✏️ Modificar" },
                { 3, "🗑️ Eliminar" },
                { 4, "🔙 Volver"},
            };

        while (isEnd)
        {
            AnsiConsole.Clear();

            ShowTrackDetails(track);

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold yellow]¿Qué deseas hacer con esta canción?[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    break;
                case 2:
                    Update(track);
                    break;
                case 3:
                    Delete(track);
                    break;
                case 4:
                    isEnd = false;
                    break;
            }
        }
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

        Album opcionAlbum = AnsiConsole.Prompt(
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
    
    public void Update(Track track)
    {
        track.Name = AnsiConsole.Ask<string>("Nuevo nombre:", track.Name);
        track.Duration = AnsiConsole.Ask<int>("Nueva duración:", track.Duration);
        track.ReleaseDate = AnsiConsole.Ask<DateTime>("Fecha de Lanzamiento:", track.ReleaseDate);
        track.SoftDelete = !AnsiConsole.Confirm("¿Activo?", !track.SoftDelete);

        trackService.UpdateTrack(track);
        AnsiConsole.MarkupLine("[green]✅ Canción modificada correctamente.[/]");
    }

    public void Delete(Track track)
    {
        bool confirm = AnsiConsole.Confirm($"¿Seguro que deseas eliminar a [red]{track.Name}[/]?");
        if (confirm
        )
        {
            trackService.DeleteTrack(track);
            AnsiConsole.MarkupLine("[red]🗑️ Canción eliminada.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]🚫 Acción cancelada por el usuario.[/]");
        }
    }

}