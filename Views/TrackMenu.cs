namespace Utils;

using System.Globalization;
using Models;
using Services;
using Spectre.Console;

public class TrackMenu
{
    private TrackService trackService = new();

    //Mostrar canciones por albúm
    public void ShowTracksByAlbum(Album album)
    {
        var tracks = album.Tracks.ToList();
        var back = new Track { Id = -1, Name = "🔙 Volver" };
        tracks.Add(back);
        var isEnd = true;

        do
        {
            Track opcionTrack = AnsiConsole.Prompt(
                new SelectionPrompt<Track>()
                    .Title("[bold underline green] LISTA DE CANCIONES[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(tracks)
                    .UseConverter(choice => $"🎵 {choice.Name}"));

            if (opcionTrack.Id == -1)
            {
                isEnd = false;
            }
            else
            {
                ActionsToTrack(opcionTrack);
            }
        } while (isEnd);
        AnsiConsole.Clear();

    }

    //Mostrar canciones por playlist
    public void ShowTracksByPlaylists(Playlist playlist)
    {

        var tracks = playlist.Tracks.ToList();
        var back = new Track { Id = -1, Name = "🔙 Volver" };
        tracks.Add(back);

        var isEnd = true;

        do
        {
            Track opcionTrack = AnsiConsole.Prompt(
                new SelectionPrompt<Track>()
                    .Title("[bold underline green] LISTA DE CANCIONES[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(tracks)
                    .UseConverter(choice => $"🎵 {choice.Name}"));

            if (opcionTrack.Id == -1)
            {
                isEnd = false;
            }
            else
            {
                string previewUrl = opcionTrack.Link;
                if (previewUrl != null)
                {
                    AudioPlayer.PlayAsync(previewUrl).GetAwaiter();

                    Console.WriteLine("Presiona una tecla para detener...");
                    Console.ReadKey();

                    AudioPlayer.Stop();
                }
            }
        } while (isEnd);
    }

    //Mostrar detalles de canciones
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

    //Acciones para canciones
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

        //si el usuario actual es nulo o no es admin solo se muestran estas opciones
        if (UserService.currentUser == null || UserService.currentUser.IsAdmin == 1)
        {
            opcions = new Dictionary<int, string>
            {
                { 1, "▶️ Reproducir" },
                { 4, "🔙 Volver"},
            };
        }

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
                    string previewUrl = track.Link;
                    if (previewUrl != null)
                    {
                        AudioPlayer.PlayAsync(previewUrl).GetAwaiter();

                        Console.WriteLine("Presiona una tecla para detener...");
                        Console.ReadKey();

                        AudioPlayer.Stop();
                    }
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

    //Registrar nueva canción
    public void Register()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Canción:[/]");
        AnsiConsole.WriteLine();

        if (ArtistService.artists.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]❌ Debe agregar algún artista antes.[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
            return;
        }


        var Name = AnsiConsole.Ask<string>("Introduce el nombre de la Canción: ");
        var Duration = AnsiConsole.Ask<int>("Introduce la duración de la Canción: ");

        var fecha = AnsiConsole.Prompt(
            new TextPrompt<string>("Introduce la fecha de lanzamiento en formato [yellow]dd-MM-yyyy[/]:")
                .Validate(input =>
                {
                    return DateTime.TryParseExact(
                        input,
                        "dd-MM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Formato inválido. Usa dd-MM-yyyy[/]");
                })
        );

        // Convertimos el string validado a DateTime
        var ReleaseDate = DateTime.ParseExact(fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        AnsiConsole.WriteLine();

        Artist opcionArtist = AnsiConsole.Prompt(
            new SelectionPrompt<Artist>()
                .Title("[bold underline green] LISTA DE ARTISTAS[/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(ArtistService.artists)
                .UseConverter(choice => $"{choice.Name}"));

        if (opcionArtist.Albums.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]❌ Debe agregar algún albúm al artista.[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
            return;
        }

        Album opcionAlbum = AnsiConsole.Prompt(
            new SelectionPrompt<Album>()
                .Title("[bold underline green] LISTA DE ALBUMES[/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(opcionArtist.Albums)
                .UseConverter(choice => $"{choice.Name}"));

        string? url = trackService.GetTrackFromAPI(Name + " " + opcionArtist.Name).Result;
        if (url == null)
        {
            AnsiConsole.MarkupLine($"[red]❌ Error al buscar pista de la canción.[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
            return;
        }

        Track track = new Track
        {
            Id = trackService.GetTracks().Count + 1,
            Name = Name,
            IdArtist = opcionArtist.Id,
            IdAlbum = opcionAlbum.Id,
            Duration = Duration,
            ReleaseDate = ReleaseDate,
            Link = url,
            Plays = 0,
            SoftDelete = false,
        };
        trackService.AddTrack(track);
        AnsiConsole.MarkupLine("[green]✅ Canción registrada correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    //Modificar canción
    public void Update(Track track)
    {
        track.Name = AnsiConsole.Ask<string>("Nuevo nombre:", track.Name);
        track.Duration = AnsiConsole.Ask<int>("Nueva duración:", track.Duration);
        var fecha = AnsiConsole.Prompt(
            new TextPrompt<string>("Nueva fecha de lanzamiento en formato [yellow]dd-MM-yyyy[/]:")
                .Validate(input =>
                {
                    return DateTime.TryParseExact(
                        input,
                        "dd-MM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Formato inválido. Usa dd-MM-yyyy[/]");
                })
        );

        // Convertimos el string validado a DateTime
        track.ReleaseDate = DateTime.ParseExact(fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        track.SoftDelete = !AnsiConsole.Confirm("¿Activo?", !track.SoftDelete);

        trackService.UpdateTrack(track);
        AnsiConsole.MarkupLine("[green]✅ Canción modificada correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    //Eliminar canción
    public void Delete(Track track)
    {
        bool confirm = AnsiConsole.Confirm($"¿Seguro que deseas eliminar a [red]{track.Name}[/]?");
        if (confirm
        )
        {
            trackService.DeleteTrack(track);
            AnsiConsole.MarkupLine("[red]🗑️ Canción eliminada.[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]🚫 Acción cancelada por el usuario.[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
        }
    }

}