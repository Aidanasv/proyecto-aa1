namespace Views;

using Models;
using Services;
using Spectre.Console;
using Utils;

public class ArtistMenu
{
    private static ArtistService artistService = new();
    private AlbumMenu albumMenu = new();

    //Mostrar todos los artistas
    public void ShowArtists(string? name = null)
    {

        var artists = ArtistService.artists.Where(artist => name is null || artist.Name.ToLower().Contains(name.ToLower())).ToList();
        var back = new Artist { Id = -1, Name = "🔙 Volver" };
        artists.Add(back);
        var isEnd = true;

        do
        {
            AnsiConsole.Clear();
            Artist opcionArtist = AnsiConsole.Prompt(
                new SelectionPrompt<Artist>()
                    .Title("[bold underline green] LISTA DE ARTISTAS[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(artists)
                    .UseConverter(choice => $"{choice.Name}"));

            if (opcionArtist.Id == -1)
            {
                isEnd = false;
            }
            else
            {
                ActionsToArtists(opcionArtist);
            }
        } while (isEnd);
    }

    //Mostrar el detalle del artista seleccionado
    public void ShowArtistDetails(Artist artist)
    {
        var details = new Panel(
            $"[bold cyan]🎤 Nombre:[/] {artist.Name}\n" +
            $"[bold cyan]👥 Seguidores:[/] {artist.Followers}\n" +
            $"[bold cyan]📖 Biografía:[/] {(!string.IsNullOrWhiteSpace(artist.Biography) ? artist.Biography : "N/D")}\n" +
            $"[bold cyan]📅 Fecha de creación:[/] {artist.CreateDate:dd/MM/yyyy}\n" +
            $"[bold cyan]🗑️ Activo:[/] {(artist.SoftDelete ? "[red]❌ No[/]" : "[green]✅ Sí[/]")}\n" +
            $"[bold cyan]💿 Álbumes publicados:[/] {(artist.Albums != null ? artist.Albums.Count : 0)}"
        )

        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader($"[bold yellow]👤 {artist.Name}[/]"),
            Padding = new Padding(2, 1, 2, 1),
            Expand = true
        };

        AnsiConsole.Write(details);
    }

    //Mostrar menú de acciones en el artista seleccionado
    public void ActionsToArtists(Artist artist)
    {
        bool isEnd = true;
        var opcions = new Dictionary<int, string>
            {
                { 1, "✏️ Modificar" },
                { 2, "🗑️ Eliminar" },
                { 3, "📀 Ver álbumes" },
                { 4, "🔙 Volver"},
            };

        if (UserService.currentUser == null || UserService.currentUser.IsAdmin == 1)
        {
            opcions = new Dictionary<int, string>
            {
                { 3, "📀 Ver álbumes" },
                { 4, "🔙 Volver"},
            };
        }

        while (isEnd)
        {
            AnsiConsole.Clear();
            ShowArtistDetails(artist);

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold yellow]¿Qué deseas hacer con este artista?[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    Update(artist);
                    break;
                case 2:
                    Delete(artist);
                    break;
                case 3:
                    albumMenu.ShowAlbumsByArtist(artist);
                    break;
                case 4:
                    isEnd = false;
                    break;
            }
        }
    }

    //Registrar nuevo artista
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
        AnsiConsole.MarkupLine("[green]✅ Artista registrado correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    //Modificar artista
    public void Update(Artist artist)
    {
        artist.Name = AnsiConsole.Ask<string>("Nuevo nombre:", artist.Name);
        artist.Followers = AnsiConsole.Ask<int>("Seguidores:", artist.Followers);
        artist.Biography = AnsiConsole.Ask<string>("Biografía:", artist.Biography ?? "");
        artist.SoftDelete = !AnsiConsole.Confirm("¿Activo?", !artist.SoftDelete);

        artistService.UpdateArtist(artist);
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green]✅ Artista modificado correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    //Eliminar artista
    public void Delete(Artist artist)
    {
        bool confirm = AnsiConsole.Confirm($"¿Seguro que deseas eliminar a [red]{artist.Name}[/]?");
        if (confirm
        )
        {
            artistService.DeleteArtist(artist);
            AnsiConsole.MarkupLine("[red]🗑️ Artista eliminado.[/]");
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