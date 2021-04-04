using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CinemaApplication;

namespace JsonHandler
{
    public static class JsonFile
    {
        public static void AppendToFile(object value, string filePath)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string oldFile = File.ReadAllText(filePath);
            JsonDocument doc = JsonDocument.Parse(oldFile);
            var docList = new List<JsonElement>(doc.RootElement.EnumerateArray());
            var newItem = JsonSerializer.Serialize(value, options);
            var newItemInJson = JsonDocument.Parse(newItem);
            docList.Add(newItemInJson.RootElement);
            File.WriteAllText(filePath, JsonSerializer.Serialize(docList.ToArray(), options));
        }
    }
}