namespace Views;

using Models;
using NAudio.CoreAudioApi;
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

    //Registrar nuevo usuario
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

        var BirthDate = AnsiConsole.Ask<DateTime>("Introduce tu fecha de nacimiento (mm/dd/aaaa): ");

        //todos los usuarios se inicializan en cliente, si no hay usuarios el primero es admin
        var role = 1;
        if (UserService.users.Count == 0)
        {
            role = 0;
        }
        //si es un usuario admin el que está registrando puede seleccionar si es admin o cliente
        if (UserService.currentUser is not null && UserService.currentUser.IsAdmin == 0 && UserService.users.Count != 0)
        {
            var opcionsRole = new Dictionary<int, string>
            {
                { 0, "✏️ Administrador" },
                { 1, "🗑️ Cliente" },
            };

            role = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold yellow]¿Qué rol tendrá este usuario?[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcionsRole.Keys)
                    .UseConverter(choice => $"{choice}- {opcionsRole[choice]}"));
        }

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
            IsAdmin = role,
            Playlists = []
        };
        userService.AddUser(user);

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[green]✅ Usuario registrado correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
        
    }

    //Modificar usuario
    public void Update(User user)
    {
        user.Name = AnsiConsole.Ask<string>("Nuevo nombre:", user.Name);
        user.Password = AnsiConsole.Ask<string>("Contraseña:", user.Password);
        var ValidPassword = "";
        do
        {
            ValidPassword = AnsiConsole.Ask<string>("Introduce nuevamente tu contraseña: ");
        } while (user.Password != ValidPassword);
        user.BirthDate = AnsiConsole.Ask<DateTime>("Nuevo fecha de nacimiento (mm/dd/aaaa):", user.BirthDate);

        userService.UpdateUser(user);
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[green]✅ Usuario modificado correctamente.[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    //Inicio de sesión
    public void Login()
    {
        AnsiConsole.MarkupLine("[bold underline green]Iniciar Sesión:[/]");
        AnsiConsole.WriteLine();

        var Username = AnsiConsole.Ask<string>("Introduce tu nombre de usuario: ");
        var Password = AnsiConsole.Ask<string>("Introduce tu contraseña: ");

        AnsiConsole.Clear();
        userService.SearchUser(Username, Password);

        if (UserService.currentUser is null)
        {
            AnsiConsole.MarkupLine("[underline red]Usuario o Contraseña incorrecta[/]");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
        }
        else if (UserService.currentUser.IsAdmin == 0)
        {
            ShowAdminMenu();
        }
        else
        {
            ShowUserMenu();
        }
    }

    //Mostrar menú de usuario
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
                    playlistMenu.ShowPlaylistsByUser(UserService.currentUser);
                    break;
                case 2:
                    playlistMenu.CreatePlaylist(UserService.currentUser);
                    break;
                case 3:
                    Update(UserService.currentUser);
                    break;
                case 4:
                    isEnd = false;
                    UserService.currentUser = null;
                    break;
            }
        }
    }

    //Mostrar menú administrador
    public void ShowAdminMenu()
    {
        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "🎤 Ver todos los artistas" },
                { 2, "➕ Crear nuevo artista" },
                { 3, "🆕 Crear nuevo álbum"},
                { 4, "🎵 Crear nueva canción"},
                { 5, "👤 Registrar usuario"},
                { 6, "🔙 Cerrar sesión"}
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
                    Register();
                    break;
                case 6:
                    isEnd = false;
                    UserService.currentUser = null;
                    break;
            }
        }
    }

    //Zona Publica
    public void ShowPublicZone()
    {
        AnsiConsole.Clear();

        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "🔎 Buscador" },
                { 2, "🎤 Artistas" },
                { 3, "🔙 Volver"}
            };

        while (isEnd)
        {

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold underline green]🌐 ZONA PÚBLICA 🌐[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    Search();
                    break;
                case 2:
                    artistMenu.ShowArtists();
                    break;
                case 3:
                    isEnd = false;
                    break;
            }
        }
    }

    //Buscador
    public void Search()
    {
        AnsiConsole.Clear();
        var search = AnsiConsole.Ask<string>("[bold underline green]Introduce un artista:[/]");
        AnsiConsole.Clear();
        AnsiConsole.WriteLine();

        artistMenu.ShowArtists(search);
    }


}