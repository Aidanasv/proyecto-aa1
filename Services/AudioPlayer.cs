namespace Services;

using Models;
using NAudio.Wave;
using Spectre.Console;

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
            Console.WriteLine("▶️ Simulando audio ...");
            Thread.Sleep(2000);
            AnsiConsole.Clear();
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
