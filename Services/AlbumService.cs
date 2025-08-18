namespace Services;

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
                }
            }
        );
        JsonStorage.SaveFile("datos.json", ArtistService.artists);
    }
    
    public List<Album> GetAlbums()
    {
        //Obtiene todos los arrays de los albumes y los convierte en un unico array
        return ArtistService.artists.SelectMany(artist => artist.Albums).ToList();
    }
}