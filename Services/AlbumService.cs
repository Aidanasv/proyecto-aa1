namespace Services;

using Microsoft.Extensions.Logging;
using Models;
using Utils;

public class AlbumService
{
    private ArtistService artistService = new();

    public void AddAlbum(Album album)
    {
        //Si el id del artista es igual al id del artista del album se agrega el album el artista
        ArtistService.artists.ForEach(
            artist =>
            {
                if (artist.Id == album.IdArtist)
                {
                    artist.Albums.Add(album);
                    Logger.LoggerApp.LogInformation("🚀 Album añadido");
                }
            }
        );
        JsonStorage.SaveFile("datos.json", ArtistService.artists);
    }

    public void UpdateAlbum(Album album)
    {
        int index = ArtistService.artists.FindIndex(artistUpdated => album.IdArtist == artistUpdated.Id);
        if (index != -1)
        {
            int indexAlbum = ArtistService.artists[index].Albums.FindIndex(albumUpdate => album.Id == albumUpdate.Id);

            ArtistService.artists[index].Albums[indexAlbum] = album;
            Logger.LoggerApp.LogInformation("🚀 Album modificado");
            JsonStorage.SaveFile("datos.json", ArtistService.artists);
        }
    }

    public void DeleteAlbum(Album album)
    {
        int index = ArtistService.artists.FindIndex(artistUpdated => album.IdArtist == artistUpdated.Id);
        if (index != -1)
        {
            int indexAlbum = ArtistService.artists[index].Albums.FindIndex(albumUpdate => album.Id == albumUpdate.Id);

            album.SoftDelete = true;
            ArtistService.artists[index].Albums[indexAlbum] = album;
            Logger.LoggerApp.LogInformation("🚀 Album eliminado");
            JsonStorage.SaveFile("datos.json", ArtistService.artists);
        }
    }
    
    public List<Album> GetAlbums()
    {
        //Obtiene todos los arrays de los albumes y los convierte en un unico array
        return ArtistService.artists.SelectMany(artist => artist.Albums).ToList();
    }
}