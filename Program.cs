using Models;
using Services;
using Utils;
using Views;

var ArtistData = JsonStorage.LoadFile<List<Artist>>("datos.json");
if (ArtistData != null)
{
    ArtistService.artists = ArtistData;
}

var UserData = JsonStorage.LoadFile<List<User>>("UserData.json");
if (UserData != null)
{
    UserService.users = UserData;
}

var mainMenu = new MainMenu();
mainMenu.Show();
