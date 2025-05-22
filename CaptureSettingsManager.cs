using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WowShot2
{
	public class CaptureSettingsManager
	{
		public List<CaptureShortcutProfile> Profiles { get; set; } = new();

		private static string ConfigPath => Path.Combine(AppContext.BaseDirectory, "profiles.json");

		public void Save()
		{
			var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
			{
				WriteIndented = true
			});
			File.WriteAllText(ConfigPath, json);
		}

		public static CaptureSettingsManager Load()
		{
			if (File.Exists(ConfigPath))
			{
				string json = File.ReadAllText(ConfigPath);
				return JsonSerializer.Deserialize<CaptureSettingsManager>(json);
			}

			return new CaptureSettingsManager();
		}

		public CaptureShortcutProfile? FindByKey(Keys key, bool ctrl, bool shift, bool alt)
		{
			return Profiles.FirstOrDefault(p =>
				p.Key == key &&
				p.UseCtrl == ctrl &&
				p.UseShift == shift &&
				p.UseAlt == alt);
		}
	}
}
