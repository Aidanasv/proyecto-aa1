namespace Utils;

using Models;
using Services;
using Spectre.Console;

public class AlbumMenu
{
    private AlbumService albumService = new();
    private TrackMenu trackMenu = new();

    public void ShowAlbumsByArtist(Artist artist)
    {
        if (artist.Albums == null || artist.Albums.Count == 0)
    {
        AnsiConsole.MarkupLine($"[red]❌ El artista [bold]{artist.Name}[/] no tiene álbumes registrados.[/]");
        return;
    }

        var albums = artist.Albums.ToList();
        var back = new Album { Id = -1, Name = "🔙 Volver" };
        albums.Add(back);

        Album opcionAlbum = AnsiConsole.Prompt(
                 new SelectionPrompt<Album>()
                     .Title($"[bold underline green] ÁLBUMES DE {artist.Name.ToUpper()} [/]")
                     .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                     .AddChoices(albums)
                     .UseConverter(choice => $"{choice.Name}"));

        if (opcionAlbum.Id == -1)
        {
            return;
        }

        ActionsToAlbums(opcionAlbum);
    }

    public void ShowAlbumDetails(Album album)
    {
        var details = new Panel(
            $"[bold cyan]🎤 Nombre:[/] {album.Name}\n" +
            $"[bold cyan]💿 Duración:[/] {album.Duration}\n" +
            $"[bold cyan]📅 Fecha de creación:[/] {album.ReleaseDate:dd/MM/yyyy}\n" +
            $"[bold cyan]🗑️ Activo:[/] {(album.SoftDelete ? "[red]❌ No[/]" : "[green]✅ Sí[/]")}\n" +
            $"[bold cyan]💿 Canciones:[/] {(album.Tracks != null ? album.Tracks.Count : 0)}"
        )

        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader($"[bold yellow]👤 {album.Name}[/]"),
            Padding = new Padding(2, 1, 2, 1),
            Expand = true
        };

        AnsiConsole.Write(details);
    }

    public void ActionsToAlbums(Album album)
    {
        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "✏️ Modificar" },
                { 2, "🗑️ Eliminar" },
                { 3, "📀 Ver Canciones" },
                { 4, "🔙 Volver"},
            };

        while (isEnd)
        {
            AnsiConsole.Clear();
            
            ShowAlbumDetails(album);

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold yellow]¿Qué deseas hacer con este álbum?[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    Update(album);
                    break;
                case 2:
                    Delete(album);
                    break;
                case 3:
                    trackMenu.ShowTracksByAlbum(album);
                    break;
                case 4:
                    isEnd = false;
                    break;
            }
        }
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

    public void Update(Album album)
    {
        album.Name = AnsiConsole.Ask<string>("Nuevo nombre:", album.Name);
        album.Duration = AnsiConsole.Ask<int>("Nueva duración:", album.Duration);
        album.ReleaseDate = AnsiConsole.Ask<DateTime>("Fecha de Lanzamiento:", album.ReleaseDate);
        album.SoftDelete = !AnsiConsole.Confirm("¿Activo?", !album.SoftDelete);

        albumService.UpdateAlbum(album);
        AnsiConsole.MarkupLine("[green]✅ Álbum modificado correctamente.[/]");
    }

    public void Delete(Album album)
    {
        bool confirm = AnsiConsole.Confirm($"¿Seguro que deseas eliminar a [red]{album.Name}[/]?");
        if (confirm
        )
        {
            albumService.DeleteAlbum(album);
            AnsiConsole.MarkupLine("[red]🗑️ Álbum eliminado.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]🚫 Acción cancelada por el usuario.[/]");
        }
    }
}