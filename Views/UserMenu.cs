namespace Views;

using System.ComponentModel;
using Models;
using Services;
using Spectre.Console;
using Utils;

public class UserMenu
{
    private UserService userService = new();
    private ArtistMenu artistMenu = new();
    private AlbumMenu albumMenu = new();
    private TrackMenu trackMenu = new();
    private PlaylistMenu playlistMenu = new();
    public void Register()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Usuario:[/]");
        AnsiConsole.WriteLine();

        var Email = AnsiConsole.Ask<string>("Introduce tu correo: ");
        var Name = AnsiConsole.Ask<string>("Introduce tu nombre: ");
        var Username = AnsiConsole.Ask<string>("Introduce tu nombre de usuario: ");
        var Password = AnsiConsole.Ask<string>("Introduce tu contraseña: ");
        var ValidPassword = "";
        do
        {
            ValidPassword = AnsiConsole.Ask<string>("Introduce nuevamente tu contraseña: ");
        } while (Password != ValidPassword);



        var BirthDate = AnsiConsole.Ask<DateTime>("Introduce tu fecha de nacimiento (dd/mm/aaaa): ");

        User user = new User
        {
            Id = UserService.users.Count + 1,
            Name = Name,
            Username = Username,
            Email = Email,
            Password = Password,
            BirthDate = BirthDate,
            CreateDate = DateTime.Today,
            LastLogin = DateTime.Today,
            IsAdmin = 0,
            Playlists = []
        };
        userService.AddUser(user);

    }
    public void Login()
    {
        AnsiConsole.MarkupLine("[bold underline green]Iniciar Sesión:[/]");
        AnsiConsole.WriteLine();

        var Username = AnsiConsole.Ask<string>("Introduce tu nombre de usuario: ");
        var Password = AnsiConsole.Ask<string>("Introduce tu contraseña: ");

        AnsiConsole.Clear();
        userService.SearchUser(Username, Password);

        if (userService.currentUser is null)
        {
            AnsiConsole.MarkupLine("[underline red]Usuario o Contraseña incorrecta[/]");
        }
        else if (userService.currentUser.IsAdmin == 0)
        {
            ShowAdminMenu();
        }
        else
        {
            ShowUserMenu();
        }
    }

    public void ShowUserMenu()
    {
        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "📂 Ver mis playlists" },
                { 2, "➕ Crear nueva playlist" },
                { 3, "👤 Editar mi perfil"},
                { 4, "🔙 Cerrar sesión"}
            };

        while (isEnd)
        {

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold underline green]🎵 ZONA USUARIO 🎵[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    playlistMenu.ShowPlaylistsByUser(userService.currentUser);
                    break;
                case 2:
                    playlistMenu.CreatePlaylist(userService.currentUser);
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
    }

    public void ShowAdminMenu()
    {
        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "🎤 Ver todos los artistas" },
                { 2, "➕ Crear nuevo artista" },
                { 3, "🆕 Crear nuevo álbum"},
                { 4, "🎵 Crear nueva canción"},
                { 5, "🔙 Cerrar sesión"}
            };

        while (isEnd)
        {

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold underline green]🛠️ PANEL DE ADMINISTRADOR 🛠️[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    artistMenu.ShowArtists();
                    break;
                case 2:
                    artistMenu.Register();
                    break;
                case 3:
                    albumMenu.Register();
                    break;
                case 4:
                    trackMenu.Register();
                    break;
                case 5:
                    break;
            }
        }
    }
}