namespace Services;

using Microsoft.Extensions.Logging;
using Models;
using Utils;

public class ArtistService
{
    public static List<Artist> artists = new List<Artist>();
    public void AddArtist(Artist artist)
    {
        artists.Add(artist);
        Logger.LoggerApp.LogInformation("🚀 Artista añadido");
        JsonStorage.SaveFile("datos.json", artists);
    }

    public void UpdateArtist(Artist artist)
    {
        int index = artists.FindIndex(artistUpdated => artist.Id == artistUpdated.Id);
        if (index != -1)
        {
            artists[index] = artist;
            Logger.LoggerApp.LogInformation("🚀 Artista modificado");
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
            Logger.LoggerApp.LogInformation("🚀 Artista eliminado");
            JsonStorage.SaveFile("datos.json", artists);
        }
    }
}