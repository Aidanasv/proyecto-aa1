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

    public void UpdateArtist(Artist artist)
    {
        int index = artists.FindIndex(artistUpdated => artist.Id == artistUpdated.Id);
        if (index != -1)
        {
            artists[index] = artist;
            JsonStorage.SaveFile("datos.json", artists);
        }
    }

    public void DeleteArtist(Artist artist)
    {
        int index = artists.FindIndex(artistUpdated => artist.Id == artistUpdated.Id);
        if (index != -1)
        {
            artist.SoftDelete = true;
            artists[index] = artist;
            JsonStorage.SaveFile("datos.json", artists);
        }
    }
}