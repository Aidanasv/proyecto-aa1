namespace Utils;

using Models;
using Services;
using Spectre.Console;
using System.Threading;

public class AlbumMenu
{
    private AlbumService albumService = new();
    private TrackMenu trackMenu = new();

    //Mostrar albumes segun artista
    public void ShowAlbumsByArtist(Artist artist)
    {
        if (artist.Albums == null || artist.Albums.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]❌ El artista [bold]{artist.Name}[/] no tiene álbumes registrados.[/]");
            Thread.Sleep(2000);
            return;
        }

        var albums = artist.Albums.ToList();
        var back = new Album { Id = -1, Name = "🔙 Volver" };
        albums.Add(back);
        var isEnd = true;

        do
        {
            Album opcionAlbum = AnsiConsole.Prompt(
                  new SelectionPrompt<Album>()
                      .Title($"[bold underline green] ÁLBUMES DE {artist.Name.ToUpper()} [/]")
                      .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                      .AddChoices(albums)
                      .UseConverter(choice => $"{choice.Name}"));

            if (opcionAlbum.Id == -1)
            {
                isEnd = false;
            }
            else
            {
                ActionsToAlbums(opcionAlbum);
            }

        } while (isEnd);
    }

    //Mostrar detalle de albumes
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

    //Acciones para albumes
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

        if (UserService.currentUser == null || UserService.currentUser.IsAdmin == 1)
        {
            opcions = new Dictionary<int, string>
            {
                { 3, "📀 Ver canciones" },
                { 4, "🔙 Volver"},
            };
        }

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

    //Registrar album nuevo
    public void Register()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Album:[/]");
        AnsiConsole.WriteLine();

        if (ArtistService.artists.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]❌ Debe agregar algún artista antes.[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
            return;
        }

        var Name = AnsiConsole.Ask<string>("Introduce el nombre del Album: ");
        var ReleaseDate = AnsiConsole.Ask<DateTime>("Introduce la fecha de lanzamiento (mm/dd/aaaa): ");
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

    //Modificar albúm
    public void Update(Album album)
    {
        album.Name = AnsiConsole.Ask<string>("Nuevo nombre:", album.Name);
        album.Duration = AnsiConsole.Ask<int>("Nueva duración:", album.Duration);
        album.ReleaseDate = AnsiConsole.Ask<DateTime>("Fecha de Lanzamiento (mm/dd/aaaa):", album.ReleaseDate);
        album.SoftDelete = !AnsiConsole.Confirm("¿Activo?", !album.SoftDelete);

        albumService.UpdateAlbum(album);
        AnsiConsole.MarkupLine("[green]✅ Álbum modificado correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    //Eliminar album 
    public void Delete(Album album)
    {
        bool confirm = AnsiConsole.Confirm($"¿Seguro que deseas eliminar a [red]{album.Name}[/]?");
        if (confirm
        )
        {
            albumService.DeleteAlbum(album);
            AnsiConsole.MarkupLine("[red]🗑️ Álbum eliminado.[/]");
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