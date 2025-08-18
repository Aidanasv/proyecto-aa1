namespace Models;

public class Playlist
{
    public int IdPlaylist { get; set; }
    public int IdUser { get; set; }
    public string Name { get; set; }
    public List<Track> Tracks { get; set; }
    public bool SoftDelete { get; set; }

}