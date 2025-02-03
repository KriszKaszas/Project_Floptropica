using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace Project_Floptropica.Services
{
    public class SoundParser
    {
        private static readonly string[] AllowedExtensions = { ".mp3", ".ogg", ".wav" };

        public static void GenerateSoundsJson(string folderPath, string outputFilePath)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder does not exist: " + folderPath);
                return;
            }

            var categories = new Dictionary<string, List<SoundItem>>();

            foreach (var file in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
            {
                if (!AllowedExtensions.Contains(Path.GetExtension(file).ToLower())) continue;

                var relativePath = Path.GetRelativePath(folderPath, file).Replace("\\", "/");
                var category = Path.GetDirectoryName(relativePath)?.Replace("\\", "/") ?? "Uncategorized";

                if (!categories.ContainsKey(category))
                {
                    categories[category] = new List<SoundItem>();
                }

                categories[category].Add(new SoundItem
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    File = relativePath
                });
            }

            var json = JsonSerializer.Serialize(new SoundData { Categories = categories }, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(outputFilePath, json);
            Console.WriteLine("sounds.json updated successfully.");
        }

        private class SoundItem
        {
            public string Name { get; set; } = "";
            public string File { get; set; } = "";
        }

        private class SoundData
        {
            public Dictionary<string, List<SoundItem>> Categories { get; set; } = new();
        }
    }
}
