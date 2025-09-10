using System.IO;
using System.Text.Json;

public class DbConfig
{
    public string DbType { get; set; }
    public string ConnectionString { get; set; }

    public static DbConfig Load(string filePath = "dbconfig.json")
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<DbConfig>(json);
    }
}
