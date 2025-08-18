namespace Services;

using Models;
using Utils;

public class UserService
{
    public User? currentUser { get; set; }
    public static List<User> users = new List<User>();
    public void AddUser(User user)
    {
        users.Add(user);
        JsonStorage.SaveFile("UserData.json", users);
    }

    public void UpdateUser(User user, int id)
    {

    }

    public void SearchUser(string username, string password)
    {
        User? user = users.Find(user => user.Username == username && user.Password == password);
        Console.WriteLine(user?.Email);
        this.currentUser = user;
    }
}