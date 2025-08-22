namespace Views;

using Models;
using Services;
using Spectre.Console;
using Spectre.Console.Rendering;
using Utils;

public class PlaylistMenu
{
    private PlaylistService playlistService = new();
    private TrackMenu trackMenu = new();
    private TrackService trackService = new();


    public void ShowPlaylistsByUser(User user)
    {
        if (user.Playlists == null || user.Playlists.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]❌ No tienes playlists creadas.[/]");
            return;
        }

        var playlists = user.Playlists.ToList();
        var back = new Playlist { Id = -1, Name = "🔙 Volver" };
        playlists.Add(back);

        Playlist opcionPlaylist = AnsiConsole.Prompt(
                 new SelectionPrompt<Playlist>()
                     .Title($"[bold underline green] PlAYLISTS DE {user.Name.ToUpper()} [/]")
                     .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                     .AddChoices(playlists)
                     .UseConverter(choice => $"{choice.Name}"));

        if (opcionPlaylist.Id == -1)
        {
            return;
        }

        ActionsToPlaylists(opcionPlaylist);
    }

    public void ShowPlaylistDetails(Playlist playlist)
    {
        var details = new Panel(
            $"[bold cyan]🎵 Nombre:[/] {playlist.Name}\n" +
            $"[bold cyan]📝 Descripción:[/] {playlist.Description}\n" +
            $"[bold cyan]💿 Canciones:[/] {(playlist.Tracks != null ? playlist.Tracks.Count : 0)}"
        )

        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader($"[bold yellow]🎶 {playlist.Name}[/]"),
            Padding = new Padding(2, 1, 2, 1),
            Expand = true
        };

        AnsiConsole.Write(details);
    }

    public void ActionsToPlaylists(Playlist playlist)
    {
        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "✏️ Modificar" },
                { 2, "🗑️ Eliminar" },
                { 3, "📀 Ver Canciones" },
                { 4, "🎵 Añadir canción"},
                { 5, "🔙 Volver"},
            };

        while (isEnd)
        {
            AnsiConsole.Clear();

            ShowPlaylistDetails(playlist);

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold yellow]¿Qué deseas hacer con esta playlist?[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    Update(playlist);
                    break;
                case 2:
                    bool delete = Delete(playlist);
                    if (delete)
                    {
                        isEnd = false;
                    }
                    break;
                case 3:
                    trackMenu.ShowTracksByPlaylists(playlist);
                    break;
                case 4:
                    AddTracksToPlaylist(playlist);
                    break;
                case 5:
                    isEnd = false;
                    break;
            }
        }
    }

    public void CreatePlaylist(User user)
    {
        AnsiConsole.MarkupLine("[bold underline green]Crear nueva Playlist:[/]");
        AnsiConsole.WriteLine();

        var Name = AnsiConsole.Ask<string>("Introduce el nombre: ");
        var Description = AnsiConsole.Ask<string>("Introduce una descripción: ");

        Playlist playlist = new Playlist
        {
            Id = playlistService.GetPlaylists().Count + 1,
            IdUser = user.Id,
            Name = Name,
            Description = Description,
            SoftDelete = false,
            Tracks = []
        };
        playlistService.AddPlaylist(user, playlist);
    }

    public void Update(Playlist playlist)
    {
        playlist.Name = AnsiConsole.Ask<string>("Nuevo nombre:", playlist.Name);
        playlist.Description = AnsiConsole.Ask<string>("Nueva descripción:", playlist.Description);


        playlistService.UpdatePlaylist(playlist);
        AnsiConsole.MarkupLine("[green]✅ Playlist modificada correctamente.[/]");
    }

    public bool Delete(Playlist playlist)
    {
        bool confirm = AnsiConsole.Confirm($"¿Seguro que deseas eliminar [red]{playlist.Name}[/]?");
        if (confirm
        )
        {
            playlistService.DeletePlaylist(playlist);
            AnsiConsole.MarkupLine("[red]🗑️ Playlist eliminada.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]🚫 Acción cancelada por el usuario.[/]");
        }

        return confirm;
    }

    public void AddTracksToPlaylist(Playlist playlist)
    {
        var tracks = trackService.GetTracks().ToList();
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

        playlistService.AddTrackToPlaylist(opcionTrack, playlist);
    }
}