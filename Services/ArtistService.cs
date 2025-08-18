namespace Services;

using Models;
using Utils;

public class ArtistService
{
    public static List<Artist> artists = new List<Artist>();
    public void AddArtist(Artist artist)
    {
        artists.Add(artist);
        JsonStorage.SaveFile("datos.json", artists);
    }


}