namespace Models;

public class Playlist
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Track> Tracks { get; set; }
    public bool SoftDelete { get; set; }

}