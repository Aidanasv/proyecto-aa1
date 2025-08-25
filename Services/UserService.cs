namespace Services;

using Microsoft.Extensions.Logging;
using Models;
using Utils;

public class UserService
{
    public static User? currentUser { get; set; }
    public static List<User> users = new List<User>();
    public void AddUser(User user)
    {
        users.Add(user);
        Logger.LoggerApp.LogInformation("🚀 Usuario añadido");
        JsonStorage.SaveFile("UserData.json", users);
    }

    public void UpdateUser(User user)
    {
        int index = users.FindIndex(userUpdated => user.Id == userUpdated.Id);
        if (index != -1)
        {
            users[index] = user;
            Logger.LoggerApp.LogInformation("🚀 Usuario modificado");
            JsonStorage.SaveFile("UserData.json", users);
        }
    }

    public void SearchUser(string username, string password)
    {
        User? user = users.Find(user => user.Username == username && user.Password == password);
        currentUser = user;
        Logger.LoggerApp.LogInformation("🚀 Busqueda de usuarios");
    }
}