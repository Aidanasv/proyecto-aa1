namespace Utils;

using System.Text.Json;
using Models;

public static class JsonStorage
{
    public static void SaveFile<T>(string filepath, T data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filepath, json);
    }
    public static T? LoadFile<T>(string filepath)
    {
        if (!File.Exists(filepath)) return default;

        var json = File.ReadAllText(filepath);
        return JsonSerializer.Deserialize<T>(json);
    }
}