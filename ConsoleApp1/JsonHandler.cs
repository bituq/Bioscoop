using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JsonHandler
{
    public static class JsonFile
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private static void OverwriteFile(string filePath, List<JsonElement> docList, JsonSerializerOptions options) =>
            File.WriteAllText(filePath, JsonSerializer.Serialize(docList.ToArray(), options));

        public static List<JsonElement> FileAsList(string filePath)
        {
            var doc = JsonDocument.Parse(File.ReadAllText(filePath));
            return new List<JsonElement>(doc.RootElement.EnumerateArray());
        }

        public static void AppendToFile(object value, string filePath)
        {
            var newItem = JsonDocument.Parse(JsonSerializer.Serialize(value, options));
            var docList = FileAsList(filePath);
            docList.Add(newItem.RootElement[0]);
            OverwriteFile(filePath, docList, options);
        }

        public static void RemoveFromFile(string key, string value, string filePath)
        {
            var oldList = FileAsList(filePath);
            var newList = new List<JsonElement>(oldList);
            foreach (JsonElement obj in oldList)
            {
                if (obj.GetProperty(key).ToString() == value)
                    newList.Remove(obj);
            }
            OverwriteFile(filePath, newList, options);
        }
    }
}