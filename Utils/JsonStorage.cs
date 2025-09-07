namespace Utils;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Models;

public static class JsonStorage
{
    public static void SaveFile<T>(string filepath, T data)
    {
        var dataPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? "/app/data/data";
        Directory.CreateDirectory(dataPath);

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dataPath + "/" + filepath, json);
        Logger.LoggerApp.LogInformation("🚀 Data guardada");
    }
    public static T? LoadFile<T>(string filepath)
    {
        var dataPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? "/app/data/data";
        
        if (!File.Exists(dataPath + "/" + filepath)) return default;

        var json = File.ReadAllText(dataPath + "/" + filepath);
        Logger.LoggerApp.LogInformation("🚀 Data actualizada");
        return JsonSerializer.Deserialize<T>(json);
    }
}