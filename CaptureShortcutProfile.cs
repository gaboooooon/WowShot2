using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WowShot2
{
	public class CaptureShortcutProfile
	{
		public string ProfileName { get; set; } = "";
		public Keys Key { get; set; } = Keys.None;
		public bool UseCtrl { get; set; } = false;
		public bool UseShift { get; set; } = false;
		public bool UseAlt { get; set; } = false;

		public bool UseDelay { get; set; } = false;
		public int DelaySeconds { get; set; } = 0;

		public string CaptureTarget { get; set; } = "全ディスプレイ"; // 既定値

		public bool SaveToFile { get; set; } = true;
		public string SaveDirectory { get; set; } = "";
		public string FileNameTemplate { get; set; } = "WS%NNNNNN_%YYYY%MM%DD_%hh%mm%ss";
		public string FileFormat { get; set; } = "png";

		public bool RememberLastNumber { get; set; } = false;

		public bool CopyToClipboard { get; set; } = true;

		public int LastUsedNumber { get; set; } = 0;

		[JsonIgnore]
		public int NumberDigitCount
		{
			get
			{
				var match = Regex.Match(FileNameTemplate, @"%N+");
				return match.Success ? match.Value.Length - 1 : 0;
			}
		}
	}
}
