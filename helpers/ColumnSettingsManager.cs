using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using MaliyeHesaplama.models;

namespace MaliyeHesaplama.helpers
{
    public static class ColumnSettingsManager
    {
        private static readonly ConcurrentDictionary<string, List<ColumnSetting>> _cache = new();
        private static readonly string _basePath;

        static ColumnSettingsManager()
        {
            _basePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MaliyeHesaplama", "ColumnSettings");
            Directory.CreateDirectory(_basePath);
        }

        private static string GetCacheKey(string screenName, string gridName, int userId)
            => $"{screenName}|{gridName}|{userId}";

        private static string GetFilePath(string screenName, string gridName, int userId)
            => Path.Combine(_basePath, $"{GetCacheKey(screenName, gridName, userId)}.json");

        public static List<ColumnSetting> Load(string screenName, string gridName, int userId)
        {
            var key = GetCacheKey(screenName, gridName, userId);

            if (_cache.TryGetValue(key, out var cached))
                return cached;

            var filePath = GetFilePath(screenName, gridName, userId);
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    var settings = JsonSerializer.Deserialize<List<ColumnSetting>>(json);
                    if (settings != null)
                    {
                        _cache[key] = settings;
                        return settings;
                    }
                }
                catch { }
            }

            return new List<ColumnSetting>();
        }

        public static void Save(string screenName, string gridName, int userId, List<ColumnSetting> settings)
        {
            var key = GetCacheKey(screenName, gridName, userId);
            _cache[key] = settings;

            var filePath = GetFilePath(screenName, gridName, userId);
            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch { }
        }

        public static void ClearCache(string screenName, string gridName, int userId)
        {
            var key = GetCacheKey(screenName, gridName, userId);
            _cache.TryRemove(key, out _);
        }

        public static void ClearAllCache()
        {
            _cache.Clear();
        }
    }
}
