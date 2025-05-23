using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace WowShot2
{
	public class CaptureSettingsManager
	{
		public int GlobalLastUsedNumber { get; set; } = 1;
		public bool RememberGlobalLastUsedNumber { get; set; } = false;
		public bool ShowCaptureNotification { get; set; } = true;

		public List<CaptureShortcutProfile> Profiles { get; set; } = new();

		private static string ConfigPath => Path.Combine(AppContext.BaseDirectory, "profiles.json");

		public void Save()
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

			string json = JsonSerializer.Serialize(this, options);
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
