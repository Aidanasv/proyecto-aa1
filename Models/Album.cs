namespace Models;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int IdArtist { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Track> Tracks { get; set; }
    public int Duration { get; set; } // 
    public bool SoftDelete { get; set; }
}