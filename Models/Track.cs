namespace Models;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int IdAlbum { get; set; }
    public int IdArtist { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Plays { get; set; }
    public bool SoftDelete { get; set; }

}