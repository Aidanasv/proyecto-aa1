namespace Services;

using System.Security.Cryptography;
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

    public void UpdateTrack(Track track)
    {
        int index = ArtistService.artists.FindIndex(artistUpdated => track.IdArtist == artistUpdated.Id);
        if (index != -1)
        {
            int indexAlbum = ArtistService.artists[index].Albums.FindIndex(albumUpdated => track.IdAlbum == albumUpdated.Id);

            if (indexAlbum != -1)
            {
                int indexTrack = ArtistService.artists[index].Albums[indexAlbum].Tracks.FindIndex(trackUpdated => track.Id == trackUpdated.Id);
                ArtistService.artists[index].Albums[indexAlbum].Tracks[indexTrack] = track;
                JsonStorage.SaveFile("datos.json", ArtistService.artists);
            }
        }
    }

    public void DeleteTrack(Track track)
    {
        int index = ArtistService.artists.FindIndex(artistUpdated => track.IdArtist == artistUpdated.Id);
        if (index != -1)
        {
            int indexAlbum = ArtistService.artists[index].Albums.FindIndex(albumUpdate => track.IdAlbum == albumUpdate.Id);

            if (indexAlbum != -1)
            {
                int indexTrack = ArtistService.artists[index].Albums[indexAlbum].Tracks.FindIndex(trackUpdated => track.Id == trackUpdated.Id);
                track.SoftDelete = true;
                ArtistService.artists[index].Albums[indexAlbum].Tracks[indexTrack] = track;
                JsonStorage.SaveFile("datos.json", ArtistService.artists);
            }


        }
    }

    public List<Track> GetTracks()
    {
        //Obtiene todos los arrays de los albumes y las canciones y los convierte en un unico array.
        return ArtistService.artists.SelectMany(artist => artist.Albums.SelectMany(album => album.Tracks)).ToList();
    }
}