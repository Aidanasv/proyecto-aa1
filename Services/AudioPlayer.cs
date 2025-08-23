namespace Services;

using Models;
using NAudio.Wave;

public class AudioPlayer
{
    private static WaveOutEvent? outputDevice;
    private static AudioFileReader? audioFile;
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task PlayAsync(string url)
    {
        try
        {
     

            // Preparar NAudio
            audioFile = new AudioFileReader(url);
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();

            Console.WriteLine("▶️ Reproduciendo preview...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al reproducir el audio: {ex.Message}");
        }
        // Descargar el MP3 en memoria


    }

    public static void Stop()
    {
        if (outputDevice != null)
        {
            outputDevice.Stop();
            outputDevice.Dispose();
            outputDevice = null;
        }

        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }

        Console.WriteLine("⏹️ Reproducción detenida.");
    }
}
