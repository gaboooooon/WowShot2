using System.IO;
using System;
using System.Text.Json;

namespace WowShot2
{
	public class CaptureSettings
	{
		public string SaveDirectory { get; set; } = "";
		public string SaveFormat { get; set; } = "png"; // png / jpg / bmp

		private static string ConfigPath => Path.Combine(AppContext.BaseDirectory, "settings.json");

		public static CaptureSettings Load()
		{
			if (File.Exists(ConfigPath))
			{
				string json = File.ReadAllText(ConfigPath);
				return JsonSerializer.Deserialize<CaptureSettings>(json);
			}
			return new CaptureSettings();
		}

		public void Save()
		{
			string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(ConfigPath, json);
		}
	}
}

