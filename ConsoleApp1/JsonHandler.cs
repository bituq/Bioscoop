using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JsonHandler
{
    public static class JsonFile
    {
        /// <summary>
        /// Options for serializing to JSON.
        /// </summary>
        public static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        private static void OverwriteFile(string filePath, List<JsonElement> docList, JsonSerializerOptions options) =>
            File.WriteAllText(filePath, JsonSerializer.Serialize(docList.ToArray(), options));
        /// <param name="filePath">The path to the file.</param>
        /// <returns><see cref="List{T}"/>All items in the JSON file.</returns>
        public static List<JsonElement> FileAsList(string filePath)
        {
            var doc = JsonDocument.Parse(File.ReadAllText(filePath));
            return new List<JsonElement>(doc.RootElement.EnumerateArray());
        }
        /// <summary>
        /// Use an object with <c>get</c> and <c>set</c> accessors to append to a file.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="filePath"></param>
        public static void AppendToFile(object value, string filePath)
        {
            var docList = FileAsList(filePath);
            var newItem = JsonDocument.Parse(JsonSerializer.Serialize(value, options));
            docList.Add(newItem.RootElement);
            OverwriteFile(filePath, docList, options);
        }
        /// <summary>
        /// Remove the object containing the given value for the given key from the JSON file.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="filePath"></param>
        public static void RemoveFromFile(string key, string value, string filePath)
        {
            var oldList = FileAsList(filePath);
            var newList = new List<JsonElement>(oldList);
            foreach (JsonElement obj in oldList)
            {
                if (obj.GetProperty(key).ToString() == value)
                {
                    newList.Remove(obj);
                    break;
                }
            }
            OverwriteFile(filePath, newList, options);
        }
        /// <summary>
        /// Remove the object containing the given value for the given key from the JSON file.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="filePath"></param>
        public static void RemoveFromFile(string key, int value, string filePath)
        {
            var oldList = FileAsList(filePath);
            var newList = new List<JsonElement>(oldList);
            foreach (JsonElement obj in oldList)
            {
                if (obj.GetProperty(key).GetInt32() == value)
                {
                    newList.Remove(obj);
                    break;
                }
            }
            OverwriteFile(filePath, newList, options);
        }
    }
}