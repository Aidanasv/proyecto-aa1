namespace Services;

using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
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
                                Logger.LoggerApp.LogInformation("🚀 Canción añadida");
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
                Logger.LoggerApp.LogInformation("🚀 Cancion modificada");
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
                Logger.LoggerApp.LogInformation("🚀 Canción eliminada");
                JsonStorage.SaveFile("datos.json", ArtistService.artists);
            }


        }
    }

    public List<Track> GetTracks()
    {
        //Obtiene todos los arrays de los albumes y las canciones y los convierte en un unico array.
        Logger.LoggerApp.LogInformation("🚀 Usuarios obtenidos");
        return ArtistService.artists.SelectMany(artist => artist.Albums.SelectMany(album => album.Tracks)).ToList();
    }

    public async Task<string?> GetTrackFromAPI(string trackName)
    {
        try
        {
            HttpClient client = new HttpClient();

            string url = "https://api.deezer.com/search?q=" + trackName;
            string responseBody = await client.GetStringAsync(url);
            DataApi dataApi = JsonSerializer.Deserialize<DataApi>(responseBody);
            string urlaudio = dataApi.data[0].preview;
            var audiobytes = await client.GetByteArrayAsync(urlaudio);
            var nameClear = trackName.Replace(" ","");
            string filePath = Path.Combine("Previews", $"{nameClear}.mp3");

            var previewsPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? "/app/data/data";
            Directory.CreateDirectory(previewsPath + "/Previews");

            await File.WriteAllBytesAsync(previewsPath + "/" + filePath , audiobytes);

            Logger.LoggerApp.LogInformation("🚀 Url obtenida");
            return previewsPath + "/" + filePath;
        }
        catch (Exception ex)
        {
            Logger.LoggerApp.LogError("🚀 Error al obtener url: " + ex.Message);
            return null;
        }

    }
}