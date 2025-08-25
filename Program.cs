using Microsoft.Extensions.Logging;
using Models;
using Services;
using Utils;
using Views;

Logger.LoggerApp.LogInformation("🚀 Aplicación iniciada");


var ArtistData = JsonStorage.LoadFile<List<Artist>>("datos.json");
if (ArtistData != null)
{
    ArtistService.artists = ArtistData;
    Logger.LoggerApp.LogInformation("🚀 Cargando data de artistas");
}

var UserData = JsonStorage.LoadFile<List<User>>("UserData.json");
if (UserData != null)
{
    UserService.users = UserData;
    Logger.LoggerApp.LogInformation("🚀 Cargando data de usuarios");
}

var mainMenu = new MainMenu();
mainMenu.Show();
