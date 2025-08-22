namespace Services;

using Models;
using Utils;

public class PlaylistService
{
    public void AddPlaylist(User user, Playlist playlist)
    {
        //Si el id del usuario es igual al id del usuario que quiere agregar la playlist se agrega la playlist al usuario
        UserService.users.ForEach(
            userInList =>
            {
                if (user.Id == userInList.Id)
                {
                    userInList.Playlists.Add(playlist);
                }
            }
        );
        JsonStorage.SaveFile("UserData.json", UserService.users);
    }

    public void UpdatePlaylist(Playlist playlist)
    {
       int index = UserService.users.FindIndex(userUpdated => playlist.IdUser== userUpdated.Id);
        if (index != -1)
        {
            int indexPlaylist = UserService.users[index].Playlists.FindIndex(playlistUpdated => playlist.Id == playlistUpdated.Id);

            UserService.users[index].Playlists[indexPlaylist] = playlist;
            JsonStorage.SaveFile("UserData.json", UserService.users);
        } 
    }

    public void DeletePlaylist(Playlist playlist)
    {
        int index = UserService.users.FindIndex(userUpdated => playlist.IdUser== userUpdated.Id);
        if (index != -1)
        {
            int indexPlaylist = UserService.users[index].Playlists.FindIndex(playlistUpdated => playlist.Id == playlistUpdated.Id);

            UserService.users[index].Playlists.RemoveAt(indexPlaylist);
            JsonStorage.SaveFile("UserData.json", UserService.users);
        }
    }

      public void AddTrackToPlaylist(Track track, Playlist playlist)
    {
        int searchTrack = playlist.Tracks.FindIndex(currentTrack => track.Id == currentTrack.Id);
        if (searchTrack == -1)
        {
            playlist.Tracks.Add(track);
            UpdatePlaylist(playlist);
        }
    }

    public List<Playlist> GetPlaylists()
    {
        //Obtiene todos los arrays de las playlist y los convierte en un unico array
        return UserService.users.SelectMany(user => user.Playlists).ToList();
    }
}