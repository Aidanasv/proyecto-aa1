namespace Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastLogin { get; set; }
    public int IsAdmin { get; set; }
    public List<Playlist> Playlists { get; set; }

    
}