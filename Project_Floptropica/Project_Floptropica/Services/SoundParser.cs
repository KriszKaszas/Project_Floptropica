using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_Floptropica.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Collections.Generic;

    public class SoundParser
    {
        private static readonly string[] AllowedExtensions = { ".mp3", ".ogg", ".wav" }; // Add more if needed

        public static void GenerateSoundsJson(string folderPath, string outputFilePath)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder does not exist: " + folderPath);
                return;
            }

            List<SoundItem> existingSounds = new();
            if (File.Exists(outputFilePath))
            {
                try
                {
                    string existingJson = File.ReadAllText(outputFilePath);
                    var existingData = JsonSerializer.Deserialize<SoundData>(existingJson);
                    if (existingData != null)
                    {
                        existingSounds = existingData.files;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading existing sounds.json: " + ex.Message);
                }
            }

            var newSounds = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => AllowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .Select(file => new SoundItem
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    File = Path.GetRelativePath(folderPath, file).Replace("\\", "/")
                })
                .Where(newSound => !existingSounds.Any(existing => existing.File == newSound.File))
                .ToList();

            if (newSounds.Any())
            {
                existingSounds.AddRange(newSounds);
                var json = JsonSerializer.Serialize(new SoundData { files = existingSounds }, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(outputFilePath, json);
                Console.WriteLine("sounds.json updated successfully.");
            }
            else
            {
                Console.WriteLine("No new sounds to add.");
            }
        }

        private class SoundItem
        {
            public string Name { get; set; } = "";
            public string File { get; set; } = "";
        }

        private class SoundData
        {
            public List<SoundItem> files { get; set; } = new();
        }
    }

}
