namespace Services;

using Models;
using Utils;

public class TrackService
{
    public void AddTrack(Track track)
    //Recorro los artistas y albumes para verificar si coincide el Id, y añado track. 
    {
        ArtistService.artists.ForEach(
             artist =>
             {
                 if (artist.Id == track.IdArtist)
                 {
                     artist.Albums.ForEach(
                         album =>
                         {
                             if (album.Id == track.IdAlbum)
                             {
                                 album.Tracks.Add(track);
                             }
                         }
                     );

                 }
             }
         ); 
        
        JsonStorage.SaveFile("datos.json", ArtistService.artists);
    }
    public List<Track> GetTracks()
    {
         //Obtiene todos los arrays de los albumes y las canciones y los convierte en un unico array.
        return ArtistService.artists.SelectMany(artist => artist.Albums.SelectMany(album => album.Tracks)).ToList();
    }
}